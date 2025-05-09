using System;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace AndroidDataApp
{
    public partial class DeviceInfoForm : Form
    {
        private string connectionString = "Data Source=localhost;Initial Catalog=AndroidDataDB;Integrated Security=True;";

        public DeviceInfoForm()
        {
            InitializeComponent();
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("CPU", "CPU Info");
            dataGridView1.Columns.Add("Memory", "Memory Info");
            dataGridView1.Columns.Add("Model", "Device Model");
            dataGridView1.Columns.Add("Version", "Android Version");
            dataGridView1.Columns.Add("Battery", "Battery");
            dataGridView1.Columns.Add("IMEI", "IMEI");
            dataGridView1.Columns.Add("Apps", "Installed Apps");
        }

        private void btnLoadData_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog { Filter = "Text Files (*.txt)|*.txt" };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                dataGridView1.Rows.Clear();
                foreach (var line in File.ReadAllLines(ofd.FileName))
                {
                    var parts = line.Split(',');
                    if (parts.Length >= 7)
                        dataGridView1.Rows.Add(
                            parts[0].Trim(), parts[1].Trim(), parts[2].Trim(),
                            parts[3].Trim(), parts[4].Trim(), parts[5].Trim(), parts[6].Trim());
                }
            }
        }

        private void btnSaveToDb_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.IsNewRow) continue;

                    string query = @"INSERT INTO DeviceInfoTable 
                        (CPUInfo, MemoryInfo, Model, AndroidVersion, BatteryStatus, IMEI, InstalledApps) 
                        VALUES (@CPU, @Mem, @Model, @Ver, @Batt, @IMEI, @Apps)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CPU", row.Cells[0].Value);
                        cmd.Parameters.AddWithValue("@Mem", row.Cells[1].Value);
                        cmd.Parameters.AddWithValue("@Model", row.Cells[2].Value);
                        cmd.Parameters.AddWithValue("@Ver", row.Cells[3].Value);
                        cmd.Parameters.AddWithValue("@Batt", row.Cells[4].Value);
                        cmd.Parameters.AddWithValue("@IMEI", row.Cells[5].Value);
                        cmd.Parameters.AddWithValue("@Apps", row.Cells[6].Value);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Device Info Saved.");
            }
        }

        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog { Filter = "CSV Files (*.csv)|*.csv" };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter sw = new StreamWriter(sfd.FileName))
                {
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                        if (!row.IsNewRow)
                            sw.WriteLine($"{row.Cells[0].Value},{row.Cells[1].Value},{row.Cells[2].Value},{row.Cells[3].Value},{row.Cells[4].Value},{row.Cells[5].Value},{row.Cells[6].Value}");
                }
                MessageBox.Show("CSV Report Generated.");
            }
        }
    }
}

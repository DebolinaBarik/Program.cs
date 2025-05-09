using System;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace AndroidDataApp
{
    public partial class CallLogsForm : Form
    {
        private string connectionString = "Data Source=localhost;Initial Catalog=AndroidDataDB;Integrated Security=True;";

        public CallLogsForm()
        {
            InitializeComponent();
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("Number", "Number");
            dataGridView1.Columns.Add("CallType", "Call Type");
            dataGridView1.Columns.Add("Duration", "Duration");
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
                    if (parts.Length == 3)
                        dataGridView1.Rows.Add(parts[0].Trim(), parts[1].Trim(), parts[2].Trim());
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
                    string number = row.Cells[0].Value?.ToString();
                    string type = row.Cells[1].Value?.ToString();
                    string duration = row.Cells[2].Value?.ToString();

                    string query = "INSERT INTO CallLogsTable (Number, CallType, Duration) VALUES (@Number, @Type, @Duration)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Number", number);
                        cmd.Parameters.AddWithValue("@Type", type);
                        cmd.Parameters.AddWithValue("@Duration", duration);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Call Logs Saved.");
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
                            sw.WriteLine($"{row.Cells[0].Value},{row.Cells[1].Value},{row.Cells[2].Value}");
                }
                MessageBox.Show("CSV Report Generated.");
            }
        }
    }
}

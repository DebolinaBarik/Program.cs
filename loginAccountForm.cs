using System;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace AndroidDataApp
{
    public partial class AccountsForm : Form
    {
        private string connectionString = "Data Source=localhost;Initial Catalog=AndroidDataDB;Integrated Security=True;";

        public AccountsForm()
        {
            InitializeComponent();
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("Account", "Account Name");
            dataGridView1.Columns.Add("Email", "Email Address");
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
                    if (parts.Length == 2)
                        dataGridView1.Rows.Add(parts[0].Trim(), parts[1].Trim());
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
                    string name = row.Cells[0].Value?.ToString();
                    string email = row.Cells[1].Value?.ToString();

                    string query = "INSERT INTO LoginAccountsTable (AccountName, EmailAddress) VALUES (@Name, @Email)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Name", name);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Accounts Saved.");
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
                            sw.WriteLine($"{row.Cells[0].Value},{row.Cells[1].Value}");
                }
                MessageBox.Show("CSV Report Generated.");
            }
        }
    }
}

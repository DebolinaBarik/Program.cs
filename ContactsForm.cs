using System;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace AndroidDataExtractor
{
    public partial class ContactsForm : Form
    {
        // Connection String to SQL Server
        private string connectionString = "Data Source=localhost;Initial Catalog=AndroidDataDB;Integrated Security=True;";

        public ContactsForm()
        {
            InitializeComponent();
            InitializeGrid();
        }

        // Initialize the DataGridView with columns
        private void InitializeGrid()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("Name", "Name");
            dataGridView1.Columns.Add("PhoneNumber", "Phone Number");
            dataGridView1.Columns.Add("Email", "Email");
        }

        // Load Data from .txt into DataGridView
        private void btnLoadData_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog
            {
                Filter = "Text Files (*.txt)|*.txt",
                Title = "Select Contact File"
            };

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                dataGridView1.Rows.Clear();
                string[] lines = File.ReadAllLines(openFile.FileName);

                foreach (string line in lines)
                {
                    string[] parts = line.Split(',');
                    if (parts.Length == 3)
                    {
                        dataGridView1.Rows.Add(parts[0].Trim(), parts[1].Trim(), parts[2].Trim());
                    }
                }
            }
        }

        // Save DataGridView rows to SQL Server
        private void btnSaveToDb_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("No data to save.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.IsNewRow) continue;

                    string name = row.Cells["Name"].Value?.ToString();
                    string phone = row.Cells["PhoneNumber"].Value?.ToString();
                    string email = row.Cells["Email"].Value?.ToString();

                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        string query = "INSERT INTO ContactsTable (Name, PhoneNumber, Email) VALUES (@Name, @Phone, @Email)";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@Name", name);
                            cmd.Parameters.AddWithValue("@Phone", phone ?? "");
                            cmd.Parameters.AddWithValue("@Email", email ?? "");
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                MessageBox.Show("Data successfully saved to the database.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // Export DataGridView to CSV
        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("No data to export.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog saveFile = new SaveFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv",
                Title = "Save CSV Report"
            };

            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter sw = new StreamWriter(saveFile.FileName))
                {
                    // Optional: write headers
                    sw.WriteLine("Name,Phone Number,Email");

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (!row.IsNewRow)
                        {
                            string name = row.Cells["Name"].Value?.ToString();
                            string phone = row.Cells["PhoneNumber"].Value?.ToString();
                            string email = row.Cells["Email"].Value?.ToString();

                            sw.WriteLine($"{name},{phone},{email}");
                        }
                    }
                }

                MessageBox.Show("CSV report generated successfully.", "Report", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.IO;

namespace Gas_System
    // 這一面的code跟其他頁面的編排邏輯不太一樣, 為了分頁
{
    public partial class 瓦斯桶登錄 : Form
    {
        //連接資料庫
        private readonly string connectionString = ConfigurationManager.AppSettings["ConnectionString"];
        private readonly string companyId;

        //分頁功能
        private const int pageSize = 3;
        private int currentPage = 0;
        private int totalPageCount = 0;


        public 瓦斯桶登錄(string companyId)
        {
            InitializeComponent();
            this.companyId = companyId;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void 瓦斯桶登錄_Load(object sender, EventArgs e)
        {
            CalculateTotalPages();
            LoadData();
        }
        private void CalculateTotalPages()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                MySqlCommand countCmd = new MySqlCommand($"SELECT COUNT(*) FROM gas WHERE GAS_Company_Id = @CompanyId", conn);
                countCmd.Parameters.AddWithValue("@CompanyId", companyId);
                int totalRecords = Convert.ToInt32(countCmd.ExecuteScalar());
                totalPageCount = (int)Math.Ceiling((double)totalRecords / pageSize);

                conn.Close();
            }
        }
        private void LoadData()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                int offset = currentPage * pageSize;

                MySqlCommand cmd = new MySqlCommand($"SELECT * FROM gas WHERE GAS_Company_Id = @CompanyId LIMIT {offset}, {pageSize}", conn);
                cmd.Parameters.AddWithValue("@CompanyId", companyId);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);
                dataGridView1.DataSource = dt;
                conn.Close();

                currentPageLabel.Text = (currentPage + 1).ToString();
                totalPagesLabel.Text = totalPageCount.ToString();
            }
        }
        private void RefreshDataGridView()
        {
            currentPage = 0;
            LoadData();
        }
        private void ShowAll_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT * FROM gas", conn);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);
                dataGridView1.DataSource = dt;
                conn.Close();
            }
        }

        private void add_Click(object sender, EventArgs e)
        {
            //開啟瓦斯桶資料頁面
            //新增一筆資料
            gas f1;
            f1 = new gas();
            f1.ShowDialog();
        }

        private void edit_Click(object sender, EventArgs e)
        {
            // Get the selected row
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Get the Gas_ID of the selected row
                string gasId = dataGridView1.SelectedRows[0].Cells["Gas_ID"].Value.ToString();

                // Pass the gasId to the edit form
                gas_edit editForm = new gas_edit(gasId);
                editForm.ShowDialog();

                // Refresh the DataGridView after editing
                RefreshDataGridView();
            }
            else
            {
                MessageBox.Show("Please select a row to edit.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void delete_Click(object sender, EventArgs e)
        {
            //選取表單一行資料後按刪除鍵，刪除一筆資料
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DialogResult result = MessageBox.Show("确定删除此行資料吗？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    string id = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                    string query = "DELETE FROM `gas` WHERE `gas_ID` = @ID";
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        using (MySqlCommand command = new MySqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@ID", id);
                            connection.Open();
                            int rowsAffected = command.ExecuteNonQuery();
                            connection.Close();
                            if (rowsAffected > 0)
                            {
                                dataGridView1.Rows.RemoveAt(dataGridView1.SelectedRows[0].Index);
                                MessageBox.Show("删除成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
            }
        }

        private void search_Click(object sender, EventArgs e)
        {
            //搜索關鍵字
            string searchTerm = txt.Text;

            //設定可搜索資料欄位的範圍
            if (!string.IsNullOrEmpty(searchTerm))
            {
                string query = "SELECT * FROM `gas` WHERE Gas_ID LIKE @Gas_ID OR Gas_Company_ID LIKE @Gas_Company_ID OR Gas_Type LIKE @Gas_Type OR Gas_Volume LIKE @Gas_Volume";


                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Gas_ID", "%" + searchTerm + "%");
                        command.Parameters.AddWithValue("@Gas_Company_ID", "%" + searchTerm + "%");
                        command.Parameters.AddWithValue("@Gas_Type", "%" + searchTerm + "%");
                        command.Parameters.AddWithValue("@Gas_Volume", "%" + searchTerm + "%");


                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            DataTable table = new DataTable();
                            adapter.Fill(table);

                            if (table.Rows.Count == 0)
                            {
                                MessageBox.Show("未找到結果。請重試。", "搜尋失敗", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                dataGridView1.DataSource = table;
                            }
                        }
                    }
                }
            }
        }

        //刷新dataGridView的顯示資料
        private void refreshButton_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT * FROM gas", conn);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);
                dataGridView1.DataSource = dt;
                conn.Close();
            }
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            currentPage++;
            LoadData();
        }

        private void previousButton_Click(object sender, EventArgs e)
        {
            if (currentPage > 0)
            {
                currentPage--;
                LoadData();
            }
        }
        private void firstPageButton_Click(object sender, EventArgs e)
        {
            if (currentPage > 0)
            {
                currentPage = 0;
                LoadData();
            }
        }

        private void lastPageButton_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPageCount - 1)
            {
                currentPage = totalPageCount - 1;
                LoadData();
            }
        }

        private void PrintButton_Click(object sender, EventArgs e)
        {
            // Create a SaveFileDialog to select the file path and name
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "CSV file (*.csv)|*.csv";
                saveFileDialog.Title = "Save CSV file";
                saveFileDialog.ShowDialog();

                // If the user clicked the "Save" button
                if (saveFileDialog.FileName != "")
                {
                    try
                    {
                        // Create the CSV file and write the column headers
                        StringBuilder csvContent = new StringBuilder();
                        for (int i = 0; i < dataGridView1.Columns.Count; i++)
                        {
                            csvContent.Append(dataGridView1.Columns[i].HeaderText);
                            if (i < dataGridView1.Columns.Count - 1)
                                csvContent.Append(",");
                        }
                        csvContent.AppendLine();

                        // Write the data rows to the CSV file
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            for (int i = 0; i < dataGridView1.Columns.Count; i++)
                            {
                                if (row.Cells[i].Value != null)
                                    csvContent.Append(row.Cells[i].Value.ToString());
                                if (i < dataGridView1.Columns.Count - 1)
                                    csvContent.Append(",");
                            }
                            csvContent.AppendLine();
                        }

                        // Save the CSV file
                        File.WriteAllText(saveFileDialog.FileName, csvContent.ToString());

                        MessageBox.Show("CSV file saved successfully.", "Save Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error saving CSV file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
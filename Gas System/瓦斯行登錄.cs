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

namespace Gas_System
{
    public partial class 瓦斯行登錄 : Form
    {
        //連接資料庫
        private readonly string connectionString = ConfigurationManager.AppSettings["ConnectionString"];

        public 瓦斯行登錄()
        {
            InitializeComponent();
        }

        private void 瓦斯行登錄_Load(object sender, EventArgs e)
        {
            //設定dataGridView與資料表連接
            RefreshDataGridView();
        }

        private void RefreshDataGridView()
        {
            string query = "SELECT * FROM `company`";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection))
                {
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    dataGridView1.DataSource = table;
                }
                dataGridView1.Columns["COMPANY_Id"].HeaderText = "公司編號";
                dataGridView1.Columns["COMPANY_Name"].HeaderText = "公司名稱";
                dataGridView1.Columns["COMPANY_Phone_No"].HeaderText = "公司電話";
                dataGridView1.Columns["COMPANY_City"].HeaderText = "公司城市";
                dataGridView1.Columns["COMPANY_District"].HeaderText = "公司區域";
                dataGridView1.Columns["COMPANY_Address"].HeaderText = "公司地址";
                dataGridView1.Columns["COMPANY_Notes"].HeaderText = "備註";

                dataGridView1.Columns["COMPANY_Notes"].Visible = false;
                dataGridView1.Columns["COMPANY_Text_Id"].Visible = false;
                dataGridView1.Columns["COMPANY_Registration_Time"].Visible = false;
            }
        }

        private void add_Click(object sender, EventArgs e)
        {
            //開啟瓦斯行資料頁面
            //新增一筆資料
            company f1;
            f1 = new company(null);
            f1.ShowDialog();

            // Refresh the DataGridView after adding a new entry
            RefreshDataGridView();
        }

        private void edit_Click(object sender, EventArgs e)
        {
            // Get the selected row
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Get the Company_ID of the selected row
                string companyId = dataGridView1.SelectedRows[0].Cells["Company_ID"].Value.ToString();

                // Pass the companyId to the edit form
                company f1 = new company(companyId);
                f1.ShowDialog();

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
                DialogResult result = MessageBox.Show("确定删除此行資料？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    string id = dataGridView1.SelectedRows[0].Cells["Company_ID"].Value.ToString();
                    string query = "DELETE FROM `company` WHERE `Company_ID` = @Company_ID";
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        using (MySqlCommand command = new MySqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Company_ID", id);
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
                string query = "SELECT * FROM `company` WHERE Company_ID LIKE @Company_ID OR Company_Name LIKE @Company_Name OR Company_City LIKE @Company_City";
                //string query = "SELECT * FROM `iot` WHERE IOT_ID LIKE @IOT_ID OR Coustomer_ID LIKE @Coustomer_ID OR Coustomer_Name LIKE @Coustomer_Name OR IP LIKE @IP";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Company_ID", "%" + searchTerm + "%");
                        command.Parameters.AddWithValue("@Company_Name", "%" + searchTerm + "%");
                        command.Parameters.AddWithValue("@Company_City", "%" + searchTerm + "%");

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            DataTable table = new DataTable();
                            adapter.Fill(table);

                            if (table.Rows.Count == 0)
                            {
                                MessageBox.Show("未找到結果。請重試。", "搜索失敗", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            RefreshDataGridView();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
                SharedData.SelectedCompanyId = selectedRow.Cells["COMPANY_Id"].Value.ToString();
                // Rest of your code...
            }
        }
    }
}

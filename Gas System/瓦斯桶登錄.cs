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
    public partial class 瓦斯桶登錄 : Form
    {
        //連接資料庫
        private readonly string connectionString = ConfigurationManager.AppSettings["ConnectionString"];
        private readonly string companyId;

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
            //設定dataGridView與資料表連接
            string query = $"SELECT * FROM `gas` WHERE GAS_Company_Id = '{companyId}'";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection))
                {
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    dataGridView1.DataSource = table;

                    // Set the sorting mode for the "GAS_Examine_Day" column to automatic
                    dataGridView1.Columns["GAS_Examine_Day"].SortMode = DataGridViewColumnSortMode.Automatic;

                    // Set the initial sorting order for the "GAS_Examine_Day" column to ascending
                    dataGridView1.Sort(dataGridView1.Columns["GAS_Examine_Day"], ListSortDirection.Ascending);
                    // Columns rename
                    dataGridView1.Columns["GAS_Id"].HeaderText = "瓦斯桶編號";
                    dataGridView1.Columns["GAS_Company_Id"].HeaderText = "所屬公司編號";
                    dataGridView1.Columns["GAS_Weight_Full"].HeaderText = "滿桶重量";
                    dataGridView1.Columns["GAS_Weight_Empty"].HeaderText = "空桶重量";
                    dataGridView1.Columns["GAS_Type"].HeaderText = "瓦斯桶種類";
                    dataGridView1.Columns["GAS_Price"].HeaderText = "瓦斯價格";
                    dataGridView1.Columns["GAS_Volume"].HeaderText = "瓦斯桶容量";
                    dataGridView1.Columns["GAS_Examine_Day"].HeaderText = "檢驗日期";
                    dataGridView1.Columns["GAS_Produce_Day"].HeaderText = "出廠日期";
                    dataGridView1.Columns["GAS_Supplier"].HeaderText = "供應商";
                    dataGridView1.Columns["Gas_Registration_Time"].HeaderText = "瓦斯桶註冊時間";
                    dataGridView1.Columns["last_worker_id"].HeaderText = "最後經手員工";
                    dataGridView1.Columns["GAS_EXAMINE_Day"].HeaderText = "瓦斯桶檢測日期";
                }
            }
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
            //開啟瓦斯桶資料頁面
            //編輯修改某筆資料
            gas f1;
            f1 = new gas();
            f1.ShowDialog();
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
                    string query = "DELETE FROM `gas` WHERE `GAS_Id` = @ID";
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
            瓦斯桶登錄_Load(sender,e);
        }
    }
}

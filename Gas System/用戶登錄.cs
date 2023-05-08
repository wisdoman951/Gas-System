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

namespace Gas_System
{
    public partial class 用戶登錄 : Form
    {
        //連接資料庫
        string connectionString = "server=localhost;user=root;password=89010607;database=new_test;";

        public 用戶登錄()
        {
            InitializeComponent();
        }

        private void 用戶登錄_Load(object sender, EventArgs e)
        {
            //設定dataGridView與資料表連接
            string query = "SELECT * FROM `coustomer`";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection))
                {
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    dataGridView1.DataSource = table;
                }
            }
        }

        private void add_Click(object sender, EventArgs e)
        {
            //開啟基本用戶資料頁面
            //新增一筆資料
            coustomer f1;
            f1 = new coustomer();
            f1.ShowDialog();
        }

        private void edit_Click(object sender, EventArgs e)
        {
            //開啟基本用戶資料頁面
            //編輯修改某筆資料
            coustomer f1;
            f1 = new coustomer();
            f1.ShowDialog();
        }

        private void delete_Click(object sender, EventArgs e)
        {
            //選取表單一行資料後按刪除鍵，刪除一筆資料
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DialogResult result = MessageBox.Show("确定删除此行資料？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    string id = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                    string query = "DELETE FROM `coustomer` WHERE `ID` = @ID";
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
            else
            {
                MessageBox.Show("請選擇要刪除的資料行", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void search_Click(object sender, EventArgs e)
        {
            //搜索關鍵字
            string searchTerm = txt.Text;

            //設定可搜索資料欄位的範圍
            if (!string.IsNullOrEmpty(searchTerm))
            {
                string query = "SELECT * FROM `coustomer` WHERE Coustomer_ID LIKE @Coustomer_ID OR Coustomer_Name LIKE @Coustomer_Name OR Coustomer_Phone LIKE @Coustomer_Phone OR Coustomer_City LIKE @Coustomer_City";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Coustomer_ID", "%" + searchTerm + "%");
                        command.Parameters.AddWithValue("@Coustomer_Name", "%" + searchTerm + "%");
                        command.Parameters.AddWithValue("@Coustomer_Phone", "%" + searchTerm + "%");
                        command.Parameters.AddWithValue("@Coustomer_City", "%" + searchTerm + "%");

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
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT * FROM coustomer", conn);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);
                dataGridView1.DataSource = dt;
                conn.Close();
            }
        }
    }
}

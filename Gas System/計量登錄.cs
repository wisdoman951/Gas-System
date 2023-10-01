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
    public partial class 計量登錄 : Form
    {
        //連接資料庫
        private readonly string connectionString = ConfigurationManager.AppSettings["ConnectionString"];

        public 計量登錄()
        {
            InitializeComponent();
        }
        private void 計量登錄_Load(object sender, EventArgs e)
        {
            //設定dataGridView與資料表連接
            string query = "SELECT * FROM `iot`";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection))
                {
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    dataGridView1.DataSource = table;
                }
                dataGridView1.Columns["SENSOR_Id"].HeaderText = "感測器編號";
                dataGridView1.Columns["CUSTOMER_Id"].HeaderText = "使用者編號";
                dataGridView1.Columns["SENSOR_GPS"].HeaderText = "感測器定位";
                dataGridView1.Columns["Gas_Id"].HeaderText = "瓦斯桶編號";
                dataGridView1.Columns["Gas_Empty_Weight"].HeaderText = "瓦斯空桶重";
                dataGridView1.Columns["Gas_Original_Weight"].HeaderText = "瓦斯滿桶重";
            }
        }

        private void add_Click(object sender, EventArgs e)
        {
            //開啟計量器(iot)資料頁面
            //iot登錄資料的欄位目前也還不完全，要跟德宏確認
            //新增一筆資料
            iot f1;
            f1 = new iot();
            f1.ShowDialog();
        }

        private void edit_Click(object sender, EventArgs e)
        {
            //開啟計量器(iot)資料頁面
            //編輯修改某筆資料
            iot f1;
            f1 = new iot();
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
                    string query = "DELETE FROM `iot` WHERE `ID` = @ID";
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
                string query = "SELECT * FROM `iot` WHERE IOT_ID LIKE @IOT_ID OR Coustomer_ID LIKE @Coustomer_ID OR Coustomer_Name LIKE @Coustomer_Name OR IP LIKE @IP";
                //string query = "SELECT * FROM `iot` WHERE IOT_ID LIKE @IOT_ID OR Coustomer_ID LIKE @Coustomer_ID OR Coustomer_Name LIKE @Coustomer_Name OR IP LIKE @IP";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IOT_ID", "%" + searchTerm + "%");
                        command.Parameters.AddWithValue("@Coustomer_ID", "%" + searchTerm + "%");
                        command.Parameters.AddWithValue("@Coustomer_Name", "%" + searchTerm + "%");
                        command.Parameters.AddWithValue("@IP", "%" + searchTerm + "%");

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

                MySqlCommand cmd = new MySqlCommand("SELECT * FROM iot", conn);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);
                dataGridView1.DataSource = dt;
                conn.Close();
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = @"SELECT *
                                FROM sensor_history
                                WHERE SENSOR_Id = (
                                    SELECT SENSOR_Id
                                    FROM iot
                                    WHERE SENSOR_Id = @SENSOR_Id
                                )
                                ORDER BY SENSOR_Time DESC;
                                ";

                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
                string SENDOR_Id = selectedRow.Cells["SENSOR_Id"].Value.ToString();
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SENSOR_Id", SENDOR_Id);

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    {
                        DataTable historyOrders = new DataTable();
                        adapter.Fill(historyOrders);

                        // Create an instance of the HistoryOrder form
                        HistoryOrder historyOrderForm = new HistoryOrder();

                        // Pass the historyOrders DataTable to the form
                        historyOrderForm.SetData(historyOrders);

                        // Display the form
                        historyOrderForm.ShowDialog();

                    }
                }
            }
        }
    }
}

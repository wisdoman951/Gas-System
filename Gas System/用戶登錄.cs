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
    public partial class 用戶登錄 : Form
    {
        //連接資料庫
        private readonly string connectionString = ConfigurationManager.AppSettings["ConnectionString"];

        public 用戶登錄()
        {
            InitializeComponent();
        }

        private void 用戶登錄_Load(object sender, EventArgs e)
        {
            //設定dataGridView與資料表連接
            string query = @"SELECT c.*, a.Alert_Volume, sh.SENSOR_Weight, i.Sensor_Id
                 FROM customer c
                 LEFT JOIN iot i ON c.CUSTOMER_Id = i.CUSTOMER_Id
                 LEFT JOIN alert a ON i.Sensor_Id = a.Sensor_Id
                 LEFT JOIN (
                     SELECT *,
                            ROW_NUMBER() OVER (PARTITION BY SENSOR_Id ORDER BY SENSOR_Time DESC) AS rn
                     FROM sensor_history
                 ) sh ON i.Sensor_Id = sh.SENSOR_Id AND sh.rn = 1";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection))
                {
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    dataGridView1.DataSource = table;

                    //客戶要求不要顯示出這些欄位
                    table.Columns.Remove("CUSTOMER_Password");
                    table.Columns.Remove("COMPANY_Id");
                    table.Columns.Remove("COMPANY_HistoryID");
                    // Change the original column order
                    string[] columnOrder = {
                                            "CUSTOMER_Id",
                                            "CUSTOMER_Name",
                                            "CUSTOMER_Address",
                                            "Sensor_Id",
                                            "Alert_Volume",
                                            "SENSOR_Weight",
                                            "CUSTOMER_PhoneNo",
                                            "CUSTOMER_Sex",
                                            "CUSTOMER_Postal_Code",
                                            "CUSTOMER_HouseTelpNo",
                                            "CUSTOMER_Email",
                                            "CUSTOMER_FamilyMemberId",
                                            "CUSTOMER_Notes",
                                            "CUSTOMER_Registration_Time"
                                        };

                    // Loop through the columns and set their display order
                    foreach (string columnName in columnOrder)
                    {
                        if (dataGridView1.Columns.Contains(columnName))
                        {
                            dataGridView1.Columns[columnName].DisplayIndex = Array.IndexOf(columnOrder, columnName);
                        }
                    }
                    // Columns rename
                    dataGridView1.Columns["CUSTOMER_Id"].HeaderText = "客戶編號";
                    dataGridView1.Columns["CUSTOMER_Name"].HeaderText = "客戶姓名";
                    dataGridView1.Columns["Sensor_Id"].HeaderText = "感測器編號";
                    dataGridView1.Columns["Alert_Volume"].HeaderText = "通報門檻";
                    dataGridView1.Columns["SENSOR_Weight"].HeaderText = "當前瓦斯量";
                    dataGridView1.Columns["CUSTOMER_Sex"].HeaderText = "客戶性別";
                    dataGridView1.Columns["CUSTOMER_PhoneNo"].HeaderText = "客戶電話";
                    dataGridView1.Columns["CUSTOMER_Postal_Code"].HeaderText = "客戶郵遞區號";
                    dataGridView1.Columns["CUSTOMER_Address"].HeaderText = "客戶地址";
                    dataGridView1.Columns["CUSTOMER_HouseTelpNo"].HeaderText = "客戶家用電話";
                    dataGridView1.Columns["CUSTOMER_Email"].HeaderText = "客戶電子郵件";
                    dataGridView1.Columns["CUSTOMER_FamilyMemberId"].HeaderText = "客戶關係家人";
                    dataGridView1.Columns["CUSTOMER_Notes"].HeaderText = "客戶備註";
                    dataGridView1.Columns["CUSTOMER_Registration_Time"].HeaderText = "客戶註冊時間";
                }
            }
        }

        private void add_Click(object sender, EventArgs e)
        {
            //開啟基本用戶資料頁面
            //新增一筆資料
            customer_form f1;
            f1 = new customer_form();
            f1.ShowDialog();
        }

        private void edit_Click(object sender, EventArgs e)
        {
            //開啟基本用戶資料頁面
            //編輯修改某筆資料
            customer_form f1;
            f1 = new customer_form();
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
                    string query = "DELETE FROM `customer` WHERE `ID` = @ID";
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
                string query = "SELECT * FROM `customer` WHERE Customer_ID LIKE @Customer_ID OR Customer_Name LIKE @Customer_Name OR Customer_PhoneNo LIKE @Customer_Phone OR Customer_City LIKE @Customer_City";
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Customer_ID", "%" + searchTerm + "%");
                        command.Parameters.AddWithValue("@Customer_Name", "%" + searchTerm + "%");
                        command.Parameters.AddWithValue("@Customer_Phone", "%" + searchTerm + "%");
                        command.Parameters.AddWithValue("@Customer_City", "%" + searchTerm + "%");

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

                MySqlCommand cmd = new MySqlCommand("SELECT * FROM customer", conn);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);
                dataGridView1.DataSource = dt;
                conn.Close();
            }
        }
    }
}

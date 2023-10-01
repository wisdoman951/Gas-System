using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace Gas_System
{
    public partial class flow_meter : Form
    {
        //連接資料庫
        string connectionString = ConfigurationManager.AppSettings["ConnectionString"];

        public flow_meter()
        {
            InitializeComponent();
        }

        private void flow_meter_Load(object sender, EventArgs e)
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
            }
            dataGridView1.Columns["SENSOR_Id"].HeaderText = "感測器編號";
            dataGridView1.Columns["CUSTOMER_Id"].HeaderText = "使用者編號";
            dataGridView1.Columns["SENSOR_GPS"].HeaderText = "感測器定位";
            dataGridView1.Columns["Gas_Id"].HeaderText = "瓦斯桶編號";
            dataGridView1.Columns["Gas_Empty_Weight"].HeaderText = "瓦斯空桶重";
            dataGridView1.Columns["Gas_Original_Weight"].HeaderText = "瓦斯滿桶重";
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace Gas_System
{
    public partial class Measurement : Form
    {
        string connectionString = ConfigurationManager.AppSettings["ConnectionString"];

        public Measurement()
        {
            InitializeComponent();
        }

        private void Measurement_Load(object sender, EventArgs e)
        {
            //設定dataGridView與資料表連接
            string query = "SELECT * FROM `sensor_history`";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection))
                {
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    dataGridView1.DataSource = table;
                }
            }
            dataGridView1.Columns["SENSOR_Index"].HeaderText = "歷史編號";
            dataGridView1.Columns["SENSOR_Id"].HeaderText = "感測器編號";
            dataGridView1.Columns["SENSOR_Weight"].HeaderText = "感測器測量值";
            dataGridView1.Columns["SENSOR_Time"].HeaderText = "回傳時間";
            dataGridView1.Columns["SENSOR_GPS"].HeaderText = "感測器定位";
            dataGridView1.Columns["SENSOR_Battery"].HeaderText = "感測器電量";
            dataGridView1.Columns["Gas_Id"].HeaderText = "瓦斯桶編號";
            dataGridView1.Columns["SENSOR_Interval"].HeaderText = "通報間隔";
        }
    }
}

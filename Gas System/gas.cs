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
    public partial class gas : Form
    {
        public gas()
        {
            InitializeComponent();
        }

        private void gas_Load(object sender, EventArgs e)
        {

        }

        private void button15_Click(object sender, EventArgs e)
        {
            //連接資料庫
            string connStr = ConfigurationManager.AppSettings["ConnectionString"];
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                //新增一筆資料
                string insertQuery = "INSERT INTO gas (Gas_ID,Gas_Company_ID,Gas_Weight_Full,Gas_Type,Gas_Volume,Gas_Examine_Day,Gas_Produced_Day,Registered_at) VALUES (@Gas_ID,@Gas_Weight_Full,@Gas_Type,@Gas_Volume,@Gas_Examine_Day,@Gas_Produced_Day,NOW())";

                MySqlCommand cmd = new MySqlCommand(insertQuery, conn);
                cmd.Parameters.AddWithValue("@Gas_ID", id.Text);
                cmd.Parameters.AddWithValue("@Gas_Weight_Full", Weight_Full.Text);
                cmd.Parameters.AddWithValue("@Gas_Type", type.Text);
                cmd.Parameters.AddWithValue("@Gas_Volume", volume.Text);
                cmd.Parameters.AddWithValue("@Gas_Examine_Day", dateTimePicker1.Text);
                cmd.Parameters.AddWithValue("@Gas_Produced_Day", dateTimePicker2.Text);


                if (cmd.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("登錄成功！");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("登錄失敗！");
                }

            }
        }
    }
}

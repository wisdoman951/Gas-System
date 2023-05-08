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
    public partial class iot : Form
    {
        public iot()
        {
            InitializeComponent();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            //連接資料庫
            string connStr = "server=localhost;user=root;password=89010607;database=new_test;";
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                //新增一筆資料
                string insertQuery = "INSERT INTO iot (IOT_ID,Coustomer_ID,Coustomer_Name,IP,Version,Registered_at) VALUES (@IOT_ID,@Coustomer_ID,@Coustomer_Name,@IP,Version,NOW())";
                MySqlCommand cmd = new MySqlCommand(insertQuery, conn);
                cmd.Parameters.AddWithValue("@IOT_ID", ID.Text);
                cmd.Parameters.AddWithValue("@Coustomer_ID", number.Text);
                cmd.Parameters.AddWithValue("@Coustomer_Name", user.Text);
                cmd.Parameters.AddWithValue("@IP", IP.Text);
                cmd.Parameters.AddWithValue("@Version", Version.Text);




                if (cmd.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("登錄成功！");
                    this.Close();

                }
                else
                {
                    MessageBox.Show("登錄失敗！");
                }
                conn.Close();

            }
        }
    }
}

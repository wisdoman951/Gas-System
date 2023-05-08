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
    public partial class company : Form
    {
        public company()
        {
            InitializeComponent();
        }

        private void company_Load(object sender, EventArgs e)
        {

        }

        private void button15_Click(object sender, EventArgs e)
        {
            //連接資料庫
            string connStr = "server=localhost;user=root;password=89010607;database=new_test;";
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                //新增一筆資料
                string insertQuery = "INSERT INTO company (Company_Name,Company_Phone,Company_Phone2,Company_Email,Company_City,Company_District,Company_Address,Company_Contact,Registered_at) VALUES (@Company_Name,@Company_Phone,@Company_Phone2,@Company_Email,@Company_City,@Company_District,@Company_Address,@Company_Contact,NOW())";
                MySqlCommand cmd = new MySqlCommand(insertQuery, conn);
                cmd.Parameters.AddWithValue("@Company_Name", name.Text);
                cmd.Parameters.AddWithValue("@Company_Phone", phone.Text);
                cmd.Parameters.AddWithValue("@Company_Phone2", phone2.Text);
                cmd.Parameters.AddWithValue("@Company_Email", email.Text);
                cmd.Parameters.AddWithValue("@Company_City", city.Text);
                cmd.Parameters.AddWithValue("@Company_District", district.Text);
                cmd.Parameters.AddWithValue("@Company_Address", address.Text);
                cmd.Parameters.AddWithValue("@Company_Contact", contact.Text);



                if (cmd.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("登錄成功！");
                    this.Close();
                    //瓦斯行登錄 form1 = new 瓦斯行登錄();
                    //form1.Show();
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

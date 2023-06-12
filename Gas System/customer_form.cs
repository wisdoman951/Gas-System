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
    public partial class customer_form : Form
    {
        

        public customer_form()
        {
            InitializeComponent();
        }

        private void coustomer_Load(object sender, EventArgs e)
        {
            
        }

        private void ConfirmAddButton_Click(object sender, EventArgs e)
        {
            //連接資料庫
            string connStr = ConfigurationManager.AppSettings["ConnectionString"]; ;
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                //新增一筆資料
                string insertQuery = "INSERT INTO customer (Customer_ID, Customer_Name, Customer_Sex, Customer_Phone, Customer_HouseTel, Customer_Email, Customer_City, Customer_District, Customer_Address, Customer_FamilyMember_ID, Company_ID,Registered_at)" + 
                    "VALUES (@Customer_ID,@Customer_Name,@Customer_Sex,@Customer_Phone,@Customer_HouseTel,@Customer_Email,@Customer_City,@Customer_District,@Customer_Address,@Customer_FamilyMember_ID,Company_ID,NOW())";

                MySqlCommand cmd = new MySqlCommand(insertQuery, conn);
                cmd.Parameters.AddWithValue("@Customer_ID", number.Text);
                cmd.Parameters.AddWithValue("@Customer_Name", Uname.Text);
                cmd.Parameters.AddWithValue("@Customer_Sex", sex.Text);
                cmd.Parameters.AddWithValue("@Customer_Phone", phone.Text);
                cmd.Parameters.AddWithValue("@Customer_HouseTel", tel.Text);
                cmd.Parameters.AddWithValue("@Customer_Email", email.Text);
                cmd.Parameters.AddWithValue("@Customer_City", city.Text);
                cmd.Parameters.AddWithValue("@Customer_District", district.Text);
                cmd.Parameters.AddWithValue("@Customer_Address", address.Text);
                cmd.Parameters.AddWithValue("@Customer_FamilyMember_ID", family.Text);
                cmd.Parameters.AddWithValue("@Company_ID", company.Text);

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

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
    public partial class Form3 : Form
    {
        //連接資料庫
        private readonly string connectionString = "Server=localhost;Database=new_test;Uid=root;Pwd=89010607";

        public Form3()
        {
            InitializeComponent();
        }

        private void exit_Click(object sender, EventArgs e)
        {
            //按X退出系統
            Application.Exit();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            //設定每個欄位為必填
            string Manager_Name = txtName.Text;
            string Phone = txtPhone.Text;
            string City = txtCity.Text;
            string District = txtDistrict.Text;
            string Address = txtAddress.Text;
            string email = txtEmail.Text;
            string password = txtPassword.Text;
            string confirmPassword = txtPassword2.Text;

            if (string.IsNullOrEmpty(Manager_Name) || string.IsNullOrEmpty(Phone) ||
                string.IsNullOrEmpty(City) || string.IsNullOrEmpty(District) || string.IsNullOrEmpty(Address) ||
                string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("所有欄位皆為必填。");
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("密碼和確認密碼不相符。");
                return;
            }
            // 儲存新增 manager 資料後，資料庫自動產生Manager_ID
            int Manager_ID;

            // 建立資料庫連線(基本資料)
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = @"INSERT INTO manager (Manager_Name, Phone,City, District, Address, Registered_at)
                            VALUES (@Manager_Name, @Phone, @City, @District, @Address, @Registered_at);
                            SELECT LAST_INSERT_ID();";
                cmd.Parameters.AddWithValue("@Manager_Name",Manager_Name);
                cmd.Parameters.AddWithValue("@Phone",Phone);
                cmd.Parameters.AddWithValue("@City",City);
                cmd.Parameters.AddWithValue("@District",District);
                cmd.Parameters.AddWithValue("@Address",Address);
                cmd.Parameters.AddWithValue("@Registered_at",DateTime.Now);
                Manager_ID = Convert.ToInt32(cmd.ExecuteScalar());
            }

            // 建立另一個資料庫連線(帳號密碼)
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = @"INSERT INTO manager_account (Manager_ID, Email, Password)
                            VALUES (@Manager_ID, @Email, @Password);";
                cmd.Parameters.AddWithValue("@Manager_ID", Manager_ID);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.ExecuteNonQuery();
            }

            // 顯示註冊成功的訊息，並返回登入介面
            MessageBox.Show("註冊成功！");
            this.Close();
            Form1 f1 = new Form1();
            f1.Show();
        }

        private void label8_Click(object sender, EventArgs e)
        {
            //左上角返回登入介面
            Form1 f1 = new Form1();
            f1.Show();
            this.Hide();
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }
    }
}

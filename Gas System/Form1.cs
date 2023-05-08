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
    public partial class Form1 : Form
    {
        //連接資料庫
        private readonly string connectionString = ConfigurationManager.AppSettings["ConnectionString"];

        public Form1()
        {
            InitializeComponent();
        }

        private void exit_Click(object sender, EventArgs e)
        {
            //按X退出系統
            Application.Exit();
        }

        private void btlogin_Click(object sender, EventArgs e)
        {
            //輸入格若為空值，顯示提示訊息
            string email = textBox1.Text;
            string password = textBox2.Text;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("請輸入帳號密碼");
                return;
            }

            // 連接資料庫進行帳號密碼驗證
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = @"SELECT Manager_ID FROM manager_account WHERE Email = @Email AND Password = @Password;";
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);
                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    // 驗證成功顯示登入成功訊息，並顯示主畫面
                    int Manager_ID = Convert.ToInt32(result);
                    MessageBox.Show($"{Manager_ID} 登入成功！");
                    Form2 mainForm = new Form2();
                    mainForm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("帳號或密碼錯誤。");
                }
            }

        }

        
        private void button2_Click(object sender, EventArgs e)
        {
            //至註冊頁面
            Form3 mainForm = new Form3();
            mainForm.Show();
            this.Hide();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}

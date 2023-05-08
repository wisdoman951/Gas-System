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
    public partial class Form2 : Form
    {
        //連接資料庫
        private readonly string connectionString = ConfigurationManager.AppSettings["ConnectionString"];

        public Form2()
        {
            InitializeComponent();
            customizDesing();
        }

        // 設置菜單列的次選單
        private void customizDesing()
        {
            panelG1.Visible = false;
            panelG2.Visible = false;
            panelG3.Visible = false;
        }

        // 隱藏次選單
        private void hideSubMenu()
        {
            if (panelG1.Visible == true)
                panelG1.Visible = false;
            if (panelG2.Visible == true)
                panelG2.Visible = false;
            if (panelG3.Visible == true)
                panelG3.Visible = false;
        }

        // 顯示指定的次選單
        private void showSubMenu(Panel subMenu)
        {
            if (subMenu.Visible == false)
            {
                hideSubMenu();
                subMenu.Visible = true;
            }
            else
                subMenu.Visible = false;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            //設定日期、時間、星期
            timer1.Start();
            date.Text = DateTime.Now.ToLongDateString();
            time.Text = DateTime.Now.ToLongTimeString();
            week.Text = DateTime.Now.ToString("dddd");
        }

        
        private void timer1_Tick(object sender, EventArgs e)
        {
            //Timer 控制時間的事件處理方法
            time.Text = DateTime.Now.ToLongTimeString();
            timer1.Start();
        }

        private Form activeForm = null;

        //開啟子視窗方法
        private void openChildForm(Form childForm)
        {
            if (activeForm != null)
                activeForm.Close();
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            form_pl.Controls.Add(childForm);
            form_pl.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        private void OrderManagementPage_Click(object sender, EventArgs e)
        {
            //關閉分頁
            activeForm.Close();
            hideSubMenu();
        }

        private void DataInsertPage_Click(object sender, EventArgs e)
        {
            //點選菜單列，開啟子選單
            showSubMenu(panelG1);
        }

        private void ReportFunctionPage_Click(object sender, EventArgs e)
        {
            //點選菜單列，開啟子選單
            showSubMenu(panelG2);
        }

        private void DaraSearchPage_Click(object sender, EventArgs e)
        {
            //點選菜單列，開啟子選單
            showSubMenu(panelG3);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //開啟對應分頁
            openChildForm(new 用戶登錄());
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //開啟對應分頁
            openChildForm(new 瓦斯行登錄());
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //開啟對應分頁
            openChildForm(new 瓦斯桶登錄());
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //開啟對應分頁
            openChildForm(new 計量登錄());
        }
        private void button9_Click(object sender, EventArgs e)
        {
            //開啟安全存量通知的頁面
            //主要功能是 顯示低於預設瓦斯用量用戶，然後每日提醒用戶、瓦斯行
            //通知頻率/時間：用戶在App可設定選擇低於安全存量3.5、2.5kg下拉式選項之通知天數頻率，後台依照用戶設定做通知，低於安全存量 2kg 時，系統將會每日通知兩次
            openChildForm(new 瓦斯通報());

        }
        private void form_pl_Paint(object sender, PaintEventArgs e)
        {
            //連接資料表
            string query = "SELECT * FROM `new_order`";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection))
                {
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    dataGridView1.DataSource = table;
                }
            }
        }


        private void button15_Click(object sender, EventArgs e)
        {
            //搜索關鍵字
            string searchTerm = txt.Text;

            //設定可搜索資料欄位的範圍
            if (!string.IsNullOrEmpty(searchTerm))
            {
                string query = "SELECT * FROM `new_order` WHERE Order_ID LIKE @Order_ID OR Customer_Name LIKE @Customer_Name";
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Order_ID", "%" + searchTerm + "%");
                        command.Parameters.AddWithValue("@Customer_Name", "%" + searchTerm + "%");

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            DataTable table = new DataTable();
                            adapter.Fill(table);

                            //查詢結果
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

        private void button16_Click(object sender, EventArgs e)
        {
            //確認收取新訂單的按鈕
            //確認後，訂單將傳送至指定瓦斯行，後續由瓦斯行完成訂單的派送
        }

        
        private void button14_Click(object sender, EventArgs e)
        {
            //刷新dataGridView顯示的資料表
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT * FROM new_order", conn);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);
                dataGridView1.DataSource = dt;
                conn.Close();
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            //開啟客戶資料的視窗
            coustomer f1;
            f1 = new coustomer();
            f1.ShowDialog();
        }

        //以下頁面"未完"
        private void button12_Click(object sender, EventArgs e)
        {
            //紅利點數(具體紅利方案沒有明確...之後要跟德宏確認)
            openChildForm(new bonus_points());
        }

        private void button10_Click(object sender, EventArgs e)
        {
            //查詢流量
            //實時監控瓦斯量
            openChildForm(new flow_meter());
        }

        private void button11_Click(object sender, EventArgs e)
        {
            //查詢重量
            //實時監控瓦斯用量
            openChildForm(new Measurement());
        }

        private void button13_Click(object sender, EventArgs e)
        {
            //瓦斯行明細查詢(具體要查什麼沒有明確...之後要跟德宏確認)
        }

        
    }
}

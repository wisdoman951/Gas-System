﻿using System;
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
            if (activeForm != null)
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
            if (SharedData.SelectedCompanyId != "")
            {
                openChildForm(new 瓦斯桶登錄(SharedData.SelectedCompanyId));
            }
            else
            {
                openChildForm(new 瓦斯桶登錄(""));

            }
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
            //string query = "SELECT * FROM `gas_order`";
            string query = "SELECT o.ORDER_Id, c.CUSTOMER_PhoneNo, o.DELIVERY_Address, o.DELIVERY_Time, c.CUSTOMER_Name, od.Order_type, od.Order_weight,o.Gas_Quantity, o.COMPANY_Id FROM `gas_order` o JOIN`customer` c ON o.CUSTOMER_Id = c.CUSTOMER_Id JOIN `gas_order_detail` od ON o.ORDER_Id = od.Order_ID;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection))
                {
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    dataGridView1.DataSource = table;
                    dataGridView1.Columns["ORDER_Id"].HeaderText = "訂單編號";
                    dataGridView1.Columns["CUSTOMER_PhoneNo"].HeaderText = "顧客電話";
                    dataGridView1.Columns["DELIVERY_Address"].HeaderText = "送貨地址";
                    dataGridView1.Columns["DELIVERY_Time"].HeaderText = "送貨時間";
                    dataGridView1.Columns["CUSTOMER_Name"].HeaderText = "訂購人";
                    dataGridView1.Columns["Order_type"].HeaderText = "瓦斯桶種類";
                    dataGridView1.Columns["Order_weight"].HeaderText = "瓦斯規格";
                    dataGridView1.Columns["Gas_Quantity"].HeaderText = "數量";
                    dataGridView1.Columns["COMPANY_Id"].HeaderText = "選擇瓦斯行";
                }
            }
        }


        private void SearchButton_Click(object sender, EventArgs e)
        {
            string searchTerm = txt.Text.Trim();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                DataTable dataTable = (DataTable)dataGridView1.DataSource;
                if (dataTable != null)
                {
                    string filterExpression = $"CUSTOMER_Name LIKE '%{searchTerm}%' OR CONVERT(ORDER_Id, 'System.String') LIKE '%{searchTerm}%' OR Customer_PhoneNo LIKE '%{searchTerm}%'";
                    dataTable.DefaultView.RowFilter = filterExpression;
                }
            }
            else
            {
                // Clear the filter if the search term is empty
                DataTable dataTable = (DataTable)dataGridView1.DataSource;
                dataTable.DefaultView.RowFilter = string.Empty;
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            //確認收取新訂單的按鈕
            //確認後，訂單將傳送至指定瓦斯行，後續由瓦斯行完成訂單的派送
        }

        
        private void RefreshButton_Click(object sender, EventArgs e)
        {
            //刷新dataGridView顯示的資料表
            string query = "SELECT o.ORDER_Id, c.CUSTOMER_PhoneNo, o.DELIVERY_Address, o.DELIVERY_Time, c.CUSTOMER_Name, od.Order_type, od.Order_weight,o.Gas_Quantity, o.COMPANY_Id FROM `gas_order` o JOIN`customer` c ON o.CUSTOMER_Id = c.CUSTOMER_Id JOIN `gas_order_detail` od ON o.ORDER_Id = od.Order_ID;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection))
                {
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    txt.Text = "";
                    dataGridView1.DataSource = table;
                    dataGridView1.Columns["ORDER_Id"].HeaderText = "訂單編號";
                    dataGridView1.Columns["CUSTOMER_PhoneNo"].HeaderText = "顧客電話";
                    dataGridView1.Columns["DELIVERY_Address"].HeaderText = "送貨地址";
                    dataGridView1.Columns["DELIVERY_Time"].HeaderText = "送貨時間";
                    dataGridView1.Columns["CUSTOMER_Name"].HeaderText = "訂購人";
                    dataGridView1.Columns["Order_type"].HeaderText = "瓦斯桶種類";
                    dataGridView1.Columns["Order_weight"].HeaderText = "瓦斯規格";
                    dataGridView1.Columns["Gas_Quantity"].HeaderText = "數量";
                    dataGridView1.Columns["COMPANY_Id"].HeaderText = "選擇瓦斯行";
                }
            }
        }

        /*private void button17_Click(object sender, EventArgs e)
        {
            //開啟客戶資料的視窗
            customer_form f1;
            f1 = new customer_form();
            f1.ShowDialog();
        }*/

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
            openChildForm(new 瓦斯行明細查詢());
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // string query = "SELECT * FROM `gas_order` WHERE Order_ID LIKE @Order_ID OR Customer_Name LIKE @Customer_Name";
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
                //string query = "select * from `gas_order` a, `gas_order_detail` b, `customer` c where @a.order_id = @b.order_id and @c.customer_id = @a.customer_id and @a.order_id = @order_Id;";
                // Access the data in the selected row and autofill other fields in the form
                string orderId = selectedRow.Cells["Order_ID"].Value.ToString();
                string customerName = selectedRow.Cells["Customer_Name"].Value.ToString();
                string customerPhone = selectedRow.Cells["Customer_PhoneNo"].Value.ToString();
                string deliveryTime = selectedRow.Cells["Delivery_Time"].Value.ToString();
                string deliveryAddress = selectedRow.Cells["Delivery_Address"].Value.ToString();
                string orderWeight = selectedRow.Cells["Order_weight"].Value.ToString();
                string orderType = selectedRow.Cells["Order_type"].Value.ToString();
                string companyId = selectedRow.Cells["Company_Id"].Value.ToString();

                // Autofill the other fields(Textbox) in the form
                OrderID.Text = orderId;
                CustomerName.Text = customerName;
                CustomerPhone.Text = customerPhone;
                DeliveryTime.Text = deliveryTime;
                DeliveryAddress.Text = deliveryAddress;
                GasType.Text = orderType;
                GasWeight.Text = orderWeight;
                GasStationSelection.Text = companyId;

            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = @"SELECT *
                                FROM gas_order_history
                                WHERE CUSTOMER_Id = (
                                    SELECT CUSTOMER_Id
                                    FROM gas_order_history
                                    WHERE ORDER_Id = @order_id
                                )
                                ORDER BY DELIVERY_Time DESC;
                                ";

                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
                string order_id = selectedRow.Cells["Order_Id"].Value.ToString();
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@order_id", order_id);

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    {
                        DataTable historyOrders = new DataTable();
                        adapter.Fill(historyOrders);

                        // Create an instance of the HistoryOrder form
                        HistoryOrder historyOrderForm = new HistoryOrder();

                        // Pass the historyOrders DataTable to the form
                        historyOrderForm.SetData(historyOrders);

                        // Display the form
                        historyOrderForm.ShowDialog();

                    }
                }
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

    }
}

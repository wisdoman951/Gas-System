﻿using System.Data;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;
using System.Configuration;
using System;

namespace Gas_System
{
    public partial class 瓦斯行明細查詢 : Form
    {
        private readonly string connectionString = ConfigurationManager.AppSettings["ConnectionString"];

        //用來判斷是否要加入第二次的濾網(FromFilter)
        private bool isQueryMode;
        public 瓦斯行明細查詢()
        {
            InitializeComponent();
        }
        private void 瓦斯行明細查詢_Load(object sender, EventArgs e)
        {
            // Populate FromSource combobox
            FromSource.Items.AddRange(new string[] { "未完成訂單", "已完成訂單", "新增顧客", "新增瓦斯桶", "工人" });
        }

        private void FromSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedFromSource = FromSource.SelectedItem.ToString();
            FromFilter.Items.Clear();

            if (selectedFromSource == "未完成訂單")
            {
                FromFilter.Items.AddRange(new string[] { "ORDER_Id", "CUSTOMER_Id", "COMPANY_Id", "DELIVERY_Condition", "Exchange", "DELIVERY_Phone", "Gas_Quantity", "Order_Time", "Delivery_Method" });
            }
            else if (selectedFromSource == "已完成訂單")
            {
                FromFilter.Items.AddRange(new string[] { "ORDER_Id", "CUSTOMER_Id", "COMPANY_Id", "DELIVERY_Time", "DELIVERY_Condition", "DELIVERY_Address", "DELIVERY_PHONE", "Gas_Quantity", "Order_Time", "Expect_Time", "Delivery_Method", "Gas_Detail_Id", "Order_Quantity", "Order_type", "Order_weight", "Exchange", "Completion_Date" });
            }
            else if (selectedFromSource == "新增顧客")
            {
                FromFilter.Items.AddRange(new string[] { "CUSTOMER_Id", "CUSTOMER_Name", "CUSTOMER_Sex", "CUSTOMER_PhoneNo", "CUSTOMER_Postal_Code", "CUSTOMER_Address", "CUSTOMER_HouseTelpNo", "CUSTOMER_Password", "CUSTOMER_Email", "CUSTOMER_FamilyMemberId", "COMPANY_Id", "COMPANY_HistoryID", "CUSTOMER_Notes", "CUSTOMER_Registration_Time" });
            }
            else if (selectedFromSource == "新增瓦斯桶")
            {
                FromFilter.Items.AddRange(new string[] { "GAS_Id", "GAS_Company_Id", "GAS_Weight_Full", "GAS_Weight_Empty", "GAS_Type", "GAS_Price", "GAS_Volume", "GAS_Examine_Day", "GAS_Produce_Day", "GAS_Supplier", "Gas_Registration_Time", "last_worker_id" });
            }
            else if (selectedFromSource == "工人")
            {
                FromFilter.Items.AddRange(new string[] { "WORKER_Id", "WORKER_Name", "WORKER_Sex", "WORKER_PhoneNum", "WORKER_HouseTelpNo", "WORKER_Password", "WORKER_Email", "WORKER_Address", "Permission", "WORKER_Company_Id", "Worker_Notes" });
            }
        }



        private void SearchButton_Click(object sender, EventArgs e)
        {
            string selectedFromSource = FromSource.SelectedItem?.ToString();
            string selectedFromFilter = FromFilter.SelectedItem?.ToString();
            string filterValue = Filter.Text;
            string n = FromDay.Text;

            // 處理近幾日的資料日期格式
            DateTime fromDate = DateTime.Now.Date;
            DateTime toDate = fromDate.AddDays(-int.Parse(n));
            string fromDateStr = fromDate.ToString("yyyy-MM-dd HH:mm:ss");
            string toDateStr = toDate.ToString("yyyy-MM-dd HH:mm:ss");

            Console.WriteLine(fromDateStr);
            Console.WriteLine(toDateStr);



            // Check if day and FromSource are selected
            if (string.IsNullOrEmpty(selectedFromSource))
            {
                MessageBox.Show("Please select FromSource.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            string query = "";

            if (selectedFromSource == "未完成訂單")
            {
                query = "SELECT * FROM gas_order WHERE Order_Time <= @FromDate AND Order_Time >= @ToDate";
                isQueryMode = true;
            }
            /*else if (selectedFromSource == "已完成訂單")
            {
                query = "SELECT * FROM gas_order_history WHERE Registration_Time <= @FromDate AND Registration_Time >= @ToDate";
                isQueryMode = true;
            }*/
            else if (selectedFromSource == "新增顧客")
            {
                query = "SELECT * FROM customer WHERE CUSTOMER_Registration_Time <= @FromDate AND CUSTOMER_Registration_Time >= @ToDate";
                isQueryMode = true;
            }
            else if (selectedFromSource == "新增瓦斯桶")
            {
                query = "SELECT * FROM gas WHERE Gas_Registration_Time <= @FromDate AND Gas_Registration_Time >= @ToDate";
                isQueryMode = true;
            }
            else if (selectedFromSource == "工人")
            {
                query = "SELECT * FROM worker WHERE WORKER_Registration_Time <= @FromDate AND WORKER_Registration_Time >= @ToDate";
                isQueryMode = true;
            }

            if (isQueryMode = true && selectedFromFilter != null && filterValue != null)
            {
                query = query + $" AND {selectedFromFilter} = '{filterValue}'";
            }
            // 如果選的是時間格式的資料，自動切換成dateTimePicker
            if (selectedFromFilter == "DELIVERY_Time" || selectedFromFilter == "Expect_time" || selectedFromFilter == "Delivery_Method" || selectedFromFilter == "Completion_Date" || selectedFromFilter == "CUSTOMER_Registration_Time" || selectedFromFilter == "GAS_Examine_Day" || selectedFromFilter == "GAS_Produce_Day" || selectedFromFilter == "Gas_Registration_Time")
            {
                DateTime selectedTime;
                if (DateTime.TryParse(filterValue, out selectedTime))
                {
                    filterValue = selectedTime.ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    MessageBox.Show("Invalid DateTime format.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    {
                        command.Parameters.AddWithValue("@FromDate", fromDateStr);
                        command.Parameters.AddWithValue("@ToDate", toDateStr);

                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        dataGridView1.DataSource = dataTable;
                    }
                }
            }
        }

        private void FromFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedFromFilter = FromFilter.SelectedItem.ToString();

            if (selectedFromFilter == "DELIVERY_Time" || selectedFromFilter == "DELIVERY_Time" || selectedFromFilter == "Expect_time"  || selectedFromFilter == "Completion_Date" || selectedFromFilter == "CUSTOMER_Registration_Time" || selectedFromFilter == "GAS_Examine_Day" || selectedFromFilter == "GAS_Produce_Day" || selectedFromFilter == "Gas_Registration_Time")
            {
                // Selected item is a time-related column
                Filter.Visible = false;
                TimePicker.Visible = true;
            }
            else
            {
                // Selected item is not a time-related column
                Filter.Visible = true;
                TimePicker.Visible = false;
            }
        }

        private void PrintButton_Click(object sender, EventArgs e)
        {
            // Create a SaveFileDialog to select the file path and name
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "CSV file (*.csv)|*.csv";
                saveFileDialog.Title = "Save CSV file";
                saveFileDialog.ShowDialog();

                // If the user clicked the "Save" button
                if (saveFileDialog.FileName != "")
                {
                    try
                    {
                        // Create the CSV file and write the column headers
                        StringBuilder csvContent = new StringBuilder();
                        for (int i = 0; i < dataGridView1.Columns.Count; i++)
                        {
                            csvContent.Append(dataGridView1.Columns[i].HeaderText);
                            if (i < dataGridView1.Columns.Count - 1)
                                csvContent.Append(",");
                        }
                        csvContent.AppendLine();

                        // Write the data rows to the CSV file
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            for (int i = 0; i < dataGridView1.Columns.Count; i++)
                            {
                                if (row.Cells[i].Value != null)
                                    csvContent.Append(row.Cells[i].Value.ToString());
                                if (i < dataGridView1.Columns.Count - 1)
                                    csvContent.Append(",");
                            }
                            csvContent.AppendLine();
                        }

                        // Save the CSV file
                        File.WriteAllText(saveFileDialog.FileName, csvContent.ToString());

                        MessageBox.Show("CSV file saved successfully.", "Save Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error saving CSV file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

    }
}

using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Configuration;
using System;

namespace Gas_System
{
    public partial class 瓦斯行明細查詢 : Form
    {
        private readonly string connectionString = ConfigurationManager.AppSettings["ConnectionString"];
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

            // Check if day and FromSource are selected
            if (string.IsNullOrEmpty(selectedFromSource))
            {
                MessageBox.Show("Please select FromSource.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check if FromFilter is selected when required
            if ((selectedFromSource == "未完成訂單" || selectedFromSource == "已完成訂單" || selectedFromSource == "新增顧客" || selectedFromSource == "新增瓦斯桶" || selectedFromSource == "工人") && string.IsNullOrEmpty(selectedFromFilter))
            {
                MessageBox.Show("Please select FromFilter.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "";

            if (selectedFromSource == "未完成訂單")
            {
                query = $"SELECT * FROM gas_order WHERE {selectedFromFilter} = '{filterValue}'";
            }
            else if (selectedFromSource == "已完成訂單")
            {
                query = $"SELECT * FROM gas_order_history WHERE {selectedFromFilter} = '{filterValue}'";
            }
            else if (selectedFromSource == "新增顧客")
            {
                query = $"SELECT * FROM customer WHERE {selectedFromFilter} = '{filterValue}'";
            }
            else if (selectedFromSource == "新增瓦斯桶")
            {
                query = $"SELECT * FROM gas WHERE {selectedFromFilter} = '{filterValue}'";
            }
            else if (selectedFromSource == "工人")
            {
                query = $"SELECT * FROM worker WHERE {selectedFromFilter} = '{filterValue}'";
            }

            // Check if the selected FromFilter is a time-related column
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
    }
}

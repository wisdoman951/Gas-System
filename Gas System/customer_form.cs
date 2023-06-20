﻿using System;
using System.Configuration;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;

namespace Gas_System
{
    public partial class customer_form : Form
    {
        private readonly string connectionString = ConfigurationManager.AppSettings["ConnectionString"];
        private string orderId;
        private string customerId;
        private bool isEditMode;


        public customer_form(string id, string customerId)
        {
            InitializeComponent();
            orderId = id;
            this.customerId = customerId;
            isEditMode = !string.IsNullOrEmpty(id);
        }
        private void customer_form_Load(object sender, EventArgs e)
        {
            if (isEditMode)
            {
                // Load customer data for editing
                LoadCustomerData();
            }
            else
            {
                // Clear the form for adding a new customer
                ClearForm();
            }
        }

        private void ClearForm()
        {
            CustomerName.Text = "";
            CustomerSex.Text = "";
            CustomerPhoneNo.Text = "";
            CustomerPostalCode.Text = "";
            CustomerAddress.Text = "";
            CustomerHouseTelNo.Text = "";
            CustomerPassword.Text = "";
            CustomerEmail.Text = "";
            CustomerFamilyMember.Text = "";
            CustomerNotes.Text = "";
            CustomerCompanyID.Text = "";
        }

        private void LoadCustomerData()
        {
            string query = "SELECT * FROM customer WHERE Customer_ID = @Customer_ID";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Customer_ID", customerId);

                    connection.Open();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            CustomerName.Text = reader["Customer_Name"].ToString();
                            CustomerSex.Text = reader["Customer_Sex"].ToString();
                            CustomerPhoneNo.Text = reader["Customer_PhoneNo"].ToString();
                            CustomerPostalCode.Text = reader["Customer_Postal_Code"].ToString();
                            CustomerAddress.Text = reader["Customer_Address"].ToString();
                            CustomerHouseTelNo.Text = reader["Customer_HouseTelpNo"].ToString();
                            CustomerPassword.Text = reader["Customer_Password"].ToString();
                            CustomerEmail.Text = reader["Customer_Email"].ToString();
                            CustomerFamilyMember.Text = reader["Customer_FamilyMemberId"].ToString();
                            CustomerNotes.Text = reader["Customer_Notes"].ToString();
                            CustomerCompanyID.Text = reader["Company_Id"].ToString();
                        }
                    }

                    connection.Close();
                }
            }
        }

        private void ComfirmButton_Click(object sender, EventArgs e)
        {
            // Perform validation
            if (string.IsNullOrEmpty(CustomerName.Text) || string.IsNullOrEmpty(CustomerSex.Text) ||
                string.IsNullOrEmpty(CustomerPhoneNo.Text) || string.IsNullOrEmpty(CustomerPostalCode.Text) ||
                string.IsNullOrEmpty(CustomerAddress.Text) || string.IsNullOrEmpty(CustomerHouseTelNo.Text) ||
                string.IsNullOrEmpty(CustomerPassword.Text) || string.IsNullOrEmpty(CustomerEmail.Text) ||
                string.IsNullOrEmpty(CustomerFamilyMember.Text) || string.IsNullOrEmpty(CustomerCompanyID.Text))
            {
                MessageBox.Show("All fields are required. Please fill in all the fields.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Stop further processing
            }

            if (isEditMode)
            {
                // Update existing customer data
                UpdateCustomerData();
            }
            else
            {
                // Insert new customer data
                InsertCustomerData();
            }
        }

        private void InsertCustomerData()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string insertQuery = "INSERT INTO customer (Customer_Name, Customer_Sex, Customer_PhoneNo, Customer_Postal_Code, Customer_Address, Customer_HouseTelpNo, Customer_Password, Customer_Email, Customer_FamilyMemberId, Company_Id, Customer_Notes, Customer_Registration_Time) " +
                    "VALUES (@Customer_Name, @Customer_Sex, @Customer_PhoneNo, @Customer_Postal_Code, @Customer_Address, @Customer_HouseTelpNo, @Customer_Password, @Customer_Email, @Customer_FamilyMemberId, @Company_Id, @Customer_Notes, NOW())";

                MySqlCommand cmd = new MySqlCommand(insertQuery, conn);
                cmd.Parameters.AddWithValue("@Customer_Name", CustomerName.Text);
                cmd.Parameters.AddWithValue("@Customer_Sex", CustomerSex.Text);
                cmd.Parameters.AddWithValue("@Customer_PhoneNo", CustomerPhoneNo.Text);
                cmd.Parameters.AddWithValue("@Customer_Postal_Code", CustomerPostalCode.Text);
                cmd.Parameters.AddWithValue("@Customer_Address", CustomerAddress.Text);
                cmd.Parameters.AddWithValue("@Customer_HouseTelpNo", CustomerHouseTelNo.Text);
                cmd.Parameters.AddWithValue("@Customer_Password", CustomerPassword.Text);
                cmd.Parameters.AddWithValue("@Customer_Email", CustomerEmail.Text);
                cmd.Parameters.AddWithValue("@Customer_FamilyMemberId", CustomerFamilyMember.Text);
                cmd.Parameters.AddWithValue("@Customer_Notes", CustomerNotes.Text);
                cmd.Parameters.AddWithValue("@Company_Id", CustomerCompanyID.Text);

                if (cmd.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("登錄成功！");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("登錄失敗！");
                }

                conn.Close();
            }
        }

        private void UpdateCustomerData()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string updateQuery = "UPDATE customer SET Customer_Name = @Customer_Name, Customer_Sex = @Customer_Sex, " +
                    "Customer_PhoneNo = @Customer_PhoneNo, Customer_Postal_Code = @Customer_Postal_Code, " +
                    "Customer_Address = @Customer_Address, Customer_HouseTelpNo = @Customer_HouseTelpNo, " +
                    "Customer_Password = @Customer_Password, Customer_Email = @Customer_Email, " +
                    "Customer_FamilyMemberId = @Customer_FamilyMemberId, Company_Id = @Company_Id, " +
                    "Customer_Notes = @Customer_Notes " +
                    "WHERE Customer_ID = @Customer_ID";

                MySqlCommand cmd = new MySqlCommand(updateQuery, conn);
                cmd.Parameters.AddWithValue("@Customer_Name", CustomerName.Text);
                cmd.Parameters.AddWithValue("@Customer_Sex", CustomerSex.Text);
                cmd.Parameters.AddWithValue("@Customer_PhoneNo", CustomerPhoneNo.Text);
                cmd.Parameters.AddWithValue("@Customer_Postal_Code", CustomerPostalCode.Text);
                cmd.Parameters.AddWithValue("@Customer_Address", CustomerAddress.Text);
                cmd.Parameters.AddWithValue("@Customer_HouseTelpNo", CustomerHouseTelNo.Text);
                cmd.Parameters.AddWithValue("@Customer_Password", CustomerPassword.Text);
                cmd.Parameters.AddWithValue("@Customer_Email", CustomerEmail.Text);
                cmd.Parameters.AddWithValue("@Customer_FamilyMemberId", CustomerFamilyMember.Text);
                cmd.Parameters.AddWithValue("@Customer_Notes", CustomerNotes.Text);
                cmd.Parameters.AddWithValue("@Company_Id", CustomerCompanyID.Text);
                cmd.Parameters.AddWithValue("@Customer_ID", customerId);

                if (cmd.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("更新成功！");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("更新失敗！");
                }

                conn.Close();
            }
        }


    }
}

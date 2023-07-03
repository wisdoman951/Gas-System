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
    public partial class company : Form
    {
        private string companyId;
        private bool isEditMode;

        //連接資料庫
        private readonly string connectionString = ConfigurationManager.AppSettings["ConnectionString"];

        public company(string companyId)
        {
            InitializeComponent();
            this.companyId = companyId;
            this.isEditMode = !string.IsNullOrEmpty(companyId);
        }

        private void company_Load(object sender, EventArgs e)
        {
            if (isEditMode)
            {
                // Load company data for editing
                LoadCompanyData();
            }
            else
            {
                // Clear the form for adding a new company
                ClearForm();
            }
        }
        // 如果是以 add_form 的形式打開，需要先清空。
        private void ClearForm()
        {
            // Clear the textboxes or set them to default values
            CompanyName.Text = "";
            CompanyPhoneNo.Text = "";
            CompanyCity.Text = "";
            CompanyDistrict.Text = "";
            CompanyAddress.Text = "";
            CompanyNote.Text = "";
            CompanyTextID.Text = "";
        }
        private void LoadCompanyData()
        {
            string query = "SELECT * FROM company WHERE Company_ID = @Company_ID";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Company_ID", companyId);

                    connection.Open();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            CompanyName.Text = reader["Company_Name"].ToString();
                            CompanyPhoneNo.Text = reader["COMPANY_Phone_No"].ToString();
                            CompanyCity.Text = reader["Company_City"].ToString();
                            CompanyDistrict.Text = reader["Company_District"].ToString();
                            CompanyAddress.Text = reader["Company_Address"].ToString();
                            CompanyNote.Text = reader["Company_notes"].ToString();
                            CompanyTextID.Text = reader["Company_Text_Id"].ToString();
                        }
                    }

                    connection.Close();
                }
            }
        }

        private void AddComfirmButton_Click(object sender, EventArgs e)
        {
            // Perform validation
            if (string.IsNullOrEmpty(CompanyName.Text) || string.IsNullOrEmpty(CompanyPhoneNo.Text) ||
                string.IsNullOrEmpty(CompanyCity.Text) || string.IsNullOrEmpty(CompanyDistrict.Text) ||
                string.IsNullOrEmpty(CompanyAddress.Text) || string.IsNullOrEmpty(CompanyNote.Text) ||
                string.IsNullOrEmpty(CompanyTextID.Text))
            {
                MessageBox.Show("All fields are required. Please fill in all the fields.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Stop further processing
            }

            if (isEditMode)
            {
                // Update existing company data
                UpdateCompanyData();
            }
            else
            {
                // Insert new company data
                InsertCompanyData();
            }
        }


        private void InsertCompanyData()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string insertQuery = "INSERT INTO company (Company_Name, Company_Phone_No, Company_City, Company_District, Company_Address, Company_notes, Company_Text_Id, Company_Registration_Time) " +
                    "VALUES (@Company_Name, @Company_Phone_No, @Company_City, @Company_District, @Company_Address, @Company_notes, @Company_Text_Id, NOW())";

                MySqlCommand cmd = new MySqlCommand(insertQuery, conn);
                cmd.Parameters.AddWithValue("@Company_Name", CompanyName.Text);
                cmd.Parameters.AddWithValue("@Company_Phone_No", CompanyPhoneNo.Text);
                cmd.Parameters.AddWithValue("@Company_City", CompanyCity.Text);
                cmd.Parameters.AddWithValue("@Company_District", CompanyDistrict.Text);
                cmd.Parameters.AddWithValue("@Company_Address", CompanyAddress.Text);
                cmd.Parameters.AddWithValue("@Company_notes", CompanyNote.Text);
                cmd.Parameters.AddWithValue("@Company_Text_Id", CompanyTextID.Text);

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

        private void UpdateCompanyData()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string updateQuery = "UPDATE company SET Company_Name = @Company_Name, Company_Phone_No = @Company_Phone_No, Company_City = @Company_City, " +
                    "Company_District = @Company_District, Company_Address = @Company_Address, Company_notes = @Company_notes, " +
                    "Company_Text_Id = @Company_Text_Id WHERE Company_ID = @Company_ID";

                MySqlCommand cmd = new MySqlCommand(updateQuery, conn);
                cmd.Parameters.AddWithValue("@Company_Name", CompanyName.Text);
                cmd.Parameters.AddWithValue("@Company_Phone_No", CompanyPhoneNo.Text);
                cmd.Parameters.AddWithValue("@Company_City", CompanyCity.Text);
                cmd.Parameters.AddWithValue("@Company_District", CompanyDistrict.Text);
                cmd.Parameters.AddWithValue("@Company_Address", CompanyAddress.Text);
                cmd.Parameters.AddWithValue("@Company_notes", CompanyNote.Text);
                cmd.Parameters.AddWithValue("@Company_Text_Id", CompanyTextID.Text);
                cmd.Parameters.AddWithValue("@Company_ID", companyId);

                if (cmd.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("更新成功！");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("更新失敗！");
                }

                conn.Close();
            }
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void text_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

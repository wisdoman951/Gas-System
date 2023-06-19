using MySql.Data.MySqlClient;
using System.Data;
using System;
using System.Windows.Forms;
using System.Configuration;

namespace Gas_System
{
    public partial class gas_edit : Form
    {
        private readonly string connectionString = ConfigurationManager.AppSettings["ConnectionString"];
        private string gasId;
        private DataRow originalRow;
        public gas_edit()
        {
            InitializeComponent();
        }
        // ...

        public gas_edit(string gasId)
        {
            InitializeComponent();
            this.gasId = gasId;

            // Retrieve the original row data
            RetrieveOriginalRowData();

            // Autofill the elements based on the original row data
            GasCompanyID.Text = originalRow["Gas_Company_ID"].ToString();
            GasWeightFull.Text = originalRow["Gas_Weight_Full"].ToString();
            GasWeightEmpty.Text = originalRow["Gas_Weight_Empty"].ToString();
            GasType.Text = originalRow["Gas_Type"].ToString();
            GasVolume.Text = originalRow["Gas_Volume"].ToString();
            GasExamineDay.Text = originalRow["Gas_Examine_Day"].ToString();
            GasProduceDay.Text = originalRow["Gas_Produce_Day"].ToString();
        }

        private void RetrieveOriginalRowData()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM gas WHERE Gas_ID = @GasId", conn);
                cmd.Parameters.AddWithValue("@GasId", gasId);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    originalRow = dt.Rows[0];
                }
                conn.Close();
            }
        }


        private bool AreChangesMade()
        {
            return GasCompanyID.Text != originalRow["Gas_Company_ID"].ToString() ||
                   GasWeightFull.Text != originalRow["Gas_Weight_Full"].ToString() ||
                   GasWeightEmpty.Text != originalRow["Gas_Weight_Empty"].ToString() ||
                   GasType.Text != originalRow["Gas_Type"].ToString() ||
                   GasVolume.Text != originalRow["Gas_Volume"].ToString() ||
                   GasExamineDay.Text != originalRow["Gas_Examine_Day"].ToString() ||
                   GasProduceDay.Text != originalRow["Gas_Produce_Day"].ToString();
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            // Check if any changes were made
            if (AreChangesMade())
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Construct the update query
                    string updateQuery = "UPDATE gas SET " +
                                         "Gas_Company_ID = @Gas_Company_ID, " +
                                         "Gas_Weight_Full = @Gas_Weight_Full, " +
                                         "Gas_Weight_Empty = @Gas_Weight_Empty, " +
                                         "Gas_Type = @Gas_Type, " +
                                         "Gas_Volume = @Gas_Volume, " +
                                         "Gas_Supplier = @Gas_Supplier, " +
                                         "Gas_Examine_Day = STR_TO_DATE(@Gas_Examine_Day, '%Y年%m月%d日'), " +
                                         "Gas_Produce_Day = STR_TO_DATE(@Gas_Produce_Day, '%Y年%m月%d日') " +
                                         "WHERE Gas_ID = @Gas_ID";

                    MySqlCommand cmd = new MySqlCommand(updateQuery, connection);
                    cmd.Parameters.AddWithValue("@Gas_Company_ID", GasCompanyID.Text);
                    cmd.Parameters.AddWithValue("@Gas_Weight_Full", GasWeightFull.Text);
                    cmd.Parameters.AddWithValue("@Gas_Weight_Empty", GasWeightEmpty.Text);
                    cmd.Parameters.AddWithValue("@Gas_Type", GasType.Text);
                    cmd.Parameters.AddWithValue("@Gas_Volume", GasVolume.Text);
                    cmd.Parameters.AddWithValue("@Gas_Supplier", GasSupplier.Text);
                    cmd.Parameters.AddWithValue("@Gas_Examine_Day", GasExamineDay.Text);
                    cmd.Parameters.AddWithValue("@Gas_Produce_Day", GasProduceDay.Text);
                    cmd.Parameters.AddWithValue("@Gas_ID", gasId);

                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show("Update successful!");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Update failed!");
                    }
                }
            }
            else
            {
                MessageBox.Show("No changes detected.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // ...

    }
}

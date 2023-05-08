using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace Gas_System
{
    public partial class flow_meter : Form
    {
        //連接資料庫
        string connectionString = ConfigurationManager.AppSettings["ConnectionString"];

        public flow_meter()
        {
            InitializeComponent();
        }

        private void flow_meter_Load(object sender, EventArgs e)
        {

        }
    }
}

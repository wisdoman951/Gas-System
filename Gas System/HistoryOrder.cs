using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gas_System
{
    public partial class HistoryOrder : Form
    {
        private DataGridView dataGridView1;
        private DataTable data;
        public HistoryOrder()
        {
            InitializeComponent();
        }


        public void SetData(DataTable historyOrders)
        {
            data = historyOrders;

            // Process the data and display it in the form controls
            dataGridView1.DataSource = data;
        }

        private void HistoryOrder_Load(object sender, EventArgs e)
        {

        }

        private void InitializeComponent()
        {
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(-1, 1);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 27;
            this.dataGridView1.Size = new System.Drawing.Size(1009, 576);
            this.dataGridView1.TabIndex = 0;
            // 
            // HistoryOrder
            // 
            this.ClientSize = new System.Drawing.Size(1009, 578);
            this.Controls.Add(this.dataGridView1);
            this.Name = "HistoryOrder";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }
    }
}

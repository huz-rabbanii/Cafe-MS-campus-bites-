using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CampusBites
{
    public partial class CafeManager1 : Form
    {
        private void LoadInventoryItems()
        {
            var connectionString = "Data Source=DESKTOP-3LAM43F\\SQLEXPRESS;Database=campusBites;Integrated Security=True";

            using (SqlConnection sqlconn = new SqlConnection(connectionString))
            {
                sqlconn.Open();

                SqlCommand itemsCommand = new SqlCommand("SELECT Item_id, Item_name, Item_price, Item_quantity, Item_category FROM Inventory", sqlconn);

                using (SqlDataReader itemsReader = itemsCommand.ExecuteReader())
                {
                    dataGridView1.Rows.Clear();
                    dataGridView1.Columns.Clear();


                    dataGridView1.Columns.Add("Item_id", "Item ID");
                    dataGridView1.Columns.Add("Item_name", "Item Name");
                    dataGridView1.Columns.Add("Item_price", "Price");
                    dataGridView1.Columns.Add("Item_quantity", "Quantity");
                    dataGridView1.Columns.Add("Item_category", "Category");

                    // Set the background color of the entire DataGridView
                    dataGridView1.BackgroundColor = Color.FromArgb(255, 224, 192);

                    dataGridView1.DefaultCellStyle.BackColor = Color.FromArgb(255, 224, 192);



                    while (itemsReader.Read())
                    {
                        int itemId = Convert.ToInt32(itemsReader["Item_id"]);
                        string itemName = itemsReader["Item_name"].ToString();
                        decimal itemPrice = Convert.ToDecimal(itemsReader["Item_price"]);
                        int itemQuantity = Convert.ToInt32(itemsReader["Item_quantity"]);
                        string itemCategory = itemsReader["Item_category"].ToString();

                        dataGridView1.Rows.Add(itemId, itemName, itemPrice, itemQuantity, itemCategory);
                    }
                }
            }
        }
        public CafeManager1()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Additems additems = new Additems();
            //additems.Show();
            //this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //StaffManagement staffmanagment = new StaffManagement();
            //staffmanagment.Show();
            //this.Hide();
        }
    }
}

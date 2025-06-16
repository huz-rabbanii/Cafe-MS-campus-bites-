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
    public partial class InventoryManager : Form
    {

        private void LoadInventoryItems()
        {
            var connectionString = "Data Source=DESKTOP-9KA2M23\\SQLEXPRESS;Database=campusBites;Integrated Security=True";

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
        private void LoadInventoryOverview()
        {
            var connectionString = "Data Source=DESKTOP-9KA2M23\\SQLEXPRESS;Database=campusBites;Integrated Security=True";

            using (SqlConnection sqlconn = new SqlConnection(connectionString))
            {
                sqlconn.Open();

                // Get the total number of items, categories, and their quantities
                SqlCommand overviewCommand = new SqlCommand("SELECT COUNT(Item_id) AS TotalItems, COUNT(DISTINCT Item_category) AS TotalCategories, SUM(Item_quantity) AS TotalQuantity FROM Inventory", sqlconn);

                using (SqlDataReader overviewReader = overviewCommand.ExecuteReader())
                {
                    if (overviewReader.Read())
                    {
                        int totalItems = Convert.ToInt32(overviewReader["TotalItems"]);
                        int totalCategories = Convert.ToInt32(overviewReader["TotalCategories"]);
                        int totalQuantity = Convert.ToInt32(overviewReader["TotalQuantity"]);

                        //change bg color
                        dataGridView2.BackgroundColor = Color.FromArgb(255, 224, 192);
                        dataGridView2.DefaultCellStyle.BackColor = Color.FromArgb(255, 224, 192);

                        dataGridView2.Rows.Clear();
                        dataGridView2.Columns.Clear();

                        dataGridView2.Columns.Add("InfoType", "Information Type");
                        dataGridView2.Columns.Add("InfoValue", "Value");

                        dataGridView2.Rows.Add("Total Items", totalItems);
                        dataGridView2.Rows.Add("Total Categories", totalCategories);
                        dataGridView2.Rows.Add("Total Quantity", totalQuantity);

                        dataGridView2.DefaultCellStyle.BackColor = Color.FromArgb(255, 224, 192);

                    }
                }
            }
        }


        private void dataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {

            //if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count)
            //{
            //    int quantity = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["Item_quantity"].Value);

            //    // Set bg color where quantity is less than 10
            //    if (quantity < 10)
            //    {
            //        dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Orange;
            //        dataGridView1.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
            //    }

            //}
        }


        public InventoryManager()
        {
            InitializeComponent();
            LoadInventoryItems();

            dataGridView1.RowPrePaint += dataGridView1_RowPrePaint;


        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }



        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {


        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            LoadInventoryOverview();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void updateQuantity1_Click(object sender, EventArgs e)
        {

        }

        private void pendingOrders1_Click(object sender, EventArgs e)
        {
            LoadPendingOrders();
        }

        private void checkStock1_Click(object sender, EventArgs e)
        {
            LoadLowStockItems();
            LoadHighStockItems();
        }

        private void InventoryManager_Load(object sender, EventArgs e)
        {

        }

        private void LoadPendingOrders()
        {
            var connectionString = "Data Source=DESKTOP-9KA2M23\\SQLEXPRESS;Database=campusBites;Integrated Security=True";

            using (SqlConnection sqlconn = new SqlConnection(connectionString))
            {
                sqlconn.Open();

                SqlCommand pendingOrdersCommand = new SqlCommand("SELECT Order_no, customer_id, order_status, order_timestamp FROM Orders WHERE order_status = 'Pending'", sqlconn);

                using (SqlDataReader pendingOrdersReader = pendingOrdersCommand.ExecuteReader())
                {
                    dataGridView1.Rows.Clear();
                    dataGridView1.Columns.Clear();

                    dataGridView1.Columns.Add("Order_no", "Order Number");
                    dataGridView1.Columns.Add("customer_id", "Customer ID");
                    dataGridView1.Columns.Add("order_status", "Order Status");
                    dataGridView1.Columns.Add("order_timestamp", "Order Timestamp");

                    while (pendingOrdersReader.Read())
                    {
                        int orderNo = Convert.ToInt32(pendingOrdersReader["Order_no"]);
                        int customerId = Convert.ToInt32(pendingOrdersReader["customer_id"]);
                        string orderStatus = pendingOrdersReader["order_status"].ToString();

                        // Check for DBNull before conversion
                        DateTime orderTimestamp = pendingOrdersReader["order_timestamp"] != DBNull.Value
                            ? Convert.ToDateTime(pendingOrdersReader["order_timestamp"])
                            : DateTime.MinValue; // or set to some default value

                        dataGridView1.Rows.Add(orderNo, customerId, orderStatus, orderTimestamp);
                    }
                }
            }
        }

        private void LoadLowStockItems()
        {
            var connectionString = "Data Source=DESKTOP-9KA2M23\\SQLEXPRESS;Database=campusBites;Integrated Security=True";

            using (SqlConnection sqlconn = new SqlConnection(connectionString))
            {
                sqlconn.Open();

                SqlCommand lowStockCommand = new SqlCommand("SELECT Item_id, Item_name, Item_price, Item_quantity, Item_category FROM Inventory WHERE Item_quantity < 10", sqlconn);

                using (SqlDataReader lowStockReader = lowStockCommand.ExecuteReader())
                {
                    dataGridView1.Rows.Clear();
                    dataGridView1.Columns.Clear();

                    dataGridView1.Columns.Add("Item_id", "Item ID");
                    dataGridView1.Columns.Add("Item_name", "Item Name");
                    dataGridView1.Columns.Add("Item_price", "Price");
                    dataGridView1.Columns.Add("Item_quantity", "Quantity");
                    dataGridView1.Columns.Add("Item_category", "Category");

                    while (lowStockReader.Read())
                    {
                        int itemId = Convert.ToInt32(lowStockReader["Item_id"]);
                        string itemName = lowStockReader["Item_name"].ToString();
                        decimal itemPrice = Convert.ToDecimal(lowStockReader["Item_price"]);
                        int itemQuantity = Convert.ToInt32(lowStockReader["Item_quantity"]);
                        string itemCategory = lowStockReader["Item_category"].ToString();

                        dataGridView1.Rows.Add(itemId, itemName, itemPrice, itemQuantity, itemCategory);
                    }
                }
            }
        }

        private void LoadHighStockItems()
        {
            var connectionString = "Data Source=DESKTOP-9KA2M23\\SQLEXPRESS;Database=campusBites;Integrated Security=True";

            using (SqlConnection sqlconn = new SqlConnection(connectionString))
            {
                sqlconn.Open();

                SqlCommand highStockCommand = new SqlCommand("SELECT Item_id, Item_name, Item_price, Item_quantity, Item_category FROM Inventory WHERE Item_quantity > 100", sqlconn);

                using (SqlDataReader highStockReader = highStockCommand.ExecuteReader())
                {
                    dataGridView2.Rows.Clear();
                    dataGridView2.Columns.Clear();

                    dataGridView2.Columns.Add("Item_id", "Item ID");
                    dataGridView2.Columns.Add("Item_name", "Item Name");
                    dataGridView2.Columns.Add("Item_price", "Price");
                    dataGridView2.Columns.Add("Item_quantity", "Quantity");
                    dataGridView2.Columns.Add("Item_category", "Category");

                    while (highStockReader.Read())
                    {
                        int itemId = Convert.ToInt32(highStockReader["Item_id"]);
                        string itemName = highStockReader["Item_name"].ToString();
                        decimal itemPrice = Convert.ToDecimal(highStockReader["Item_price"]);
                        int itemQuantity = Convert.ToInt32(highStockReader["Item_quantity"]);
                        string itemCategory = highStockReader["Item_category"].ToString();

                        dataGridView2.Rows.Add(itemId, itemName, itemPrice, itemQuantity, itemCategory);
                    }
                }
            }
        }
    }
}

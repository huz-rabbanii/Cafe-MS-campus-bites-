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
    public partial class Customer : Form
    {
        private List<TemporaryOrderDetail> temporaryOrderDetails;

        public List<OrderDetail> OrderDetails
        {
            get { return orderDetails; }
        }
        private List<OrderDetail> orderDetails;


        //private void PopulateItemTypeDropdown()
        //{
        //    // Your database connection string
        //    string connectionString = "Data Source=DESKTOP-9KA2M23\\SQLEXPRESS;Initial Catalog=campusBites;Integrated Security=True";

        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        connection.Open();

        //        // Retrieve distinct item types from the Inventory table
        //        string query = "SELECT DISTINCT Item_category FROM Inventory";
        //        SqlCommand command = new SqlCommand(query, connection);
        //        SqlDataReader reader = command.ExecuteReader();

        //        // Add item types to the dropdown
        //        while (reader.Read())
        //        {
        //            string itemType = reader["Item_category"].ToString();
        //            itemType.Items.Add(itemType);
        //        }

        //        // Close the reader
        //        reader.Close();
        //    }
        //}


        //DISPLAYING CATEGORIES
        private void LoadCategories()
        {
            string connectionString = "Data Source=DESKTOP-9KA2M23\\SQLEXPRESS;Initial Catalog=campusBites;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT DISTINCT Item_category FROM Inventory";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string category = reader["Item_category"].ToString();
                    category1.Items.Add(category);
                    viewMenu1.Items.Add(category);
                }

                reader.Close();
            }
        }

        //DISPLAYING QUANTITIES
        private void LoadQuantity(string itemName)
        {
            string connectionString = "Data Source=DESKTOP-9KA2M23\\SQLEXPRESS;Initial Catalog=campusBites;Integrated Security=True";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = $"SELECT Item_quantity FROM Inventory WHERE Item_name = '{itemName}'";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    // Check if the "Item_quantity" column is not null and is of numeric type
                    if (reader["Item_quantity"] != DBNull.Value && int.TryParse(reader["Item_quantity"].ToString(), out int itemQuantity))
                    {
                        if (itemQuantity > 0)
                        {
                            // Item is in stock
                            quantity1.Items.Clear();
                            for (int i = 1; i <= itemQuantity; i++)
                            {
                                quantity1.Items.Add(i);
                            }
                        }
                        else
                        {
                            // Item is out of stock
                            quantity1.Items.Clear();
                            quantity1.Items.Add("Out of Stock");
                        }
                    }
                    else
                    {
                        // Invalid or null quantity
                        quantity1.Items.Clear();
                        quantity1.Items.Add("Invalid Quantity");
                    }
                }
                else
                {
                    // Item not found (shouldn't happen if data is consistent)
                    quantity1.Items.Clear();
                }

                reader.Close();
            }
        }

        //DISPLAYING ITEMS
        private void LoadItems(string category)
        {
            string connectionString = "Data Source=DESKTOP-9KA2M23\\SQLEXPRESS;Initial Catalog=campusBites;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = $"SELECT Item_name FROM Inventory WHERE Item_category = '{category}'";
                SqlCommand command = new SqlCommand(query, connection);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string itemName = reader["Item_name"].ToString();
                    name1.Items.Add(itemName);
                }

                reader.Close();

                string selectedItem = name1.SelectedItem?.ToString();
                quantity1.Items.Clear();

                if (!string.IsNullOrEmpty(selectedItem))
                {
                    LoadQuantity(selectedItem);
                }
            }
        }
        private void LoadAvailableItems()
        {
            // Your database connection string
            var connectionString = "Data Source=DESKTOP-9KA2M23\\SQLEXPRESS;Initial Catalog=campusBites;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Retrieve available items from the Inventory table
                string query = "SELECT Item_id, Item_name, Item_price, Item_quantity, Item_category FROM Inventory";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                // Subtract the selected quantity from the available quantity in the DataTable
                foreach (var item in temporaryOrderDetails)
                {
                    var row = dataTable.AsEnumerable().FirstOrDefault(r => r.Field<string>("Item_name") == item.ItemName);
                    if (row != null)
                    {
                        int availableQuantity = row.Field<int>("Item_quantity");
                        row.SetField("Item_quantity", availableQuantity - item.Quantity);
                    }
                }

                // Bind the DataTable to the DataGridView
                dataGridView1.DataSource = dataTable;
            }
        }

        // New property to store customer ID
        public int CustomerId1 { get; private set; }
        public Customer(int customerId)
        {
            InitializeComponent();

            // Initialize the orderDetails list
            orderDetails = new List<OrderDetail>();
            temporaryOrderDetails = new List<TemporaryOrderDetail>();

            LoadAvailableItems();

            LoadCategories();

            CustomerId1 = customerId;
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void label3_Click(object sender, EventArgs e)
        {

        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void category1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedCategory = category1.SelectedItem?.ToString();
            name1.Items.Clear();
            quantity1.Items.Clear();

            if (!string.IsNullOrEmpty(selectedCategory))
            {
                LoadItems(selectedCategory);
            }
        }
        private void name1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            string selectedItem = name1.SelectedItem?.ToString();
            quantity1.Items.Clear();

            if (!string.IsNullOrEmpty(selectedItem))
            {
                // Add a breakpoint or print statements for debugging
                Console.WriteLine($"Selected item: {selectedItem}");

                // Load the quantity
                LoadQuantity(selectedItem);

                // Add a breakpoint or print statements for debugging
                Console.WriteLine("LoadQuantity called");
            }
        }
        private void itemType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void button6_Click(object sender, EventArgs e)
        {
            string selectedItem = name1.SelectedItem?.ToString();
            string selectedCategory = category1.SelectedItem?.ToString();
            string selectedQuantity = quantity1.SelectedItem?.ToString();

            if (!string.IsNullOrEmpty(selectedItem) && !string.IsNullOrEmpty(selectedCategory) && !string.IsNullOrEmpty(selectedQuantity))
            {
                // Assuming you have a method to get the price of the selected item
                decimal itemPrice = GetItemPrice(selectedItem);

                // Calculate total
                decimal total = CalculateTotal(itemPrice, Convert.ToInt32(selectedQuantity));

                // Update total1 TextBox (accumulate total)
                if (decimal.TryParse(total1.Text, out decimal currentTotal))
                {
                    total1.Text = (currentTotal + total).ToString();
                }
                else
                {
                    // Handle the case where the current total is not in the correct format
                    MessageBox.Show("Error updating total. Please check the format of the current total.");
                }

                // Add the order details to the list
                orderDetails.Add(new OrderDetail
                {
                    ItemName = selectedItem,
                    Category = selectedCategory,
                    Quantity = Convert.ToInt32(selectedQuantity),
                    Price = itemPrice,
                    Total = total
                });

                // Add the order details to the temporary list
                temporaryOrderDetails.Add(new TemporaryOrderDetail
                {
                    ItemName = selectedItem,
                    Category = selectedCategory,
                    Quantity = Convert.ToInt32(selectedQuantity),
                    Price = itemPrice
                });

                // Clear input fields
                name1.Text = string.Empty; // or use name1.Text = string.Empty;
                category1.Text = string.Empty; // or use category1.Text = string.Empty;
                quantity1.Text = string.Empty; // or use quantity1.Text = string.Empty;

                LoadAvailableItems();

                
                

                
            }
            else
            {
                MessageBox.Show("Please select item, category, and quantity before adding to the order.");
            }
        }

        //private void name1_SelectedIndexChanged_1(object sender, EventArgs e)
        //{

        //}
        private decimal GetItemPrice(string itemName)
        {
            // Your database connection string
            string connectionString = "Data Source=DESKTOP-9KA2M23\\SQLEXPRESS;Initial Catalog=campusBites;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Query to retrieve the price for the specified item
                string query = $"SELECT Item_price FROM Inventory WHERE Item_name = '{itemName}'";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Execute the query
                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        // If the result is not null, return the decimal value
                        return Convert.ToDecimal(result);
                    }
                    else
                    {
                        // Handle the case where the item price is not found
                        // You might want to throw an exception or handle it in another way
                        throw new InvalidOperationException($"Item price for '{itemName}' not found.");
                    }
                }
            }
        }
        //
        // Calculate total based on item price and quantity
        private decimal CalculateTotal(decimal itemPrice, int quantity)
        {
            return itemPrice * quantity;
        }

        private void viewOrder1_Click(object sender, EventArgs e)
        {
            customerOrder customerOrderForm = new customerOrder(CustomerId1);
            customerOrderForm.CustomerId2 = CustomerId1; // Set the customer ID
            customerOrderForm.OrderDetails = orderDetails;
            customerOrderForm.Show();
            this.Hide();
        }
        //
        private class TemporaryOrderDetail
        {
            public string ItemName { get; set; }
            public string Category { get; set; }
            public int Quantity { get; set; }
            public decimal Price { get; set; }
        }

        private void viewMenu1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Customer_Load(object sender, EventArgs e)
        {

        }
    }
}



// OrderDetail class to store order information
public class OrderDetail
{
    public string ItemName { get; set; }
    public string Category { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Total { get; set; }
}



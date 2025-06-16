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
    public partial class customerOrder : Form
    {
        
        private BindingList<OrderDetail> orderDetailsList;

        // Property to set the order details
        public List<OrderDetail> OrderDetails
        {
            set
            {  
                orderDetailsList = new BindingList<OrderDetail>(value);
                dataGridView1.DataSource = orderDetailsList;
                dataGridView1.AutoResizeColumns();
            }
        }
        public int CustomerId2 { get; set; }


        public customerOrder(int customerId)
        {
            InitializeComponent();
            CustomerId2 = customerId;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void checkout1_Click(object sender, EventArgs e)
        {
            // Calculate total amount
            decimal totalAmount = orderDetailsList.Sum(item => item.Total);

            // Display total amount and ask for confirmation
            DialogResult result = MessageBox.Show($"Your total amount is: {totalAmount:C2}\nDo you want to confirm your order?", "Confirm Order", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // The customer confirmed the order
                // You can update the database here and perform other necessary actions

                // Insert a new record into the Bill table and get the generated billId
                int billId = InsertBillRecord(totalAmount);

                if (billId != -1)
                {
                    // Update the inventory permanently
                    UpdateInventory();

                    // Display a success message
                    MessageBox.Show("Order confirmed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Open the Bill form with billing details
                    OpenBillForm(billId);

                    // Close the current form
                    this.Close();
                }
                // Handle the case where the billId is not valid
            }
            else
            {
                // The customer did not confirm the order
                // Revert any temporary changes made to the inventory
                RevertInventoryChanges();

                // Display a message indicating that the order was not confirmed
                MessageBox.Show("Order not confirmed. Your changes have been reverted.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Close the current form
                this.Close();
            }
        }
        // Method to insert a new record into the Bill table
        private int InsertBillRecord(decimal totalAmount)
        {
            // Your database connection string
            string connectionString = "Data Source=DESKTOP-9KA2M23\\SQLEXPRESS;Initial Catalog=campusBites;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Insert into the Orders table
                string orderQuery = "INSERT INTO Orders (customer_id, order_status, order_timestamp) VALUES (@CustomerId, 'Pending', GETDATE()); SELECT SCOPE_IDENTITY()";

                using (SqlCommand orderCommand = new SqlCommand(orderQuery, connection))
                {
                    orderCommand.Parameters.AddWithValue("@CustomerId", CustomerId2);

                    // Execute the query and retrieve the generated orderNo
                    object orderNoObject = orderCommand.ExecuteScalar();

                    if (orderNoObject != null && orderNoObject != DBNull.Value)
                    {
                        int orderNo = Convert.ToInt32(orderNoObject);

                        // Insert into the order_items table
                        foreach (var item in orderDetailsList)
                        {
                            string orderItemsQuery = "INSERT INTO order_items (order_no, number_of_items, total_price, item_id) VALUES (@OrderNo, @NumberOfItems, @TotalPrice, @ItemId)";

                            using (SqlCommand orderItemsCommand = new SqlCommand(orderItemsQuery, connection))
                            {
                                orderItemsCommand.Parameters.AddWithValue("@OrderNo", orderNo);
                                orderItemsCommand.Parameters.AddWithValue("@NumberOfItems", item.Quantity);
                                orderItemsCommand.Parameters.AddWithValue("@TotalPrice", item.Total);
                                orderItemsCommand.Parameters.AddWithValue("@ItemId", GetItemIdByName(item.ItemName));

                                orderItemsCommand.ExecuteNonQuery();
                            }
                        }

                        // Continue with the rest of the Bill table insertion
                        string billQuery = "INSERT INTO Bill (Amount, customer_id) VALUES (@Amount, @CustomerId); SELECT SCOPE_IDENTITY()";

                        using (SqlCommand command = new SqlCommand(billQuery, connection))
                        {
                            command.Parameters.AddWithValue("@Amount", totalAmount);
                            command.Parameters.AddWithValue("@CustomerId", CustomerId2);

                            // Execute the query and retrieve the generated billId
                            object billIdObject = command.ExecuteScalar();

                            if (billIdObject != null && billIdObject != DBNull.Value)
                            {
                                return Convert.ToInt32(billIdObject);
                            }
                            else
                            {
                                MessageBox.Show("Error getting Bill ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return -1;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error getting Order No.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return -1;
                    }
                }
            }
        }

        // Helper method to get Item_id by Item_name
        private int GetItemIdByName(string itemName)
        {
            // Your database connection string
            string connectionString = "Data Source=DESKTOP-9KA2M23\\SQLEXPRESS;Initial Catalog=campusBites;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT Item_id FROM Inventory WHERE Item_name = @ItemName";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ItemName", itemName);

                    object itemIdObject = command.ExecuteScalar();

                    if (itemIdObject != null && itemIdObject != DBNull.Value)
                    {
                        return Convert.ToInt32(itemIdObject);
                    }
                    else
                    {
                        MessageBox.Show($"Error getting Item ID for {itemName}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return -1;
                    }
                }
            }
        }


        // Method to update the inventory permanently
        private void UpdateInventory()
        {
            // Your database connection string
            string connectionString = "Data Source=DESKTOP-9KA2M23\\SQLEXPRESS;Initial Catalog=campusBites;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Update the Inventory table based on the temporary changes
                foreach (var item in orderDetailsList)
                {
                    string updateQuery = $"UPDATE Inventory SET Item_quantity = Item_quantity - {item.Quantity} WHERE Item_name = '{item.ItemName}'";

                    using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                    {
                        updateCommand.ExecuteNonQuery();
                    }
                }
            }
        }

        // Method to revert any temporary changes made to the inventory
        private void RevertInventoryChanges()
        {
            // Your database connection string
            string connectionString = "Data Source=DESKTOP-9KA2M23\\SQLEXPRESS;Initial Catalog=campusBites;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Revert the Inventory table based on the temporary changes
                foreach (var item in orderDetailsList)
                {
                    string revertQuery = $"UPDATE Inventory SET Item_quantity = Item_quantity + {item.Quantity} WHERE Item_name = '{item.ItemName}'";

                    using (SqlCommand revertCommand = new SqlCommand(revertQuery, connection))
                    {
                        revertCommand.ExecuteNonQuery();
                    }
                }
            }
        }

        private void OpenBillForm(int billId)
        {
            Bill billForm = new Bill(billId);
            billForm.Show();
            this.Hide();
        }



        private void customerOrder_Load(object sender, EventArgs e)
        {

        }
    }
}

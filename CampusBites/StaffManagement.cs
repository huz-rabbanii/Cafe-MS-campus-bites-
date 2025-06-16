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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CampusBites
{
    public partial class StaffManagement : Form
    {
        public StaffManagement()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var Name = textBox1.Text;


            try
            {
                // Connection string for your SQL Server
                string connectionString = "Data Source=DESKTOP-3LAM43F\\SQLEXPRESS;Initial Catalog=campusBites;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Assuming you have a table named "YourTable" with columns: Item_id, Item_name, Item_price, Item_quantity, Item_category
                    string insertQuery = "INSERT INTO Staff (staffId,name,user_id) VALUES (14,@Name,5) ";

                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        // Set parameters based on your actual column names and types
                        command.Parameters.AddWithValue("@Name", Name);


                        // Execute the INSERT query
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Item added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Failed to add item.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var idString = textBox2.Text;


            if (int.TryParse(idString, out int id))
            {
                string connectionString = "Data Source=DESKTOP-3LAM43F\\SQLEXPRESS;Initial Catalog=campusBites;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string deleteQuery = "DELETE FROM Staff WHERE staffId = @Id";

                    using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            Console.WriteLine($"Row with ID {id} deleted successfully.");
                        }
                        else
                        {
                            Console.WriteLine($"No rows found with ID {id}.");
                        }
                    }
                }
            }
            else
            {
                // Handle the case where the parsing fails (e.g., invalid input)
                MessageBox.Show("Invalid ID. Please enter a valid integer.");
            }

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

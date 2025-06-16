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
    public partial class Bill : Form
    {
        private int billId1;

        // Updated constructor to receive Bill ID
        public Bill(int billId)
        {
            InitializeComponent();
            this.billId1 = billId;
        }

        private void Bill_Load(object sender, EventArgs e)
        {
            // Retrieve order details and customer ID based on the Bill ID
            // Call a method to fetch and display this information
            // Example method (replace with your actual method)
            DisplayBillInformation();
        }

        private void DisplayBillInformation()
        {
            // Your database connection string
            string connectionString = "Data Source=DESKTOP-9KA2M23\\SQLEXPRESS;Initial Catalog=campusBites;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Query to retrieve order details and customer ID based on the Bill ID
                string query = @"
            SELECT
                  O.Order_no,
                 
                  O.order_status,
                 C.customerId,
              C.name AS CustomerName,
                  I.Item_name AS ItemName,
                  OD.number_of_items AS Quantity,
                   I.Item_price AS Price,
                    OD.total_price AS Total
            FROM
                          Orders O
                              JOIN Customer C ON O.customer_id = C.customerId
                             JOIN order_items OD ON O.Order_no = OD.order_no
                         JOIN Inventory I ON OD.item_id = I.Item_id
                          JOIN Bill B ON C.customerid = B.customer_id
            WHERE
                     B.Bill_no = @Bill_no";
                
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Bill_no", billId1);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Check if there are rows returned
                        if (reader.Read())
                        {
                            // Set the DataSource property of the DataGridView
                            dataGridView1.DataSource = GetDataTable(reader);

                            // Adjust column headers, etc., as needed
                            // dataGridView1.Columns[0].HeaderText = "New Header";
                        }
                        else
                        {
                            // Handle the case where no rows were returned
                            MessageBox.Show("Bill not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private DataTable GetDataTable(SqlDataReader reader)
        {
            // Create a DataTable and load data from the SqlDataReader
            DataTable dataTable = new DataTable();
            dataTable.Load(reader);
            return dataTable;
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        // ... (other methods and event handlers)
    }

}

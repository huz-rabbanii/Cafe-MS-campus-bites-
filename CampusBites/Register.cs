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
    public partial class Register : Form
    {
        public Register()
        {
            InitializeComponent();
            contactNumber1.Visible = false;
            contactNumber2.Visible = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var userType1 = userType.SelectedItem.ToString();

            // Show or hide the contact number field based on user type
            if (userType1 == "Customer" || userType1 == "Cafe Manager")
            {
                contactNumber1.Visible = true;
                contactNumber2.Visible = true;
            }
            else
            {
                contactNumber1.Visible = false;
                contactNumber2.Visible = false;
            }
        }

        private void password_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Hide();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            //ERROR HANDLING
            if (string.IsNullOrEmpty(name.Text) || string.IsNullOrEmpty(userName.Text) || string.IsNullOrEmpty(password.Text) || userType.SelectedItem == null)
            {
                MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var userName1 = userName.Text;
            var password1 = password.Text;
            var userType1 = userType.SelectedItem.ToString();
            var contactNum1 = contactNumber2.Text;
            var name1 = name.Text;

            if (userType1 == "Customer" || userType1 == "Cafe Manager")
            {
                if (string.IsNullOrEmpty(contactNum1))
                {
                    MessageBox.Show("Please enter contact Number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            var connectionString = "Data Source=DESKTOP-9KA2M23\\SQLEXPRESS;Initial Catalog=campusBites;Integrated Security=True";

           


            // Additional checks for password length
            if (password1.Length < 6)
            {
                MessageBox.Show("Password must be at least 6 characters long.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Insert into Users table
                string userQuery = "INSERT INTO Users (UserName, Password, UserType) VALUES (@UserName, @Password, @UserType); SELECT SCOPE_IDENTITY()";
                using (SqlCommand userCommand = new SqlCommand(userQuery, connection))
                {
                    userCommand.Parameters.AddWithValue("@UserName", userName1);
                    userCommand.Parameters.AddWithValue("@Password", password1);
                    userCommand.Parameters.AddWithValue("@UserType", userType1);

                    object userIdObject = userCommand.ExecuteScalar();
                    if (userIdObject != null && userIdObject != DBNull.Value)
                    {
                        int userId = Convert.ToInt32(userIdObject);

                        // Insert into respective table based on UserType
                        string query = "";
                        switch (userType1)
                        {
                            case "Cafe Manager":
                                query = "INSERT INTO CafeManager (cafeManagerId, Name, Contact_No) VALUES (@UserId, @Name, @ContactNumber)";
                                break;

                            case "Cashier":
                                query = "INSERT INTO Cashier (cashierId, Name) VALUES (@UserId, @Name)";
                                break;

                            case "Inventory Manager":
                                query = "INSERT INTO InventoryManager (inventoryManagerId, Name) VALUES (@UserId, @Name)";
                                break;

                            case "Staff":
                                query = "INSERT INTO Staff (staffId, Name) VALUES (@UserId, @Name)";
                                break;

                            case "Customer":

                                query = "INSERT INTO Customer (customerId, Name, Contact_No) VALUES (@UserId, @Name, @ContactNumber)";
                                break;

                            default:
                                MessageBox.Show("Invalid user type.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                        }

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            // Set UserId parameter
                            command.Parameters.AddWithValue("@UserId", userId);
                            command.Parameters.AddWithValue("@Name", name1);
                            if (userType1 == "Cafe Manager" ||  userType1 == "Customer")
                            {
                                command.Parameters.AddWithValue("@ContactNumber", contactNum1);
                            }

                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("User registered successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("Error registering user.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error getting UserId.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void name_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

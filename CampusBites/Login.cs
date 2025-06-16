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
using System.Xml.Linq;

namespace CampusBites
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void login_Click(object sender, EventArgs e)
        {

        }
        private void OpenRespectiveForm(string userType, int userId)
        {
            switch (userType)
            {
                case "Inventory Manager":
                    InventoryManager inventoryManagerForm = new InventoryManager();
                    inventoryManagerForm.Show();
                    this.Hide();
                    break;

                case "Cafe Manager":
                    CafeManager1 cafeManager1 = new CafeManager1();
                    cafeManager1.Show();
                    this.Hide();
                    break;

                case "Customer":
                    Customer customerForm = new Customer(userId);
                    customerForm.Show();
                    this.Hide();
                    break;

                case "Cashier":
                    Cashier CashierForm = new Cashier();
                    CashierForm.Show();
                    this.Hide();
                    break;

                case "Staff":
                    Staff StaffForm = new Staff();
                    StaffForm.Show();
                    this.Hide();
                    break;
                default:
                    MessageBox.Show("Invalid user type.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(userName.Text) || string.IsNullOrEmpty(password.Text) || userType.SelectedItem == null)
            {
                MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var userName1 = userName.Text;
            var password1 = password.Text;
            var userType1 = userType.SelectedItem.ToString();

            var connectionString = "Data Source=DESKTOP-9KA2M23\\SQLEXPRESS;Initial Catalog=campusBites;Integrated Security=True";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT UserId, UserName, UserType FROM Users WHERE UserName = @UserName AND Password = @Password AND UserType = @UserType";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserName", userName1);
                    command.Parameters.AddWithValue("@Password", password1);
                    command.Parameters.AddWithValue("@UserType", userType1);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int userId = reader.GetInt32(reader.GetOrdinal("UserId"));
                            string retrievedUserName = reader.GetString(reader.GetOrdinal("UserName"));
                            string retrievedUserType = reader.GetString(reader.GetOrdinal("UserType"));

                            MessageBox.Show($"Login successful!\nUser ID: {userId}\nUsername: {retrievedUserName}\nUser Type: {retrievedUserType}",
                                "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            OpenRespectiveForm(retrievedUserType, userId); //see again, want to get name of user
                        }
                        else
                        {
                            MessageBox.Show("Invalid credentials.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void userType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void userName_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Register registerForm = new Register();
            registerForm.Show();
            this.Hide();
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
    }
}

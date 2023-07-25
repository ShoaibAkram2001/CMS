using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using CMS.Pages;
namespace CMS.Pages.Admin
{
    public class ViewUserModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public List<User> Users { get; set; }

        public ViewUserModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet()
        {
            if (HttpContext.Session.GetString("UserType") != "admin")
            {
                // Redirect to login if not authenticated as an admin
                Response.Redirect("/index");
            }
            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Use a SQL query to fetch all users from the database
                    string query = "SELECT Username, Password, Email FROM Users";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            Users = new List<User>();

                            while (reader.Read())
                            {
                                User user = new User
                                {
                                    Username = reader["Username"].ToString(),
                                    Password = reader["Password"].ToString(),
                                    Email = reader["Email"].ToString()
                                };

                                Users.Add(user);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                // Handle any exceptions that might occur during the database operation
                // Log the error or show an error message to the user
            }
        }
    }
}

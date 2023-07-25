using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;

namespace CMS.Pages.Admin
{
    public class DeleteUserModel : PageModel
    {
        private readonly IConfiguration _configuration;

        [BindProperty]
        public string Username { get; set; }

        public DeleteUserModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult OnGet()
        {
            /*
            // Fetch user details from the database based on the username
            // Use a SQL query to retrieve the user record

            if (userRecord == null)
            {
                // Handle case when the user with the provided username is not found
                return RedirectToPage("/Admin/Users");
            }

            Username = userRecord.Username;
            // Set other user properties
             */
            return Page();
           
        }

        public IActionResult OnPost()
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Use a SQL query to delete the user record based on the username
                    string deleteQuery = "DELETE FROM Users WHERE Username = @Username";

                    using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Username", Username);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            // Redirect to the Users page after successful deletion
                            return RedirectToPage("/AdminDashboard");
                        }
                        else
                        {
                            // Handle case when the user with the provided username is not found
                            return RedirectToPage("/AdminDashboard");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that might occur during the database operation
                // Log the error or show an error message to the user
                return Page();
            }
        }
    }
}

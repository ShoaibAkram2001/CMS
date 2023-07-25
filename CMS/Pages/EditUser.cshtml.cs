using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;

namespace CMS.Pages.Admin
{
    public class EditUserModel : PageModel
    {
        private readonly IConfiguration _configuration;

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public EditUserModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult OnGet()
        { 

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                // Return the page with validation errors if the input is not valid
                return Page();
            }

            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Use a SQL query to update the user record with the new details
                    string updateQuery = "UPDATE Users SET Email = @Email, Password = @Password WHERE Username = @Username";

                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Username", Username);
                        command.Parameters.AddWithValue("@Email", Email);
                        command.Parameters.AddWithValue("@Password", Password); // Note: Password should be hashed before saving it to the database in a real-world scenario

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            // Redirect to the Users page after successful update
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
                // Handle any exceptions that might occur during the database update
                // Log the error or show an error message to the user
                return Page();
            }
        }
    }
}

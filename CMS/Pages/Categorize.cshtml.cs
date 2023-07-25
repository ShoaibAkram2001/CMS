using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using CMS.Pages;

namespace CMS.Pages
{
    public class CategorizeModel : PageModel
    {
        private readonly IConfiguration _configuration;

        [BindProperty]
        public string FirstName { get; set; }

        [BindProperty]
        public string Category { get; set; }

        public CategorizeModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public void onGet()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
            {
                // Redirect to login if not authenticated
                Response.Redirect("/index");
            }

        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                // Handle validation errors, if any
                return Page();
            }

            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Update the contact's category in the database
                    string updateQuery = "UPDATE Contacts SET Category = @category WHERE FirstName = @FirstName";

                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Category", Category);
                        command.Parameters.AddWithValue("@FirstName", FirstName);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            // Successfully updated the contact's category
                            // Redirect to the "ViewContacts" page
                            return RedirectToPage("/View");
                        }
                        else
                        {
                            // Handle case when the contact with the provided first name is not found
                            // For example, show an error message to the user
                            ModelState.AddModelError(string.Empty, "Contact not found with the provided first name.");
                            return Page();
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                // Handle any exceptions that might occur during the database operation
                // Log the error or show an error message to the user
                ModelState.AddModelError(string.Empty, "An error occurred while updating the contact category.");
                return Page();
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using Xunit;

namespace CMS.Pages
{
    public class CreateUserModel : PageModel
    {
        private readonly IConfiguration _configuration;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public CreateUserModel(IConfiguration configuration)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            _configuration = configuration;
        }

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        [BindProperty]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        public void OnGet()
        {
            if (HttpContext.Session.GetString("UserType") != "admin")
            {
                // Redirect to login if not authenticated as an admin
                Response.Redirect("/index");
            }


            // This is the default GET handler for the Signup page

        }



        public IActionResult OnPost()
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO Users (Username, Email, Password) VALUES (@Username, @Email, @Password)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        connection.Open();
                        command.Parameters.AddWithValue("@Username", Username);
                        command.Parameters.AddWithValue("@Email", Email);
                        command.Parameters.AddWithValue("@Password", Password);

                        int rowsAffected = command.ExecuteNonQuery();

                        Console.WriteLine(rowsAffected);
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("Successfully Created");
                            return RedirectToPage("/AdminDashboard");
                        }

                        ErrorMessage = "Not Successully Created";
                        //return RedirectToPage("/Error");
                        return Page();

                    }
                }


            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;

namespace CMS.Pages
{
    public class EditModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public EditModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public Guid Id { get; set; }

        [BindProperty]
        public string? FirstName { get; set; }

        [BindProperty]
        public string? LastName { get; set; }

        [BindProperty]
        public string? Email { get; set; }

        [BindProperty]
        public string? Phone { get; set; }


        public string? ErrorMessage { get; set; }

        public void OnGet()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
            {
                // Redirect to login if not authenticated
                Response.Redirect("/index");
            }
            // This is the default GET handler for the Create page
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
                    string query = "UPDATE Contacts SET FirstName = @FirstName, LastName = @LastName, Email = @Email WHERE Phone = @PhoneNumber";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        
                        command.Parameters.AddWithValue("@FirstName", FirstName);
                        command.Parameters.AddWithValue("@LastName", LastName);
                        command.Parameters.AddWithValue("@Email", Email);
                        command.Parameters.AddWithValue("@Phone", Phone);

                        connection.Open();
                        int row = command.ExecuteNonQuery();

                        if (row > 0)
                        {
                            return RedirectToPage("/Dashboard");
                        }
                    }
                }

                ErrorMessage = "Contact has not been Created";
                //return RedirectToPage("/Error");
                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }
        }
    }
}

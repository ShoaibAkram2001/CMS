using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;

namespace CMS.Pages
{
    public class CreateModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public CreateModel(IConfiguration configuration)
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
        [BindProperty]
        public string? Category { get; set; }


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
                    string query = "INSERT INTO Contacts (Id,FirstName, LastName, Email, Phone,Category)  VALUES (@ID,@FirstName, @LastName, @Email, @Phone,@category)"; ;

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        Id = Guid.NewGuid(); // Generate a unique ID
                        command.Parameters.AddWithValue("@ID", Id);
                        command.Parameters.AddWithValue("@FirstName", FirstName);
                        command.Parameters.AddWithValue("@LastName", LastName);
                        command.Parameters.AddWithValue("@Email", Email);
                        command.Parameters.AddWithValue("@Phone", Phone);
                        command.Parameters.AddWithValue("@category", Category);
                        connection.Open();
                    int row =  command.ExecuteNonQuery();

                        if(row >0)
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

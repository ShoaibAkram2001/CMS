
using System;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using CMS.Pages;

namespace CMS.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly IConfiguration _configuration;

        [BindProperty(SupportsGet = true)]
        public string FirstName { get; set; }

        public Contact Contact { get; set; }

        public DeleteModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult OnGet()
        {
            if (!string.IsNullOrEmpty(FirstName))
            {
                Contact = GetContactByFirstName(FirstName);
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!string.IsNullOrEmpty(FirstName))
            {
                DeleteContactByFirstName(FirstName);
            }

            return RedirectToPage("View");
        }

        private Contact GetContactByFirstName(string firstName)
        {
            Contact contact = null;

            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT TOP 1 FirstName, LastName, Email, Phone FROM Contacts WHERE FirstName = @FirstName";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FirstName", firstName);

                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            contact = new Contact
                            {
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                Email = reader["Email"].ToString(),
                                Phone = reader["Phone"].ToString()
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception or log error
            }

            return contact;
        }

        private void DeleteContactByFirstName(string firstName)
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM Contacts WHERE FirstName = @FirstName";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FirstName", firstName);

                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception or log error
            }
        }
    }
}

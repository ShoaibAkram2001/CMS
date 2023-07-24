using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using CMS.Pages;

namespace CMS.Pages
{
    public class SortModel : PageModel
    {
        private readonly IConfiguration _configuration;

        [BindProperty(SupportsGet = true)]
        public string FirstName { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SortOrder { get; set; } = "asc";

        public List<Contact> Contacts { get; set; }

        public SortModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult OnGet()
        {
            Contacts = GetAllContacts(FirstName, SortOrder);
            return Page();
        }

        private List<Contact> GetAllContacts(string firstName, string sortOrder)
        {
            List<Contact> contacts = new List<Contact>();

            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT FirstName, LastName, Email, Phone FROM Contacts ";

                    // If FirstName is provided, add WHERE clause for filtering
                    if (!string.IsNullOrEmpty(firstName))
                    {
                        query += "WHERE FirstName LIKE @FirstName ";
                    }

                    query += "ORDER BY FirstName " + sortOrder;

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        if (!string.IsNullOrEmpty(firstName))
                        {
                            command.Parameters.AddWithValue("@FirstName", $"%{firstName}%");
                        }

                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            Contact contact = new Contact
                            {
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                Email = reader["Email"].ToString(),
                                Phone = reader["Phone"].ToString()
                            };

                            contacts.Add(contact);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception or log error
            }

            return contacts;
        }
    }
}

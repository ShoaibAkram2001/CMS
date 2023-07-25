using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace CMS.Pages
{
    public class DuplicateModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public List<Contact> DuplicateContacts { get; set; } = new List<Contact>();

        [BindProperty]
        public string SearchPhoneNumber { get; set; }

        public DuplicateModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
            {
                // Redirect to login if not authenticated
                Response.Redirect("/index");
            }


            // Fetch all contacts from the database
            List<Contact> allContacts = GetAllContactsFromDatabase();

            // Find duplicates based on phone number
            var duplicates = allContacts
                .GroupBy(c => c.Phone)
                .Where(g => g.Count() > 1)
                .SelectMany(g => g)
                .ToList();

            DuplicateContacts.AddRange(duplicates);
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed, return the page with errors
                return Page();
            }

            // Fetch all contacts from the database
            List<Contact> allContacts = GetAllContactsFromDatabase();

            // Find duplicates based on the provided phone number
            var duplicates = allContacts
                .Where(c => c.Phone == SearchPhoneNumber)
                .ToList();

            DuplicateContacts = duplicates;

            return Page();
        }

        private List<Contact> GetAllContactsFromDatabase()
        {
            List<Contact> contacts = new List<Contact>();
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Contacts";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Contact contact = new Contact
                            {
                                ContactId = reader["Id"].ToString(),
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                Email = reader["Email"].ToString(),
                                Category = reader["Category"].ToString(),
                                Phone = reader["Phone"].ToString()
                            };

                            contacts.Add(contact);
                        }
                    }
                }
            }

            return contacts;
        }
    }
}

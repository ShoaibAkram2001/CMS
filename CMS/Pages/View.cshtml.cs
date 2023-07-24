using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using CMS.Pages;

namespace CMS.Pages
{
    public class ViewModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public ViewModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<ContactViewModel>? Contacts { get; set; }

        public void OnGet()
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT * FROM Contacts";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        Contacts = new List<ContactViewModel>();

                        while (reader.Read())
                        {
                            var contact = new ContactViewModel
                            {
                              
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                Email = reader["Email"].ToString(),
                                Phone = reader["Phone"].ToString()
                            };

                            Contacts.Add(contact);
                        }
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

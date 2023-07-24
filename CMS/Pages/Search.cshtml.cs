using System;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using CMS.Pages;
namespace CMS.Pages
{
    public class SearchModel : PageModel
    {
        private readonly IConfiguration _configuration;

        [BindProperty(SupportsGet = true)]
        public string FirstName { get; set; }

     
        [FromQuery] // Add [FromQuery] attribute here

        [BindProperty(SupportsGet = true)]
        public string Phone { get; set; }

        public SearchContactModel Contact { get; set; }

        public SearchModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult OnGet()
        {
            Console.WriteLine("Phone :" +Phone);

            if (!string.IsNullOrEmpty(Phone))
            {
                Console.WriteLine("PhoneCOnd");
                Contact = GetContactByPhoneNumber(Phone);
            }
            if (!string.IsNullOrEmpty(FirstName))
            {
                Contact = GetContactByFirstName(FirstName);
            }
           

            return Page();
        }

        

        private SearchContactModel GetContactByFirstName(string firstName)
        {
            SearchContactModel? contact = null;
            Console.WriteLine("SearchByFirstName");

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
                            contact = new SearchContactModel
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

        private SearchContactModel GetContactByPhoneNumber(string phoneNumber)
        {
            SearchContactModel? contact = null;
                Console.WriteLine("SearchByPhone");

            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT TOP 1 FirstName, LastName, Email, Phone FROM Contacts WHERE Phone = @PhoneNumber";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);

                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            contact = new SearchContactModel
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
    }
}

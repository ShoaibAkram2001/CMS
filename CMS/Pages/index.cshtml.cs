using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using Xunit;

namespace CMS.Pages;

public class IndexModel : PageModel
{
    private readonly IConfiguration _configuration;

//#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public IndexModel(IConfiguration configuration)
//#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        _configuration = configuration;
    }

    [BindProperty]
    public string? Username { get; set; }

    [BindProperty]
    public string? Password { get; set; }

    public string? ErrorMessage { get; set; }

    public void OnGet()
    {
        // This is the default GET handler for the Login page
    }

    public IActionResult OnPost()
    {
       
        if (!ModelState.IsValid)
        {
            Console.WriteLine("POST called");
            return Page();
        }

        try
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
              
                    connection.Open();
           
          
                string query = "SELECT COUNT(*) FROM Users WHERE Username = @Username AND Password = @Password";
                
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    
                    command.Parameters.AddWithValue("@Username", Username);
                  
                    command.Parameters.AddWithValue("@Password", Password);
                   
          
                    int count = (int)command.ExecuteScalar();
                  

                    if (count > 0)
                    {

                        Console.WriteLine("Login Successfull");

                        return RedirectToPage("/Dashboard");

                    }

                    else
                    {
                        Console.WriteLine("Login Error");
                        ErrorMessage = "Invalid username or password.";
                        return Page();
                    }
                }
            }
        }
        catch (Exception ex)
        {

            ErrorMessage = ex.Message;
           // Console.WriteLine(ErrorMessage);
            return Page();
        }
    }
}
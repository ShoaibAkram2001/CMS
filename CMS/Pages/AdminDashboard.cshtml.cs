using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CMS.Pages
{
    public class AdminDashboardModel : PageModel
    {
        public void OnGet()
        {
            if (HttpContext.Session.GetString("UserType") != "admin")
            {
                // Redirect to login if not authenticated as an admin
                Response.Redirect("/index");
            }


        }
    }
}

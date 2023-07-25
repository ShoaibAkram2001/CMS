using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CMS.Pages
{
    public class DashboardModel : PageModel
    {
        public void OnGet()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
            {
                // Redirect to login if not authenticated
                Response.Redirect("/index");
            }

        }
    }
}

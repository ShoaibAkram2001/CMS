using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CMS.Pages
{
    public class LogOutModel : PageModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LogOutModel(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void OnGet()
        {

            // Invalidate the session by clearing it
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            _httpContextAccessor.HttpContext.Session.Clear();
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            // Redirect the user to the login page
            Response.Redirect("/index");
        }

        
    }
}

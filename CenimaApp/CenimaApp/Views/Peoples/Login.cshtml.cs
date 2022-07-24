using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CenimaApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace CenimaApp.Pages.Peoples
{
    public class LoginModel : PageModel
    {
        private readonly CenimaApp.Models.CenimaDBContext _context;

        public LoginModel(CenimaApp.Models.CenimaDBContext context)
        {
            _context = context;
        }
        [BindProperty]
        public Person Person { get; set; }
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                var person = _context.Persons.SingleOrDefault(p => p.Email == Person.Email && p.Password == Person.Password);
                if (person != null)
                {
                    HttpContext.Session.SetString("account", JsonSerializer.Serialize(person));
                    if (person.Type==1)
                    {
                           return RedirectToPage("/Movies/Index");
                    }
                    else if (person.Type==2)
                    {
                        return RedirectToPage("/Index");
                    }
                }
                else
                {
                    return Page();
                }
            }
            return Page();
        }
    }
}

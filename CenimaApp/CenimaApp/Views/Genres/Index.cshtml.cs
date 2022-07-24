using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CenimaApp.Models;

namespace CenimaApp.Pages.Genres
{
    public class IndexModel : PageModel
    {
        private readonly CenimaApp.Models.CenimaDBContext _context;

        public IndexModel(CenimaApp.Models.CenimaDBContext context)
        {
            _context = context;
        }

        public IList<Genre> Genre { get;set; }

        public async Task OnGetAsync()
        {
            Genre = await _context.Genres.ToListAsync();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyScriptureJournal.Data;
using MyScriptureJournal.Models;

namespace MyScriptureJournal.Pages.Notes
{
    public class IndexModel : PageModel
    {
        private readonly MyScriptureJournal.Data.MyScriptureJournalContext _context;

        public IndexModel(MyScriptureJournal.Data.MyScriptureJournalContext context)
        {
            _context = context;
        }

        public IList<ScriptureNote> ScriptureNote { get;set; }
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
        public SelectList BookList { get; set; }
        [BindProperty(SupportsGet = true)]
        public string ScriptureNoteBook { get; set; }
        public SelectList SortOrderList { get; set; }
        [BindProperty(SupportsGet = true)]
        public string sortOrder { get; set; }

        


        public async Task OnGetAsync()
        {

            IQueryable<string> bookQuery = from b in _context.ScriptureNote
                orderby b.Book
                select b.Book;

            var notes = from n in _context.ScriptureNote
                select n;

            if (!string.IsNullOrEmpty(SearchString))
            {
                notes = notes.Where(keyword => keyword.NoteText.Contains(SearchString));
            }

            if (!string.IsNullOrEmpty(ScriptureNoteBook))
            {
                notes = notes.Where(x => x.Book == ScriptureNoteBook);
            }

            switch (sortOrder)
            {
                case "1":
                    notes = notes.OrderBy(s => s.Book);
                    break;
                case "2":
                    notes = notes.OrderByDescending(s => s.Book);
                    break;
                case "3":
                    notes = notes.OrderByDescending(s => s.EntryDateTime);
                    break;
                default:
                    notes = notes.OrderBy(s => s.EntryDateTime);
                    break;
            }

            BookList = new SelectList(await bookQuery.Distinct().ToListAsync());
            ScriptureNote = await notes.ToListAsync();
        }
    }
}

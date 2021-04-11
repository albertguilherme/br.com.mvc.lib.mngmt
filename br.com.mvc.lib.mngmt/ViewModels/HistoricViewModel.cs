using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using br.com.mvc.lib.mngmt.model;

namespace br.com.mvc.lib.mngmt.ViewModels
{
    public class HistoricViewModel : BaseViewModel
    {
        public string User { get; set; }
        public string Book { get; set; }
        public string Author { get; set; }
        public string BorrowDate { get; set; }
        public string ReturnDate { get; set; }
        public string ReturnedDate { get; set; }

        public HistoricViewModel ToViewModel(Historic h)
        {
            Id = h.Id;
            User = h.User.Name;
            Book = h.Book.Title;
            Author = string.Join(", ", h.Book.Author.Select(x => x.Name.Replace(".", "")));
            BorrowDate = h.BorrowDate.ToString("dd/MM/yyyy");
            ReturnDate = h.ReturnDate.ToString("dd/MM/yyyy");
            ReturnedDate = h.ReturnedDate != null ? h.ReturnedDate?.ToString("dd/MM/yyyy") : "PENDING";

            return this;
        }
    }


}

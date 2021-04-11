using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using br.com.mvc.lib.mngmt.model;

namespace br.com.mvc.lib.mngmt.ViewModels
{
    public class AuthorViewModel : BaseViewModel
    {
        public AuthorViewModel()
        {
            Books = new List<BookViewModel>();
        }

        [Required]
        [MinLength(5, ErrorMessage = "Name must be more than 5 caracters.")]
        public string Name { get; set; }
        public List<BookViewModel> Books { get; set; }

        public Author ToModel()
        {
            return new()
            {
                Id = Id,
                Name = Name,
                Books = Books.Select(x => x.ToModel()).ToList(),
            };
        }

        public AuthorViewModel ToViewModel(Author a, int dpt)
        {
            Id = a.Id;
            Name = a.Name;
            Books = dpt > 0 ? a.Books.Select(x => new BookViewModel().ToViewModel(x, --dpt)).ToList() : new List<BookViewModel>();
            return this;
        }
    }
}

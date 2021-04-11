using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using br.com.mvc.lib.mngmt.model;

namespace br.com.mvc.lib.mngmt.ViewModels
{
    public class BookViewModel : BaseViewModel
    {
        public BookViewModel()
        {
            Authors = new List<AuthorViewModel>();
        }

        [Required]
        [MinLength(5, ErrorMessage = "Title must be more then 5 caracters.")]
        public string Title { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be bigger than 1 unit.")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Only numbers are accepted.")]
        public int Quantity { get; set; }

        public List<AuthorViewModel> Authors { get; set; }

        public string Status { get; set; }

        public Book ToModel()
        {
            return new ()
            {
                Id = Id,
                Author = Authors.Select(x => x.ToModel()).ToList(),
                Title = Title,
            };
        }

        public BookViewModel ToViewModel(Book b, int dpt)
        {
            Id = b.Id;
            Title = b.Title;
            Authors = dpt > 0 ? b.Author.Select(x => new AuthorViewModel().ToViewModel(x, --dpt)).ToList() : new List<AuthorViewModel>();
            Status = b.Status.ToString("G");
            return this;
        }
    }
}
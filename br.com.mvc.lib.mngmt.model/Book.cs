using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace br.com.mvc.lib.mngmt.model
{
    public class Book : BaseModel
    {
        public Book()
        {
            Author = new HashSet<Author>();
        }
        public string Title { get; set; }
        public virtual ICollection<Author> Author { get; set; }
        public BookStatus Status { get; set; }
    }

    public enum BookStatus
    {
        AVALIABLE,
        NOT_AVALIABLE,
        BORROWED
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace br.com.mvc.lib.mngmt.model
{
    public class Author : BaseModel
    {
        public Author()
        {
            Books = new HashSet<Book>();
        }
        public string Name { get; set; }
        public virtual ICollection<Book> Books { get; set; }
    }
}

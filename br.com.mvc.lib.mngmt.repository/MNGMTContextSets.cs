using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using br.com.mvc.lib.mngmt.model;
using Microsoft.EntityFrameworkCore;

namespace br.com.mvc.lib.mngmt.repository
{
    public partial class MNGMTContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Historic> Historics { get; set; }
    }
}

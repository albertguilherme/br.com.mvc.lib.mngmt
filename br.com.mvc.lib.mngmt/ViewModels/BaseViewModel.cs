using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace br.com.mvc.lib.mngmt.ViewModels
{
    public abstract class BaseViewModel
    {
        public Guid Id { get; set; }
    }
}

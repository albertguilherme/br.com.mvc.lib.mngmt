using System;
using System.Linq;
using System.Threading.Tasks;
using br.com.mvc.lib.mngmt.repository;
using Microsoft.EntityFrameworkCore;

namespace br.com.mvc.lib.mngmt.bizrules
{
    public class User
    {
        private MNGMTContext _context;
        public User()
        {
            _context = new MNGMTContext();
        }

        public model.User UserUsernameMustBeUnique(Guid id, string username)
        {
            return _context.Users.FirstOrDefault(x => x.Id == id && x.Username == username);
        }

        public async Task<model.User> AuthenticateUser(string username, string password)
        {
            var u = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
            if (u == null)
                return null;

            return BCrypt.Net.BCrypt.Verify(password, u.Password) ? u : null;
        }
    }
}

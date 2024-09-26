using SimpleUserCrud.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SimpleUserCrud.Core.Interfaces
{
    public interface IUserRepository
    {
        public IEnumerable<User> GetAll();
        public User? GetById(Guid id);
        public void Delete(Guid id);
        public void Update(User user);
		public void Add(User user);
		IEnumerable<User> GetAllByCondition(Func<User, bool>match);

	}
}

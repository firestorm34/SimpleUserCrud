using SimpleUserCrud.Core.Interfaces;
using SimpleUserCrud.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleUserCrud.Core.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository) 
        {
           _userRepository = userRepository;
        }

        private UserOperationResult ValidateUser(User user)
        {
            
            if(user.Login ==null || user.FirstName== null || user.LastName == null)
            {
                return new UserOperationResult(false, new List<string>(){ "All fields must be filled with characters (excluding whitespaces)" });
            }
            if (user.Login.Trim() == "" || user.FirstName.Trim() == "" || user.LastName.Trim() == "")
            {
                return new UserOperationResult(false, new List<string>() { "All fields must be filled with characters (excluding whitespaces)" });
            }

            if (_userRepository.GetAllByCondition(u=> u.Id != user.Id && u.Login == user.Login).Any())
            {
				return new UserOperationResult(false, new List<string>() { "Login must be unique" });
			}

			return new UserOperationResult(true);
        }

		public IEnumerable<User> GetAll()
        {
        return _userRepository.GetAll(); 
        }
		public User? GetById(Guid id)
        {
            return _userRepository.GetById(id);
        }

        public UserOperationResult Add(User user) 
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "Error while updating: passed argument was null");
            }
            UserOperationResult validationResult = ValidateUser(user);
            if (!validationResult.isSuccess)
            {
                return validationResult;
            }
            else
            {
                _userRepository.Add(user);
                return new UserOperationResult(true);
            }
        }
		public void Delete(Guid id)
        {
            User? userFromRepo = _userRepository.GetById(id);
            if(userFromRepo == null)
            {
                throw new InvalidOperationException($"User with id: {id} wasn't found. ");
            }
            _userRepository.Delete(id); 
        }   
		public UserOperationResult Update(User user)
        {

            if (user == null)
			{
				throw new ArgumentNullException(nameof(user),"Error while updating: passed argument was null");
			}
            User? userToReplace = _userRepository.GetById(user.Id);
            if (userToReplace == null)
            {
                throw new InvalidOperationException($"User with id: {user.Id} wasn't found.");
            }
            UserOperationResult validationResult = ValidateUser(user);
			if (!validationResult.isSuccess)
			{
				return validationResult;
			}

			_userRepository.Update(user);
			return new UserOperationResult(true);

		}


	}
    public record UserOperationResult(bool isSuccess, List<string>? Errors = null);

}

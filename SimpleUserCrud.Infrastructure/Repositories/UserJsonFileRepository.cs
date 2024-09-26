using Newtonsoft.Json;
using SimpleUserCrud.Core.Interfaces;
using SimpleUserCrud.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleUserCrud.Data.Repositories
{
	public class UserJsonFileRepository : IUserRepository
	{
		private readonly string _filePath;
		private List<User> _users;
		private  List<User> Users{
			get {
				if (_users == null) {
					_users = ReadFromFile();
				}
				return _users;
			}
		}
		public UserJsonFileRepository(string filePath) {
			ValidateFilePath(filePath);
			_filePath = filePath;
			
		}

		public void Add(User user)
		{
			Users.Add(user);
			SaveToFile();
		}

		public void Delete(Guid id)
		{
			if (GetById(id) is User user)
			{
				Users.Remove(user);
			}
            SaveToFile();
		}

		public IEnumerable<User>? GetAll()
		{
			return Users;
		}

		public User? GetById(Guid id)
		{
			return Users.FirstOrDefault(user => user.Id == id);
		}

		public void Update(User user)
		{
			User userToReplace = Users.Find(u=>u.Id== user.Id);
			int index = Users.IndexOf(userToReplace);
			if (index>=0) {
                Users[index] = user;
			}
			SaveToFile();
		}

		private void ValidateFilePath(string filePath) {
			if (filePath == null)
			{
				throw new ArgumentNullException(null,"Provided file path was null. \n Please, check appsettings.json to ensure correct file path is entered.");
			}

			if (!filePath.EndsWith(".json"))
			{
				throw new FormatException("Json file path must ends with \".json\" ");
			}
		}


		private List<User> ReadFromFile()
		{
			string json = "";

			using (StreamReader stream = new StreamReader(_filePath))
			{
				json = stream.ReadToEnd();
			}

			List<User>? users = JsonConvert.DeserializeObject<List<User>>(json);
			if(users is null)
			{
				throw new InvalidOperationException("Deserialized users was null.");		
			}
            return users;

		}


		private void SaveToFile()
		{
			string json = JsonConvert.SerializeObject(Users);
			using (StreamWriter stream = new StreamWriter(_filePath,false))
			{
				stream.Write(json);
			}
		}

		public IEnumerable<User> GetAllByCondition(Func<User, bool> match)
		{
			return Users.Where(match);
		}
	}
}

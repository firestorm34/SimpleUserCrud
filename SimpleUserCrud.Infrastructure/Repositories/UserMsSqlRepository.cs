using Microsoft.Data.SqlClient;
using SimpleUserCrud.Core.Interfaces;
using SimpleUserCrud.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SimpleUserCrud.Data.Repositories
{
	public class UserMsSqlRepository : IUserRepository
	{
		private readonly string _connectionString;
		public UserMsSqlRepository(string connectionString)
		{
			_connectionString = connectionString;
		}

		public void Delete(Guid userId)
		{
			string deleteQuery = $"DELETE FROM Users WHERE Id ='{userId}'";
			ExecuteCommandNonQuery(deleteQuery);
		}

		public IEnumerable<User> GetAll()
		{
			string query = "SELECT * FROM Users";
			List<User> users = GetUsersByQuery(query);
			return users;
		}

		public User? GetById(Guid userId)
		{
			string query = $"SELECT Id, Login, FirstName, LastName FROM Users WHERE Id ='{userId}'";
			User? user = GetUsersByQuery(query).FirstOrDefault();
			return user;
		}

		public void Add(User user)
		{
			string addQuery = "INSERT INTO Users(Id, Login, FirstName, LastName) VALUES( " +
				$"'{user.Id}', '{user.Login}', '{user.FirstName}', '{user.LastName}')";

			ExecuteCommandNonQuery(addQuery);
		}
		public void Update(User user)
		{
			string updateQuery = "UPDATE Users Set " +
				$"Login = '{user.Login}', " +
				$"FirstName = '{user.FirstName}', " +
				$"LastName = '{user.LastName}' " +
				$"WHERE Id ='{user.Id}'";

			ExecuteCommandNonQuery(updateQuery);
		}

		public IEnumerable<User> GetAllByCondition(Func<User, bool> match)
		{
			var e = GetAll().Where(match);
			return e;
		}


		private List<User> GetUsersByQuery(string query)
		{
			List <User> users = new List<User>();
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open(); 
				SqlCommand command = new SqlCommand(query, connection);

				using (SqlDataReader reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						var user = new User
						{
							Id = reader.GetFieldValue<Guid>("Id"),
							FirstName = reader.GetFieldValue<string>("FirstName"),
							LastName = reader.GetFieldValue<string>("LastName"),
							Login = reader.GetFieldValue<string>("Login")
						};
						users.Add(user);
					}
				}

			}
			return users;
		}

		private void ExecuteCommandNonQuery(string command)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				
				SqlCommand sqlCommand = new SqlCommand(command, connection);
				sqlCommand.ExecuteNonQuery();

			}
		}


	}
}

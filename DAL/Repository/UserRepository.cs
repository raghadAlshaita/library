using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net;

namespace DAL.Repository
{
    public class UserRepository : IUserRepository
    {

        public async Task<int> AddUserAsync(User user)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            int userId = 0;
            using (connection)
            {
                try
                {
                    await connection.OpenAsync();
                    SqlCommand cmd = new SqlCommand("AddNewUser", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@username", user.UserName);
                    cmd.Parameters.AddWithValue("@password", user.Password);
                    using (var reder = await cmd.ExecuteReaderAsync())
                    {
                        await reder.ReadAsync();

                        if (reder != null && !reder.IsClosed)
                        {

                            userId = int.Parse(reder["userId"].ToString());

                        }
                    }
                    return userId;
                }
                catch (Exception ex)
                {
                    throw new ApplicationException($"connection error.", ex);

                }
                finally
                {
                    connection.Close();
                }
            }
        }
        public async Task<int> DeleteUserAsync(int id)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            using (connection)
            {
                try
                {
                    await connection.OpenAsync();
                    SqlCommand cmd = new SqlCommand("DeleteUser", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@userId", id);
                    var result = await cmd.ExecuteNonQueryAsync();
                    return result;

                }
                catch (Exception ex)
                {
                    throw new ApplicationException($"connection error.", ex);

                }
                finally
                {
                    connection.Close();
                }
            }
        }
        public async Task<IList<User>> GetAllUsersAsync()
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            IList<User> users = new List<User>();

            using (connection)
            {
                try
                {
                    await connection.OpenAsync();
                    SqlCommand cmd = new SqlCommand("GetAllUsers", connection);
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (var reder = await cmd.ExecuteReaderAsync())
                        {
                            while (await reder.ReadAsync())
                            {
                                users.Add(AddUserFromReader(reder));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new ApplicationException($"Procedure error.", ex);

                    }

                }
                catch (Exception ex)
                {
                    throw new ApplicationException($"connection error.", ex);

                }
                finally
                {

                    connection.Close();
                }
            }
            return users;
        }
        public async Task<IList<User>> ReadAsync(int page, int perPage, string Searchby, string search)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            IList<User> users = new List<User>();

            using (connection)
            {
                try
                {
                    await connection.OpenAsync();
                    SqlCommand cmd = new SqlCommand("GetAllUser", connection);
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (!string.IsNullOrEmpty(Searchby) && !string.IsNullOrEmpty(search))
                        {
                            cmd.Parameters.AddWithValue("@col", Searchby);
                            cmd.Parameters.AddWithValue("@searchTerm", search);

                        }
                        cmd.Parameters.AddWithValue("@PageNumber", page);
                        cmd.Parameters.AddWithValue("@RowsPerPage", perPage);

                        using (var reder = await cmd.ExecuteReaderAsync())
                        {
                            while (await reder.ReadAsync())
                            {
                                users.Add(AddUserFromReader(reder));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new ApplicationException($"Procedure error.", ex);

                    }
                }
                catch (Exception ex)
                {
                    throw new ApplicationException($"connection error.", ex);

                }
                finally
                {

                    connection.Close();
                }
            }
            return users;
        }
        public async Task<int> UpdateUserAsync(User user)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            using (connection)
            {
                try
                {
                    await connection.OpenAsync();
                    SqlCommand cmd = new SqlCommand("UpdateUserDetails", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@userId", user.Id);
                    cmd.Parameters.AddWithValue("@username", user.UserName);
                    cmd.Parameters.AddWithValue("@password", user.Password);

                    var result = await cmd.ExecuteNonQueryAsync();
                    return result;

                }
                catch (Exception ex)
                {
                    throw new ApplicationException($" error.", ex);

                }
                finally
                {
                    connection.Close();
                }
            }
        }
        public async Task<User> GetUserAsync(string parameterName, string StringParam = null, int? numberParam = null)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            using (connection)
            {
                User user = new User();
                try
                {
                    await connection.OpenAsync();
                    SqlCommand cmd = new SqlCommand($"GetUserBy{parameterName}", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (numberParam != null)
                        cmd.Parameters.AddWithValue("@param", numberParam);
                    else if (!string.IsNullOrEmpty(StringParam))
                        cmd.Parameters.AddWithValue("@param", StringParam);
                    using (var reder = await cmd.ExecuteReaderAsync())
                    {
                        await reder.ReadAsync();
                        if (reder != null && !reder.IsClosed)
                        {
                            user = AddUserFromReader(reder);

                        }

                    }
                    return user;
                }
                catch (Exception ex)
                {
                    throw new ApplicationException($"connection error.", ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
     
        public async Task<int> logIn(string userName, string password)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            using (connection)
            {
                try
                {
                    await connection.OpenAsync();
                    SqlCommand cmd = new SqlCommand($"CheckCredentials", connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
                    {
                        cmd.Parameters.AddWithValue("@username", userName);
                        cmd.Parameters.AddWithValue("@password", password);
                    }

                    using (var reder = await cmd.ExecuteReaderAsync())
                    {
                        while (await reder.ReadAsync())
                        {
                            if (reder != null && !reder.IsClosed)
                            {
                                return int.Parse(reder["Result"].ToString());
                            }
                        }
                     

                    }
                    return 0;

                }
                catch (Exception ex)
                {
                    throw new ApplicationException($"connection error.", ex);
                }
                //solve through conneaction pool

                finally
                { connection.Close(); }
            }

        }

        public async Task<bool> SetRoleForUser(int userId, bool isAdmin)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            using (connection)
            {
                try
                {
                    await connection.OpenAsync();
                    SqlCommand cmd = new SqlCommand($"SetRolesForUser", connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (userId != 0)
                    {
                        cmd.Parameters.AddWithValue("@userId", userId);
                        cmd.Parameters.AddWithValue("@adminRole", isAdmin);
                    }

                    using (var reder = await cmd.ExecuteReaderAsync())
                    {
                        await reder.ReadAsync();
                        if (reder != null && !reder.IsClosed)
                        {
                            return true;
                        }

                    }
                    return false;

                }
                catch (Exception ex)
                {
                    throw new ApplicationException($"connection error.", ex);
                }
                //solve through conneaction pool

                finally
                { connection.Close(); }
            }
        }

        public void SaveToken(Token token)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            using (connection)
            {
                try
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand($"SaveJWTToken", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@token", token.TokenValue);
                    cmd.Parameters.AddWithValue("@userId", token.UserId);
                    using (var reder =  cmd.ExecuteReader())
                    {
                         reder.Read();
                        if (reder != null && !reder.IsClosed)
                        {
                            return ;
                        }

                    }

                    return;

                }
                catch (Exception ex)
                {
                    throw new ApplicationException($"connection error.", ex);

                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private User AddUserFromReader(SqlDataReader reader)
        {
            if (reader != null && !reader.IsClosed)
            {
                return new User()
                {
                    Id = int.Parse(reader["Id"].ToString()),
                    UserName = reader["UserName"].ToString(),
                    Password = reader["Password"].ToString(),

                };

            }
            return null;
        }


    }

}

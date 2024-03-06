using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Runtime.Remoting.Messaging;
using System.Text.RegularExpressions;
using System.Net;

namespace DAL.Repository
{
    public class BookRepository : IBookRepository
    {

       
        public async Task<int> AddBookAsync(Book book)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            int bookId =0;
            using (connection)
            {
                try
                {
                    await connection.OpenAsync();
                    SqlCommand cmd = new SqlCommand($"AddNewBook", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@title", book.Title);
                    cmd.Parameters.AddWithValue("@author", book.Author);
                    cmd.Parameters.AddWithValue("@isbn", book.ISBN);

               
                    using (var reder = await cmd.ExecuteReaderAsync())
                    {
                        await reder.ReadAsync();

                        if (reder != null && !reder.IsClosed)
                        {
                          
                             bookId = int.Parse(reder["bookId"].ToString());

                        }
                    }

                    return bookId;

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
        public async Task<int> DeleteBookAsync(int id)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            using (connection)
            {
                try
                {
                    await connection.OpenAsync();
                    SqlCommand cmd = new SqlCommand("DeleteBook", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@bookId", id);
                    var result = await cmd.ExecuteNonQueryAsync();
                    if(result>0)
                    {
                        return   result;
                    }
                    else { throw new ApplicationException($"Error In Execution"); }

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
        public async Task<IList<Book>> GetAllBookAsync()
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            IList<Book> books = new List<Book>();

            using (connection)
            {
                try
                {
                    await connection.OpenAsync();
                    SqlCommand cmd = new SqlCommand("GetAllBooks", connection);
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                       
                        using (var reder = await cmd.ExecuteReaderAsync())
                        {
                                while (await reder.ReadAsync())
                            {
                                books.Add(AddBookFromReader(reder));
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
            return books;
        }
        public async Task<IList<Book>> ReadAsync(int page, int perPage,string Searchby, string search)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            IList<Book> books = new List<Book>();

            using (connection)
            {
                try
                {
                    await connection.OpenAsync();
                    SqlCommand cmd = new SqlCommand("GetAll", connection);
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
                                books.Add(AddBookFromReader(reder));
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
            return books;
        }
        public async Task<int> UpdateBookAsync(Book book)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            using (connection)
            {
                try
                {
                    await connection.OpenAsync();
                    SqlCommand cmd = new SqlCommand("UpdateBookDetails", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@bookId", book.Id);
                    cmd.Parameters.AddWithValue("@title", book.Title);
                    cmd.Parameters.AddWithValue("@author", book.Author);
                    cmd.Parameters.AddWithValue("@isbn", book.ISBN);
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
        public async Task<Book> GetBookAsync(string parameterName, int? numberParam = null, string StringParam = null)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            using (connection)
            {
                Book book = new Book();
                try
                {
                    await connection.OpenAsync();
                    SqlCommand cmd = new SqlCommand($"GetBookBy{parameterName}", connection);
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
                           book= AddBookFromReader(reder);

                        }

                    }
                    return book;
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
        private   Book AddBookFromReader(SqlDataReader reader   )
        {
           
            if (reader != null && !reader.IsClosed)
            {
                return new Book()
                {
                    Id = int.Parse(reader["Id"].ToString()),
                    Title = reader["Title"].ToString(),
                    Author = reader["Author"].ToString(),
                    ISBN = reader["ISBN"].ToString(),
                    Status = int.Parse(reader["Status"].ToString()),
                };
            }
            return null;
        }

        public async Task<int> BorrowAnBookAsync(int bookid, string username)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            int BorrowingBookid = 0;
            using (connection)
            {
              
                try
                {
                    await connection.OpenAsync();
                    SqlCommand cmd = new SqlCommand($"BorrowBook", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (bookid != 0 && !string.IsNullOrEmpty(username))
                    {
                        cmd.Parameters.AddWithValue("@bookId", bookid);
                        cmd.Parameters.AddWithValue("@username", username);

                    }

                    using (var reder = await cmd.ExecuteReaderAsync())
                    {
                        await reder.ReadAsync();

                        if (reder != null && !reder.IsClosed)
                        {

                            BorrowingBookid = int.Parse(reder["Id"].ToString());

                        }

                    }
                    return BorrowingBookid;
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

        public async Task<bool> ReturnBookAsync(int bookid)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            using (connection)
            {

                try
                {
                    await connection.OpenAsync();
                    SqlCommand cmd = new SqlCommand($"ReturnBook", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (bookid != 0  )
                    {
                        cmd.Parameters.AddWithValue("@bookId", bookid);
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
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}

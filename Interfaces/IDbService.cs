using Microsoft.EntityFrameworkCore;
using WebServer.Models;

namespace WebServer.Interfaces
{
    public interface IBaseDBService
    {

    }

    public interface IDBService
    {
        /// <summary>
        /// Adds the user to the database
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        void AddUser(string name, string password);


        /// <summary>
        /// Find user from DB by username and password 
        /// use it for authorization
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns>return user object </returns>
        User? GetUser(string username, string password);

        /// <summary>
        /// Delete user from row in Database
        /// </summary>
        /// <param name="name"></param>
        void DeleteUser(long id);

        /// <summary>
        /// Get user from database 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns>Return true if user exist otherwise false</returns>
        Task<bool> IsUserExist(string name, string password);

        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="name"></param>
        /// <returns>return User object</returns>
        Task<User>? GetUserById(long id);

        /// <summary>
        /// Get user from the database 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns>Return true if user exist otherwise false</returns>
        Task<bool> IsUserExist(string name);

        /// <summary>
        /// Get all tests from the database 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns> Return a test array if exists otherwise null </returns>
        Task<List<Test>?> GetTests();

        /// <summary>
        /// Get all tests from the database 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns> Return a test array if exists otherwise null</returns>
        Task<List<Test>?> GetTests(string author);

        /// <summary>
        /// Get one test from the database by id
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns> Return a test if exists otherwise null</returns>
        Task<Test?> GetTest(long id);

        /// <summary>
        /// Add test to the database
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        void AddTest(TestRequest testRequest);

        /// <summary>
        /// Add comment to the database
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        void AddComment(CommentRequest commentRequest);

        /// <summary>
        /// Get all the test comments 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        Task<Comment[]> GetComments(long testId);
    }
}

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
        /// <returns>User object</returns>
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
        /// <returns>Return array of tests</returns>
        Task<Test[]> GetTests();

        /// <summary>
        /// Get all tests from the database 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns>Return array of tests</returns>
        Task<Test[]> GetTests(string author);

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

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Isopoh.Cryptography.Argon2;
using System.Text.Json;

using WebServer.Interfaces;
using WebServer.Models;

namespace WebServer.Services
{

    public class BaseDBService : IBaseDBService
    {
        private AppDbContext _DbContext { get; set; }

        public BaseDBService(AppDbContext dbContext) => _DbContext = dbContext;
    }

    public class DbService : BaseDBService, IDBService
    {
        private AppDbContext _DbContext { get; set; }

        public DbService(AppDbContext dbContext) : base(dbContext) => _DbContext = dbContext;

        public async void AddUser(string username, string password)
        {
            long id = 0;
            try
            {
                id = await _DbContext.profiles.CountAsync() + 1;
            }
            catch
            {
                id = 1;
            }

            
            User user = new User { Id = id, Username = username, Password = Argon2.Hash(password), Role = Role.User };
            await _DbContext.profiles.AddAsync(user);
            await _DbContext.SaveChangesAsync();
        }

        public User? GetUser(string username, string password)
        {

            var user = _DbContext.profiles.FirstOrDefault(u => u.Username == username);

            bool isPasswordValid = Argon2.Verify(user.Password, password);

            if (isPasswordValid)
                return user;

            return null;
        }

        public async void DeleteUser(long id)
        {
            await Task.Run(() =>
            _DbContext.profiles.Where(p => p.Id == id).ExecuteDeleteAsync());
            _DbContext.SaveChanges();
        }

        public async Task<User>? GetUserById(long id)
        {
            User? user = await _DbContext.profiles.FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task<bool> IsUserExist(string username)
        {
            User? user = await _DbContext.profiles.FirstOrDefaultAsync(u => u.Username == username);
            if (user is null) return false;
            return true;
        }

        public async Task<bool> IsUserExist(string username, string password)
        {
            var user = _DbContext.profiles.FirstOrDefault(u => u.Username == username);

            bool isPasswordValid = Argon2.Verify(user.Password, password);

            if (user is null) return false;
            return true;
        }

        public async void AddTest(TestRequest testRequest)
        {
            long id = 0;

            try
            {
                id = await _DbContext.Tests.CountAsync() + 1;
            }
            catch
            {
                id = 1;
            }
            
            Test test = new Test { Id = id, Name = testRequest.Name, Author = testRequest.Author, Questions = testRequest.Questions};
            await _DbContext.Answer.AddRangeAsync(testRequest.Questions.SelectMany(a => a.Answers));
            await _DbContext.Tests.AddAsync(test);
            await _DbContext.SaveChangesAsync();
        }

        public async Task<List<Test>?> GetTests()
        {
            var tests = await _DbContext.Tests.Include(t => t.Questions).ThenInclude(a => a.Answers).ToListAsync();
            if (tests.Count() == 0) return null;

            return tests;
        }

        public async Task<Test?> GetTest(long id)
        {
            Test? test = await _DbContext.Tests.Include(t => t.Questions).ThenInclude(a => a.Answers).FirstOrDefaultAsync(t => t.Id == id);

            if (test is null) return null;

            return test;
        }

        public async Task<List<Test>?> GetTests(string author)
        {
            var tests = await _DbContext.Tests.Include(t => t.Questions).ThenInclude(a => a.Answers).Select(t => t).Where(t => t.Author == author).ToListAsync();

            if (tests.Count() == 0) return null;

            return tests;
        }

        public async Task<Comment[]> GetComments(long id)
        {
            var comment = await _DbContext.Comments.Select(t => t).Where(t => t.TestId == id).ToArrayAsync();

            return comment;
        }

        public async void AddComment(CommentRequest commentRequest)
        {
            long id = 0;

            try
            {
                id = await _DbContext.Tests.CountAsync() + 1;
            }
            catch
            {
                id = 1;
            }

            Comment comment = new Comment { Id = id, Author = commentRequest.Author, TestId = commentRequest.TestId, TestComment = commentRequest.TestComment };
            await _DbContext.Comments.AddAsync(comment);
            await _DbContext.SaveChangesAsync();
        }

        public async void AddAnswer(Question question) {
            long id = 0;

            try
            {
                id = await _DbContext.Tests.CountAsync() + 1;
            }
            catch
            {
                id = 1;
            }

            Answer answer = new Answer { Id = id, Body = "", IsTrueAnswer = false};
            await _DbContext.Answer.AddAsync(answer);
            await _DbContext.SaveChangesAsync();
        }
        public async void AddQuestion() {


        }
    }
}


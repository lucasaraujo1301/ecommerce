using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using ECommerce.AuthService.Domain.Entities;
using ECommerce.AuthService.Infrastructure.Repositories;
using ECommerce.AuthService.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;


namespace ECommerce.TestAuthService.Repositories
{
    public class TestsUserRepository
    {
        private ECommerceAuthServiceDbContext _dbContext;
        private UserRepository _repository;
        private Mock<IPasswordHasher<User>> _passwordHasherMock;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ECommerceAuthServiceDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDB") // Use in-memory database
                .Options;

            _dbContext = new ECommerceAuthServiceDbContext(options);

            _passwordHasherMock = new Mock<IPasswordHasher<User>>();

            _repository = new UserRepository(_dbContext, _passwordHasherMock.Object);

            _passwordHasherMock.Setup(
                x => x.HashPassword(It.IsAny<User>(), It.IsAny<string>())
            ).Returns(
                (User user, string password) => $"HASHED_{password}"
            );
        }

        private async Task<User> CreateUser()
        {
            User user = new()
            {
                Email = "test@test.com",
                FirstName = "Unit",
                LastName = "Test",
                Password = "abc123456"
            };

            await _repository.CreateAsync(user);

            return user;
        }

        [TearDown]
        public void Cleanup()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Test]
        public void TestCreateUser_Unsuccessfully_EmailWrongFormat()
        {
            User user = new()
            {
                Email = "test.com",
                FirstName = "Unit",
                LastName = "Test",
                Password = "abc123456"
            };

            var exception = Assert.ThrowsAsync<ArgumentException>(
                async () => await _repository.CreateAsync(user)
            );

            Assert.That(exception.Message, Is.EqualTo("Email in wrong format."));
        }

        [Test]
        public async Task TestCreateUser_Success()
        {
            User newUser = new()
            {
                Email = "test@TesTing.COM",
                FirstName = "Unit",
                LastName = "Test",
                Password = "abc123456"
            };

            await _repository.CreateAsync(newUser);

            Assert.That(newUser.UserId, Is.Positive);
            Assert.That(newUser.Email, Is.EqualTo("test@testing.com"));
        }

        [Test]
        public async Task TestGetUserByEmail_Success()
        {
            await CreateUser();

            User? user = await _repository.GetUserByEmailAsync("test@test.com");

            Assert.That(user, Is.Not.Null);
            Assert.That(user.Email, Is.EqualTo("test@test.com"));
        }

        [Test]
        public async Task TestGetUserByEmail_Unsuccess()
        {
            User? user = await _repository.GetUserByEmailAsync("test@test.com");

            Assert.That(user, Is.Null);
        }
    }
}
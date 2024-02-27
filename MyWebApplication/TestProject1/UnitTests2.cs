using DataBaseUsers.BD;
using DataBaseUsers.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using DataBaseMessage.BD;
using DataBaseMessage.Repository;
using System.Configuration;

using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using UserService.AuthorizationModel;
using UserService.Controllers;

namespace TestProject1
{
    internal class UnitTests2
    {
        private LoginController _controller;
        private Mock<IUserRepository> _userRepositoryMock;

        [SetUp]
        public void Setup()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _controller = new LoginController(null, _userRepositoryMock.Object);
        }

        [Test]
        public void AddAdmin_ok()
        {
            // Arrange
            var userLogin = new LoginModel { Name = "admin", Password = "password" };

            // Act
            var result = _controller.AddAdmin(userLogin) as OkResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public void AddAdmin_sameName_error()
        {
            // Arrange
            var userLogin = new LoginModel { Name = "admin", Password = "password" };
            _userRepositoryMock.Setup(repo => repo.UserAdd(userLogin.Name, userLogin.Password, RoleId.Admin))
                .Throws(new Exception("Some error message"));

            // Act
            var result = _controller.AddAdmin(userLogin) as StatusCodeResult;

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void Login_Ok()
        {
            // Arrange
            var userLogin = new LoginModel { Name = "user7", Password = "super7" };
            var roleId = RoleId.User;
            _userRepositoryMock.Setup(repo => repo.UserCheck(userLogin.Name, userLogin.Password)).Returns(roleId);

            // Act
            var result = _controller.Login(userLogin) as OkObjectResult;

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void Login_arror()
        {
            // Arrange
            var userLogin = new LoginModel { Name = "user7", Password = "super7" };
            _userRepositoryMock.Setup(repo => repo.UserCheck(userLogin.Name, userLogin.Password))
                .Throws(new Exception("Some error message"));

            // Act
            var result = _controller.Login(userLogin) as StatusCodeResult;

            // Assert
            Assert.IsNull(result);
        }


        [Test]
        public void AddUser_Ok()
        {
            // Arrange
            var userLogin = new LoginModel { Name = "user10", Password = "super10" };

            // Act
            var result = _controller.AddUser(userLogin) as OkResult;

            // Assert
            Assert.IsNotNull(result);
            //Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public void AddUser_error()
        {
            // Arrange
            var userLogin = new LoginModel { Name = "user", Password = "pppuper" };
            _userRepositoryMock.Setup(repo => repo.UserAdd(userLogin.Name, userLogin.Password, RoleId.User))
                .Throws(new Exception("Some error message"));

            // Act
            var result = _controller.AddUser(userLogin) as StatusCodeResult;

            // Assert
            Assert.IsNull(result);
        }



       
    }
}

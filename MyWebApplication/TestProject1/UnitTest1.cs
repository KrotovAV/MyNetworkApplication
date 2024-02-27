using DataBaseMessage.Repository;
using DataBaseUsers.Repository;
using MessageService.Controllers;
using Microsoft.AspNetCore.Mvc;
using UserService.AuthorizationModel;
using UserService.Controllers;
using Moq;

namespace TestProject1
{
    public class UnitTest1
    {
        private MessageController _controllerMess;
        private Mock<IMessageRepository> _mockMessageRepository;
        
        private LoginController _controllerUs;
        private Mock<IUserRepository> _userRepositoryMock;

        [SetUp]
        public void Setup()
        {
            _mockMessageRepository = new Mock<IMessageRepository>();
            _controllerMess = new MessageController(null, _mockMessageRepository.Object);

            _userRepositoryMock = new Mock<IUserRepository>();
            _controllerUs = new LoginController(null, _userRepositoryMock.Object);
        }


        


        [Test]
        public void SendMessageMethod_Ok_Ok()
        {
            // Arrange
            var repository = new MessagRepository();

            // Act
            var id = repository.SendMessage("test", "user1", "user2");

            // Assert
            Assert.AreNotEqual(id, Guid.Empty);
        }


        [Test]
        public void GetAllMessages_Ok()
        {
            // Arrange
            var repository = new MessagRepository();

            // Act
            var messages = repository.GetAllMessages("user");

            // Assert
            Assert.IsNotNull(messages);
        }


        [Test]
        public void GetAllMessages_NoMessages_NoUser()
        {
            // Arrange
            var repository = new MessagRepository();

            // Act
            var messages = repository.GetAllMessages("userXXX");

            // Assert
            Assert.IsNotNull(messages);
        }

        [Test]
        public void AddUser_Ok()
        {
            // Arrange
            var userLogin = new LoginModel { Name = "user10", Password = "super10" };

            // Act
            var result = _controllerUs.AddUser(userLogin) as OkResult;

            // Assert
            Assert.IsNotNull(result);
            //Assert.AreEqual(200, result.StatusCode);
        }
    }
}
using ConsoleApp06S;
using ConsoleApp06S.Abstracts;
using ConsoleApp06S.Services;
using System;
using System.Net;

namespace ConsoleApp06STest
{

    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            using (var ctx = new ChatContext())
            {
                ctx.Messages.RemoveRange(ctx.Messages);
                ctx.Users.RemoveRange(ctx.Users);
                ctx.SaveChanges();
            }
        }
        //[TearDown]
        //public void TearDown()
        //{
        //    using (var ctx = new ChatContext())
        //    {
        //        ctx.Messages.RemoveRange(ctx.Messages);
        //        ctx.Users.RemoveRange(ctx.Users);
        //        ctx.SaveChanges();
        //    }
        //}

        //---------------------------------------------------------
        [Test]
        public async Task Test0()
        {
            var mock = new MockMessageSource();
            var srv = new Server(mock);
            mock.AddServer(srv);
            await srv.Start();

            using (var сtx = new ChatContext())
            {
                //Assert.IsTrue(сtx.Users.Count() == 2, "Пользователи не созданы");

                var user1 = сtx.Users.FirstOrDefault(x => x.FullName == "Вася");
                var user2 = сtx.Users.FirstOrDefault(x => x.FullName == "Юля");

                //Assert.IsNotNull(user1, "Пользователи не созданы");
                //Assert.IsNotNull(user2, "Пользователи не созданы");

                //Assert.IsTrue(user1.MessagesFrom.Count() == 1);
                //Assert.IsTrue(user2.MessagesFrom.Count() == 1);

                Assert.IsTrue(user1.MessagesTo.Count == 0, $"{user1.MessagesTo.Count}");
                Assert.IsTrue(user2.MessagesTo.Count == 0, $"{user2.MessagesTo.Count}");

                //var msg1 = сtx.Messages.FirstOrDefault(x => x.UserFrom == user1 && x.UserTo == user2);
                //var msg2 = сtx.Messages.FirstOrDefault(x => x.UserFrom == user2 && x.UserTo == user1);

                //Assert.AreEqual("Привет, Василий", msg2.Text);
                //Assert.AreEqual("Привет, Юлька!!!", msg1.Text);
            }

        }
        //-------------------------------------------------------------------
        [Test]
        public async Task Register2UsersInBDTest()
        {
            var mock = new MockMessageSource();
            var srv = new Server(mock);
            mock.AddServer(srv);
            await srv.Start();

            using (var сtx = new ChatContext())
            {
                
                Assert.IsTrue(сtx.Users.Count() == 2, "Пользователи не созданы");
            }
        }

        [Test]
        public async Task CorrectRegisterFullNameOfUser1InBDTest()
        {
            var mock = new MockMessageSource();
            var srv = new Server(mock);
            mock.AddServer(srv);
            await srv.Start();

            using (var сtx = new ChatContext())
            {
                var user1 = сtx.Users.FirstOrDefault(x => x.FullName == "Вася");

                Assert.IsNotNull(user1, "Пользователи не созданы");
            }
        }
        [Test]
        public async Task CorrectRegisterFullNameOfUser2InBDTest()
        {
            var mock = new MockMessageSource();
            var srv = new Server(mock);
            mock.AddServer(srv);
            await srv.Start();

            using (var сtx = new ChatContext())
            {
                var user2 = сtx.Users.FirstOrDefault(x => x.FullName == "Юля");

                Assert.IsNotNull(user2, "Пользователи не созданы");
            }
        }

        [Test]
        public async Task Test1()
        {
            var mock = new MockMessageSource();
            var srv = new Server(mock);
            mock.AddServer(srv);
            await srv.Start();

            using (var сtx = new ChatContext())
            {
                //var user1 = сtx.Users.FirstOrDefault(x => x.FullName == "Вася");
                var user2 = сtx.Users.FirstOrDefault(x => x.FullName == "Юля");

                //Console.WriteLine(user1.MessagesFrom.Count());

               // Assert.IsTrue(user1.MessagesFrom.Count() != 0);
                Assert.IsTrue(user2.MessagesFrom.Count() == 0);
            }
        }


        [Test]
        public async Task CorrectTextOfMessageFromUser1ToUser2InBDTest()
        {
            var mock = new MockMessageSource();
            var srv = new Server(mock);
            mock.AddServer(srv);
            await srv.Start();

            using (var сtx = new ChatContext())
            {
                var user1 = сtx.Users.FirstOrDefault(x => x.FullName == "Вася");
                var user2 = сtx.Users.FirstOrDefault(x => x.FullName == "Юля");

                var msg1 = сtx.Messages.FirstOrDefault(x => x.UserFrom == user1 && x.UserTo == user2);
                var msg2 = сtx.Messages.FirstOrDefault(x => x.UserFrom == user2 && x.UserTo == user1);

                Assert.AreEqual("Привет, Юлька!!!", msg1.Text);
            }
        }

        [Test]
        public async Task CorrectTextOfMessageFromUser2ToUser1InBDTest()
        {
            var mock = new MockMessageSource();
            var srv = new Server(mock);
            mock.AddServer(srv);
            await srv.Start();

            using (var сtx = new ChatContext())
            {
                var user1 = сtx.Users.FirstOrDefault(x => x.FullName == "Вася");
                var user2 = сtx.Users.FirstOrDefault(x => x.FullName == "Юля");

                var msg1 = сtx.Messages.FirstOrDefault(x => x.UserFrom == user1 && x.UserTo == user2);
                var msg2 = сtx.Messages.FirstOrDefault(x => x.UserFrom == user2 && x.UserTo == user1);

                Assert.AreEqual("Привет, Василий", msg2.Text);
            }
        }
        //[Test]
        //public async Task ClientListener_ReceivesMessage_CallsConfirm()
        //{
        //    var name = "TestClient";
        //    var address = "127.0.0.1";
        //    var port = 1234;

        //    var messageSourceMock = new Mock<IMessageSource>();
        //    var udpClientMock = new Mock<UdpClient>();

        //    var client = new Client(name, address, port)
        //    {
        //        _messageSouce = messageSourceMock.Object,
        //        udpClientClient = udpClientMock.Object
        //    };

        //    var remoteEndPoint = new IPEndPoint(IPAddress.Any, 5678);
        //    var netMessage = new NetMessage { NickNameFrom = "Sender", Text = "Hello", Command = Command.Message };

        //    messageSourceMock.Setup(m => m.Receive(ref remoteEndPoint)).Returns(netMessage);
        //    messageSourceMock.Setup(m => m.SendAsync(It.IsAny<NetMessage>(), It.IsAny<IPEndPoint>())).Returns(Task.CompletedTask);

        //    await client.ClientListener();

        //    messageSourceMock.Verify(m => m.Receive(ref remoteEndPoint), Times.Once);
        //    messageSourceMock.Verify(m => m.SendAsync(It.IsAny<NetMessage>(), remoteEndPoint), Times.Once);
        //}

        //[Test]
        //public void Register_CallsMessageSourceSendAsync()
        //{

        //    var name = "TestClient";
        //    var address = "127.0.0.1";
        //    var port = 1234;

        //    var messageSourceMock = new Mock<IMessageSource>();
        //    var udpClientMock = new Mock<UdpClient>();

        //    var client = new Client(name, address, port)
        //    {
        //        _messageSouce = messageSourceMock.Object,
        //        udpClientClient = udpClientMock.Object
        //    };

        //    var remoteEndPoint = new IPEndPoint(IPAddress.Any, 5678);


        //    client.Register(remoteEndPoint);

        //    messageSourceMock.Verify(m => m.SendAsync(It.IsAny<NetMessage>(), remoteEndPoint), Times.Once);
        //}


        //[TestMethod]
        //public void TestMessageMethod()
        //{
        //    var messageSource = new Mock<IMessageSource>();
        //    var server = new Server(messageSource.Object);
        //    server.Clients.Add("User1", new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1234));
        //    var testMessage = new NetMessage { NickNameFrom = "User2", NickNameTo = "User1", Text = "Test Message" };
        //    server.RelyMessage(testMessage).Wait();
        //    messageSource.Verify(mock => mock.SendAsync(testMessage, It.IsAny<IPEndPoint>()), Times.Once);
        //}
    }
}

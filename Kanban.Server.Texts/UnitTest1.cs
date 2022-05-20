using Kanban.Server.DAL;
using Kanban.Server.SocketsManager;
using NUnit.Framework;
using System.Net.WebSockets;

namespace Kanban.Server.Texts
{
    public class Tests
    {
        public ConnectionManager? connectionManager;

        [SetUp]
        public void Setup()
        {
            connectionManager = new ConnectionManager();
        }

        [Test]
        public void OnConnectedToServer_Connected_OneConected()
        {
            WebSocket ws = new ClientWebSocket();
            connectionManager?.AddSocket(ws);
            Assert.That(connectionManager?.GetAllConnections().Count == 1);
        }

        [Test]
        public void OnConnectedToDataBase_GetAllUsers_Geted()
        {
            Assert.That(DatabaseRepository.GetAllUsers().Count >= 1);
        }

        [Test]
        public void OnConnectedToDataBase_GetAllCards_Geted()
        {
            Assert.That(DatabaseRepository.GetAllCards().Count >= 1);
        }

        [Test]
        public void OnConnectedToDataBase_GetAllBoards_Geted()
        {
            Assert.That(DatabaseRepository.GetAllBoards().Count >= 1);
        }

        [Test]
        public void OnConnectedToDataBase_GetAllColumns_Geted()
        {
            Assert.That(DatabaseRepository.GetAllColumns().Count >= 1);
        }
    }
}
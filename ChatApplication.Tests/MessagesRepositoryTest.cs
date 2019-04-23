#region License
// Разработано: Коротенко Владимиром Николаевичем (Vladimir N. Korotenko)
// email: koroten@ya.ru
// skype:vladimir-korotenko 
// https://vkorotenko.ru
// Создано:  20.04.2019 12:22
#endregion

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using ChatApplication.Dbl;
using ChatApplication.Dbl.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;

namespace ChatApplication.Tests
{
    [TestClass]
    public class MessagesRepositoryTest
    {
        private const string _cs = Cost.Connection;
        /// <summary>
        /// test id
        /// </summary>
        private const int _creatorId = 52;

        private const long _topicId = 42;
        private const long _messageId = 77;

        [TestMethod]
        public async Task GetAllTest()
        {

            var ctx = new DbContext(_cs);
            var items = await ctx.Messages.GetAll();
            Assert.AreEqual(0, items.Capacity);
        }
        [TestMethod]
        public async Task InsertTest()
        {

            var ctx = new DbContext(_cs);
            var item = new DbMessage()
            {
                Id = _messageId,
                Body = "Body test",                
                AuthorId = _creatorId,                               
                Created = new DateTime(2019, 1, 1, 22, 23, 00),
                IsRead = false,
                TopicId = _topicId
            };
            DbMessage message;
            try
            {
                message = await ctx.Messages.Create(item);
                Assert.AreEqual(item.Id, message.Id);
            }
            catch (MySqlException ex)
            {
                Debug.WriteLine(ex);
                await ctx.Messages.Delete(_messageId);
                message = await ctx.Messages.Create(item);
                Assert.AreEqual(item.Id, message.Id);
            }


        }
        [TestMethod]
        public async Task SelectTest()
        {
            var ctx = new DbContext(_cs);
            var result = await ctx.Messages.Get(_messageId);
            if (result == null) await InsertTest();            
            Assert.AreEqual(result.Id, _messageId);
        }
        [TestMethod]
        public async Task DeleteTest()
        {
            var ctx = new DbContext(_cs);
            var exist = await ctx.Messages.Get(_messageId);
            if (exist == null)
                await InsertTest();
            await ctx.Messages.Delete(_messageId);
            exist = await ctx.Messages.Get(_messageId);
            Assert.AreEqual(exist, null);
        }

        [TestMethod]
        public async Task UpdateTest()
        {
            var newname = "petr";
            var ctx = new DbContext(_cs);
            var item = await ctx.Messages.Get(_messageId);
            if (item == null) await InsertTest();
            item = await ctx.Messages.Get(_messageId);
            item.Body = newname;
            await ctx.Messages.Update(item);

            item = await ctx.Messages.Get(_messageId);

            Assert.AreEqual(item.Body, newname);
        }
    }
}

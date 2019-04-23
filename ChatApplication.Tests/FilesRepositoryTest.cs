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
    public class FilesRepositoryTest
    {
        private const string _cs = Cost.Connection;
        /// <summary>
        /// test id
        /// </summary>
        private const int _creatorId = 52;

        private const long _topicId = 42;
        private const long _messageId = 77;
        private const long _fileId = 99;
        [TestMethod]
        public async Task GetAllTest()
        {

            var ctx = new DbContext(_cs);
            var items = await ctx.Files.GetAll();
            Assert.AreEqual(1, items.Capacity);
        }
        [TestMethod]
        public async Task InsertTest()
        {

            var ctx = new DbContext(_cs);
            var item = new DbFile()
            {
                Id = _fileId,
                Name = "filetest.txt",
                AuthorId = _creatorId,
                Created = new DateTime(2019, 1, 1, 22, 23, 00),
                MessageId = _messageId                
                
            };
            DbFile message;
            try
            {
                message = await ctx.Files.Create(item);
                Assert.AreEqual(item.Id, message.Id);
            }
            catch (MySqlException ex)
            {
                Debug.WriteLine(ex);
                await ctx.Files.Delete(_fileId);
                message = await ctx.Files.Create(item);
                Assert.AreEqual(item.Id, message.Id);
            }


        }
        [TestMethod]
        public async Task SelectTest()
        {
            var ctx = new DbContext(_cs);
            var result = await ctx.Files.Get(_fileId);
            if (result == null) await InsertTest();
            Assert.AreEqual(result.Id, _fileId);
        }
        [TestMethod]
        public async Task DeleteTest()
        {
            var ctx = new DbContext(_cs);
            var exist = await ctx.Files.Get(_fileId);
            if (exist == null)
                await InsertTest();
            await ctx.Files.Delete(_fileId);
            exist = await ctx.Files.Get(_fileId);
            Assert.AreEqual(exist, null);
        }

        [TestMethod]
        public async Task UpdateTest()
        {
            var newname = "petr.txt";
            var ctx = new DbContext(_cs);
            var item = await ctx.Files.Get(_fileId);
            if (item == null) await InsertTest();
            item = await ctx.Files.Get(_fileId);
            item.Name = newname;
            await ctx.Files.Update(item);

            item = await ctx.Files.Get(_fileId);

            Assert.AreEqual(item.Name, newname);
        }
    }
}

#region License
// Разработано: Коротенко Владимиром Николаевичем (Vladimir N. Korotenko)
// email: koroten@ya.ru
// skype:vladimir-korotenko 
// https://vkorotenko.ru
// Создано:  20.04.2019 9:56
#endregion
using ChatApplication.Dbl;
using ChatApplication.Dbl.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using System;
using System.Threading.Tasks;

namespace ChatApplication.Tests
{
    [TestClass]
    public class TopicsRepositoryTest
    {
        const string _cs = "server=localhost;UserId=chatapp;Password=ffgklmmmk56dfg;database=chatapp;";
        /// <summary>
        /// test id
        /// </summary>
        private const long _creatorId = 9103421220;

        private const long _id = 42;

        [TestMethod]
        public async Task GetAllTest()
        {

            var ctx = new DbContext(_cs);
            var users = await ctx.Topics.GetAll();
            Assert.AreEqual(true, users.Count > 0);
        }
        [TestMethod]
        public async Task InsertTest()
        {

            var ctx = new DbContext(_cs);
            var item = new DbTopic()
            {
                Id = _id,
                Title = "test",
                AnnouncementId = _id,
                AuthorId = _creatorId,
                Vendor = "TestVendor",
                VendorCode = "TestVendorCode",
                Created = new DateTime(2019, 1, 1, 22, 23, 00)
            };
            DbTopic ins;
            try
            {
                ins = await ctx.Topics.Create(item);
                Assert.AreEqual(item.Id, ins.Id);
            }
            catch (MySqlException ex)
            {
                await ctx.Topics.Delete(_id);
                ins = await ctx.Topics.Create(item);
                Assert.AreEqual(item.Id, ins.Id);
            }


        }
        [TestMethod]
        public async Task SelectTest()
        {
            var ctx = new DbContext(_cs);
            var result = await ctx.Topics.Get(_id);
            Assert.AreEqual(result.Id, _id);
        }
        [TestMethod]
        public async Task DeleteTest()
        {
            var ctx = new DbContext(_cs);
            var exist = await ctx.Topics.Get(_id);
            if (exist == null)
                await InsertTest();
            await ctx.Topics.Delete(_id);
            exist = await ctx.Topics.Get(_id);

            Assert.AreEqual(exist, null);
        }

        [TestMethod]
        public async Task UpdateTest()
        {
            var newname = "petr";
            var ctx = new DbContext(_cs);
            var item = await ctx.Topics.Get(_id);
            if (item == null) await InsertTest();
            item = await ctx.Topics.Get(_id);
            item.Title = newname;
            await ctx.Topics.Update(item);

            item = await ctx.Topics.Get(_id);

            Assert.AreEqual(item.Title, newname);
        }
    }
}

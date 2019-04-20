using ChatApplication.Dbl;
using ChatApplication.Dbl.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApplication.Tests
{
    [TestClass]
    public class UserRepositoryTests
    {
        const string _cs = "server=localhost;UserId=chatapp;Password=ffgklmmmk56dfg;database=chatapp;";
        /// <summary>
        /// test id
        /// </summary>
        private const long _id = 9103421220;
        [TestMethod]
        public async Task GetUsersTest()
        {

            var ctx = new DbContext(_cs);
            var users = await ctx.Users.GetUsers();
            var user = users.FirstOrDefault(x => x.Id.ToString() == "79103421225");
            Assert.AreNotEqual(null, user);
            Assert.AreEqual(4, users.Capacity);
        }
        [TestMethod]
        public async Task InsertTest()
        {

            var ctx = new DbContext(_cs);
            var dbuser = new DbUser
            {
                LastName = "Ivanov",
                Name = "Ivan",
                MiddleName = "Ivanovitch",
                Id = _id,
                Email = "ivanov@test.com",
                Password = "123",
                LastActive = new DateTime(2019, 1, 1, 22, 23, 00)
            };
            DbUser ins;
            try
            {
                ins = await ctx.Users.Create(dbuser);
                Assert.AreEqual(dbuser.Id, ins.Id);
            }
            catch (MySqlException ex)
            {
                await ctx.Users.Delete(_id);
                ins = await ctx.Users.Create(dbuser);
                Assert.AreEqual(dbuser.Id, ins.Id);
            }


        }
        [TestMethod]
        public async Task SelectTest()
        {
            var id = 79103421225;
            var name = "Vladimir";
            var mname = "Nikolaevitch";
            var lname = "Korotenko";
            var email = "koroten@ya.ru";
            var pwd = "123";
            var ctx = new DbContext(_cs);
            var result = await ctx.Users.Get(id);

            Assert.AreEqual(result.Id, id);
            Assert.AreEqual(result.Name, name);
            Assert.AreEqual(result.LastName, lname);
            Assert.AreEqual(result.MiddleName, mname);
            Assert.AreEqual(result.Password, pwd);
            Assert.AreEqual(result.Email, email);
        }
        [TestMethod]
        public async Task DeleteTest()
        {
            var id = _id;
            var ctx = new DbContext(_cs);
            var exist = await ctx.Users.Get(id);
            if (exist == null)
                await InsertTest();
            await ctx.Users.Delete(id);
            exist = await ctx.Users.Get(id);

            Assert.AreEqual(exist, null);
        }

        [TestMethod]
        public async Task UpdateTest()
        {
            var newname = "petr";
            var ctx = new DbContext(_cs);
            var item = await  ctx.Users.Get(_id);
            if (item == null) await InsertTest();
            item = await ctx.Users.Get(_id);
            item.Name = newname;
            await ctx.Users.Update(item);

            item = await ctx.Users.Get(_id);

            Assert.AreEqual(item.Name, newname);
        }
    }
}

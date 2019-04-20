#region License
// Разработано: Коротенко Владимиром Николаевичем (Vladimir N. Korotenko)
// email: koroten@ya.ru
// skype:vladimir-korotenko 
// https://vkorotenko.ru
// Создано:  20.04.2019 8:58
#endregion

using System.Threading.Tasks;
using ChatApplication.Dbl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChatApplication.Tests
{
    [TestClass]
    public class UserInRoleRepositoryTest
    {
        const string _cs = "server=localhost;UserId=chatapp;Password=ffgklmmmk56dfg;database=chatapp;";
        [TestMethod]
        public async Task GetUserInRolesListTest()
        {
            var ctx = new DbContext(_cs);
            var items = await ctx.UserInRole.GetAll();
            Assert.AreEqual(3, items.Count);
        }
        [TestMethod]
        public async Task GetUserInRolesItemTest()
        {
            var ctx = new DbContext(_cs);
            var item = await ctx.UserInRole.Get(1);
            var second = await ctx.UserInRole.Get(2);
            Assert.AreEqual(1, item.RoleId);
            Assert.AreEqual(79103421225, item.UserId);

            Assert.AreEqual(2, second.RoleId);
            Assert.AreEqual(79611759152, second.UserId);
        }

    }
}
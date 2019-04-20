using ChatApplication.Dbl;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace ChatApplication.Tests
{
    [TestClass]
    public class RolesRepositoryTests
    {
        private const string _cs = Cost.Connection;
        [TestMethod]
        public async Task GetRolesListTest()
        {
            var ctx = new DbContext(_cs);
            var items = await ctx.Roles.GetRoles();
            Assert.AreEqual(2,items.Count);
        }
        [TestMethod]
        public async Task GetRolesItemTest()
        {
            var ctx = new DbContext(_cs);
            var item = await ctx.Roles.Get(1);
            Assert.AreEqual("admin", item.Name);
            item = await ctx.Roles.Get(2);
            Assert.AreEqual("user", item.Name);
        }
    }
}

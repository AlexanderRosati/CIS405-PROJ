using Microsoft.VisualStudio.TestTools.UnitTesting;
using KP_Alg_Greedy_v._KP_Alg_DP;

namespace KPGreedyAlg_Unit_Tests
{
    [TestClass]
    public class GreedyTests
    {
        [TestMethod]
        public void TestMethodGreedy()
        {
            //Act
            //Note: KPGreedyAlg.Solve() is returning Tuple<int[], int>.
            var t1 = KPGreedyAlg.Solve(new int[5] { 2, 4, 3, 9, 1 }, new int[5] { 2, 5, 2, 3, 7 }, 10, 5);

            //Assert
            Assert.AreEqual(14, t1.Item2);
            CollectionAssert.AreEqual(new int[5] { 1, 0, 1, 1, 0 }, t1.Item1);
        }
    }
}

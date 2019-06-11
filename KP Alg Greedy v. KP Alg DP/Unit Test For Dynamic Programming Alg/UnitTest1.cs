using KP_Alg_Greedy_v._KP_Alg_DP;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Unit_Test_For_Dynamic_Programming_Alg
{
    [TestClass]
    public class DynamicProgrammingTests
    {
        [TestMethod]
        public void TestMethodDynamicProgramming()
        {
            //Note: Used this website to help with testing: 
            //http://karaffeltut.com/NEWKaraffeltutCom/Knapsack/knapsack.html

            //Act
            //Note: KPDynamicProgAlg.Solve() is returning Tuple<int[], int>.
            var t1 = KPDynamicProgAlg.Solve(new int[0], new int[0], 0, 0);
            var t2 = KPDynamicProgAlg.Solve(new int[2] { 2, 3 }, new int[2] { 3, 4 }, 0, 2);
            var t3 = KPDynamicProgAlg.Solve(new int[0], new int[0], 3, 0);
            var t4 = KPDynamicProgAlg.Solve(new int[1] { 3 }, new int[1] { 2 }, 3, 1);
            var t5 = KPDynamicProgAlg.Solve(new int[1] { 3 }, new int[1] { 2 }, 1, 1);

            var t6 = KPDynamicProgAlg.Solve(
                                            new int[6] { 10, 12, 3, 7, 20, 7 },
                                            new int[6] { 3, 6, 2, 4, 13, 1 }, 15, 6);
            var t7 = KPDynamicProgAlg.Solve(
                                            new int[7] { 4, 7, 1, 9, 3, 7, 10 },
                                            new int[7] { 2, 3, 9, 1, 3, 7, 6 }, 25, 7);
            var t8 = KPDynamicProgAlg.Solve(
                                            new int[6] { 1, 2, 4, 2, 11, 8 },
                                            new int[6] { 1, 2, 2, 6, 4, 3 }, 10, 6);
            var t9 = KPDynamicProgAlg.Solve(
                                            new int[7] { 2, 6, 2, 7, 11, 12, 1000 },
                                            new int[7] { 5, 15, 1, 3, 8, 5, 30 }, 30, 7);
            var t10 = KPDynamicProgAlg.Solve(
                                            new int[10] { 1, 2, 4, 7, 2, 9, 6, 7, 10, 15 },
                                            new int[10] { 2, 3, 5, 2, 8, 4, 3, 2, 9, 15 }, 25, 10);
            var t11 = KPDynamicProgAlg.Solve(
                                            new int[5] { 4, 6, 1, 9, 15 },
                                            new int[5] { 3, 2, 11, 4, 7 }, 10, 5);
            var t12 = KPDynamicProgAlg.Solve(
                                            new int[5] { 1, 2, 3, 4, 100 },
                                            new int[5] { 5, 5, 5, 5, 5 }, 5, 5);
            var t13 = KPDynamicProgAlg.Solve(
                                            new int[10] { 5, 8, 5, 3, 16, 8, 100, 1, 6, 7 },
                                            new int[10] { 2, 3, 8, 2, 4, 8, 16, 15, 2, 3 }, 15, 10);
            var t14 = KPDynamicProgAlg.Solve(
                                            new int[6] { 10, 10, 10, 10, 10, 10 },
                                            new int[6] { 3, 3, 3, 3, 3, 3 }, 18, 6);
            var t15 = KPDynamicProgAlg.Solve(
                                            new int[6] { 1, 2, 3, 6, 3, 4 },
                                            new int[6] { 200, 500, 400, 350, 300, 300 }, 1000, 6);
            var t16 = KPDynamicProgAlg.Solve(
                                            new int[10] { 19, 29, 4, 16, 24, 4, 17, 6, 34, 14 },
                                            new int[10] { 14, 20, 4, 15, 17, 3, 17, 6, 25, 25 }, 100, 10);
            var t17 = KPDynamicProgAlg.Solve(
                                            new int[10] { 8, 7, 20, 16, 8, 26, 8, 4, 18, 6 },
                                            new int[10] { 9, 8, 15, 15, 13, 20, 6, 3, 19, 11 }, 100, 10);
            
            //Assert
            Assert.AreEqual(0, t1.Item2);
            Assert.AreEqual(0, t2.Item2);
            Assert.AreEqual(0, t3.Item2);
            Assert.AreEqual(3, t4.Item2);
            Assert.AreEqual(0, t5.Item2);

            Assert.AreEqual(36, t6.Item2);
            CollectionAssert.AreEqual(new int[6] { 1, 1, 0, 1, 0, 1 }, t6.Item1);

            Assert.AreEqual(40, t7.Item2);
            CollectionAssert.AreEqual(new int[7] { 1, 1, 0, 1, 1, 1, 1 }, t7.Item1);

            Assert.AreEqual(24, t8.Item2);
            CollectionAssert.AreEqual(new int[6] { 1, 0, 1, 0, 1, 1 }, t8.Item1);

            Assert.AreEqual(1000, t9.Item2);
            CollectionAssert.AreEqual(new int[7] { 0, 0, 0, 0, 0, 0, 1 }, t9.Item1);

            Assert.AreEqual(43, t10.Item2);
            CollectionAssert.AreEqual(new int[10] { 0, 0, 1, 1, 0, 1, 1, 1, 1, 0 }, t10.Item1);

            Assert.AreEqual(21, t11.Item2);
            CollectionAssert.AreEqual(new int[5] { 0, 1, 0, 0, 1 }, t11.Item1);

            Assert.AreEqual(100, t12.Item2);
            CollectionAssert.AreEqual(new int[5] { 0, 0, 0, 0, 1 }, t12.Item1);

            Assert.AreEqual(42, t13.Item2);
            CollectionAssert.AreEqual(new int[10] { 1, 1, 0, 0, 1, 0, 0, 0, 1, 1 }, t13.Item1);

            Assert.AreEqual(60, t14.Item2);
            CollectionAssert.AreEqual(new int[6] { 1, 1, 1, 1, 1, 1 }, t14.Item1);

            Assert.AreEqual(13, t15.Item2);
            CollectionAssert.AreEqual(new int[6] { 0, 0, 0, 1, 1, 1 }, t15.Item1);

            Assert.AreEqual(132, t16.Item2);
            CollectionAssert.AreEqual(new int[10] { 1, 1, 0, 1, 1, 1, 0, 1, 1, 0 }, t16.Item1);

            Assert.AreEqual(108, t17.Item2);
            CollectionAssert.AreEqual(new int[10] { 1, 0, 1, 1, 1, 1, 1, 1, 1, 0 }, t17.Item1);
        }
    }
}
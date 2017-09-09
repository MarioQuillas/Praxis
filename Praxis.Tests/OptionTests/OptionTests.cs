namespace Praxis.Tests.OptionTests
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Praxis.Extensions;

    [TestClass]
    public class OptionTests
    {
        private static Test1 dataTest;

        [ClassInitialize]
        public static void InitializeData(TestContext context)
        {
            dataTest = new Test1() { DataTest1 = 12, DateTest1 = new DateTime(2016, 10, 2) };
        }

        [TestMethod]
        public void SelectMany_correct_return_two_methods_chained()
        {
            var query3 = from x in dataTest.WrapOption()
                         from y in this.Test1_To_Test2_correct_return(x).WrapOption()
                         let z = y.DataTest2
                         let w = x.DataTest1 + 20
                         select "z : " + z + " w : " + w + " y dates: " + y.DateTest2.Date.Year + " "
                                + y.DateTest2.DateString;

            var tot3 = query3.UnWrap();

            Assert.IsTrue(query3.UnWrap() == "z : 17 w : 32 y dates: 2016 02/10/2016");
        }

        [TestMethod]
        public void SelectManyTest_return_null_two_methods_chained()
        {
            // the query is not lazy but the bind method is "kind" of lazy
            // actually is not lazy at all but the last select doesn't even get executed
            var query2 = from x in dataTest.WrapOption()
                         from y in this.Test1_To_Test2_null_return(x).WrapOption()
                         select y.DataTest2 + " test_1: " + x.DataTest1.ToString();

            Assert.IsTrue(string.IsNullOrEmpty(query2.UnWrap()));
        }

        [TestMethod]
        public void SelectManyTests_three_method_chained_correct_return()
        {
            var query4 = from x in dataTest.WrapOption()
                         from y in this.Test1_To_Test2_correct_return(x).WrapOption()
                         from z in this.Test2_to_Test3_correct_return(y).WrapOption()
                         select "z : " + z.DataTest3 + " y dates: " + y.DateTest2.Date.Year + " "
                                + y.DateTest2.DateString;

            var tot4 = query4.UnWrap();

            Assert.IsTrue(query4.UnWrap() == "z : 17:test3 y dates: 2016 02/10/2016");
        }

        [TestMethod]
        public void SelectManyTests_three_method_chained_null_return()
        {
            var query5 = from x in dataTest.WrapOption() // x = dataTest.WrapOption()
                         from y in
                             this.Test1_To_Test2_correct_return(x)
                                 .WrapOption() // y = Test1_To_Test2_correct_return(x).WrapOption() 
                         from z in
                             this.Test2_to_Test3_null_return(y)
                                 .WrapOption() // z = Test2_to_Test3_null_return(y).WrapOption() 
                         select "z : " + z + " y dates: " + y.DateTest2.Date.Year + " " + y.DateTest2.DateString + " "
                                + x.DataTest1;

            var tot5 = query5.UnWrap();

            Assert.IsTrue(string.IsNullOrEmpty(query5.UnWrap()));
        }

        [TestMethod]
        public void SelectTest()
        {
            var query = from x in dataTest.WrapOption() select x.DataTest1.ToRefType();

            Assert.IsTrue(query.UnWrap().Value == 12);

            var query1 = from x in (null as Test1).WrapOption() select x.DataTest1.ToRefType();

            Assert.IsTrue(query1.UnWrap() == null);
        }

        public Test2 Test1_To_Test2_correct_return(Test1 test)
        {
            return new Test2()
                       {
                           DataTest2 = (test.DataTest1 + 5).ToString(),
                           DateTest2 = new StringDateTest()
                                           {
                                               Date = test.DateTest1,
                                               DateString =
                                                   test.DateTest1.ToString("dd/MM/yyyy")
                                           }
                       };
        }

        public Test2 Test1_To_Test2_null_return(Test1 test) => null;

        public Test3 Test2_to_Test3_correct_return(Test2 test) => new Test3()
                                                                      {
                                                                          DataTest3 = test.DataTest2 + ":test3",
                                                                          DateStringTest3 =
                                                                              test.DateTest2.Date.AddDays(5)
                                                                                  .ToShortDateString()
                                                                      };

        public Test3 Test2_to_Test3_null_return(Test2 test) => null;

        public Test3 TwoParameterTest(Test1 test1, Test2 test2) => null;
    }
}
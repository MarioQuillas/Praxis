namespace Praxis.Tests.ResultTests
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Praxis.Extensions;

    [TestClass]
    public class ResultTests
    {
        [ClassInitialize]
        public static void InitializeData(TestContext context)
        {
        }

        [TestMethod]
        public void select_tests()
        {
            var querySelectSuccess = from x in ValidatorTest.IsNotNull("toto") select x + " 1";

            Assert.IsTrue(querySelectSuccess.IsSuccess);
            Assert.IsTrue(querySelectSuccess.SuccessValue == "toto 1");

            var querySelectFailure = from x in ValidatorTest.IsNotNull((string)null) select x + " 1";

            Assert.IsFalse(querySelectFailure.IsSuccess);
            Assert.IsTrue(querySelectFailure.FailureValue == "Value cannot be null");
        }

        [TestMethod]
        public void SelectMany_tests()
        {
            var querySelect = from x in ValidatorTest.IsNotNull("toto") select x + ":2";

            Assert.IsTrue(querySelect.IsSuccess);
            Assert.IsTrue(querySelect.SuccessValue == "toto:2");

            var querySelectEmpty = from x in ValidatorTest.IsNotNull((string)null) select x + ":2";

            Assert.IsFalse(querySelectEmpty.IsSuccess);
            Assert.IsTrue(querySelectEmpty.FailureValue == "Value cannot be null");

            var querySelectMany = from x in ValidatorTest.IsNotNull("toto")
                                  from y in ValidatorTest.IsNotEmpty(x)
                                  let z = x + " titi"
                                  let w = x + " gogo"
                                  from rr in ValidatorTest.MinLength(8)(z + " " + w)
                                  select rr.Length;

            Assert.IsTrue(querySelectMany.IsSuccess);
            Assert.IsTrue(querySelectMany.SuccessValue == 19);

            var querySelectMany2 = from x in ValidatorTest.IsNotNull("toto")
                                   from y in ValidatorTest.IsNotEmpty(x)
                                   let z = x + " titi"
                                   let w = x + " gogo"
                                   from rr in ValidatorTest.MinLength(20)(z + " " + w)
                                   select rr.Length;

            Assert.IsFalse(querySelectMany2.IsSuccess);
            Assert.IsTrue(querySelectMany2.FailureValue == "Value must be at least 20 characters long");

            var querySelectManyFailure = from x in ValidatorTest.IsNotNull(string.Empty)
                                         from y in ValidatorTest.IsNotEmpty(x)
                                         select y.Length;

            Assert.IsFalse(querySelectManyFailure.IsSuccess);
            Assert.IsTrue(querySelectManyFailure.FailureValue == "Value cannot be empty");

            var query3 = from x in ValidatorTest.IsNotNull("toto")
                         from y in ValidatorTest.MinLength(8)(x)
                         let z = y.Length + 20
                         let w = x + " 2 :"
                         select new { zeta = z, ww = w };

            Assert.IsFalse(query3.IsSuccess);
            Assert.IsTrue(query3.FailureValue == "Value must be at least 8 characters long");

            var query4 = from x in ValidatorTest.IsNotNull("toto toto")
                         from y in ValidatorTest.MinLength(8)(x)
                         let z = y.Length + 20
                         let w = x + " 2 : two"
                         select new { zeta = z, ww = w };

            Assert.IsTrue(query4.IsSuccess);
            Assert.IsTrue(query4.SuccessValue.zeta == 29);
            Assert.IsTrue(query4.SuccessValue.ww == "toto toto 2 : two");

            var query5 = from x in 55.WrapResult<int, string>() from y in this.testMethodWithException(x) select y;

            Assert.IsTrue(query5.IsSuccess);
            Assert.IsTrue(query5.SuccessValue == 110);

            var query6 = from x in 0.WrapResult<int, string>() from y in this.testMethodWithException(x) select y;

            Assert.IsFalse(query6.IsSuccess);

            // Assert.IsTrue(query6.FailureValue == @"System.Exception: Super toto exception
            // at efront.frontpmengine.Tests.SharedKernelTests.FunctionalExtensionsTests.ResultTests.testMethodWithException(Int32 x) in C:\dev4.1\srcajxvc\frontpmengine.Tests\SharedKernelTests\FunctionalExtensionsTests\ResultTests.cs:line 82");
        }

        public Result<int, string> testMethodWithException(int x)
        {
            try
            {
                if (x == 0) throw new Exception("Super toto exception");
                return (x * 2).WrapResult<int, string>();
            }
            catch (Exception e)
            {
                return Result<int, string>.FailWith(e.ToString());
            }
        }
    }
}
namespace Praxis.Tests.FuncTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class FuncTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            // Func<string> query = from filePath in new Func<string>(Console.ReadLine)
            // from encodingName in new Func<string>(Console.ReadLine)
            // from encoding in new Func<Encoding>(() => Encoding.GetEncoding(encodingName))
            // from fileContent in new Func<string>(() => File.ReadAllText(filePath, encoding))
            // select fileContent; // Define query.
            // string result = query(); // Execute query.

            // Func<string> query = from filePath in new Func<string>(() => "toto")
            // from encodingName in new Func<string>(() => "tata")
            // from encoding in new Func<Encoding>(() => Encoding.GetEncoding(encodingName))
            // from fileContent in new Func<string>(() => File.ReadAllText(filePath, encoding))
            // select fileContent; // Define query.
            // string result = query(); // Execute query.
        }
    }
}
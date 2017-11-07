using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using MsgWriter;

namespace HelloWorldTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestDefaults()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                MessageWriter mw = new MessageWriter();
                mw.Write();

                string expected = "Hello World";
                Assert.AreEqual<string>(expected, sw.ToString());
            }
        }

        [TestMethod]
        public void TestMessage()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                string expected = "Test";

                MessageWriter mw = new MessageWriter();
                mw.Message = expected;
                mw.Write();

                Assert.AreEqual<string>(expected, sw.ToString());
            }
        }

        [TestMethod]
        public void TestWriterBase()
        {
            using (StringWriter sw = new StringWriter())
            {
                StringWriter con = new StringWriter();
                Console.SetOut(con);

                string expected = "Testing";
                string unexpected = "Hello World";

                MessageWriter mw = new MessageWriter();

                mw.SetWriterFunc(
                    (m, t) => { ((StringWriter)t).Write(m); }
                    );
                mw.Message = expected;
                mw.Target = sw;
                mw.Write();

                Assert.AreEqual<string>(expected, sw.ToString());
                Assert.AreNotEqual<string>(expected, con.ToString());
                Assert.AreNotEqual<string>(unexpected, con.ToString());
            }
        }

        [TestMethod]
        public void TestWriterOverride()
        {
            using (StringWriter sw = new StringWriter())
            {
                StringWriter con = new StringWriter();
                Console.SetOut(con);

                string expected = "Testing";
                string unexpected = "Hello World";

                MessageWriter mw2 = new MessageWriter(sw, expected, (m, t) => { ((StringWriter)t).Write(m); });

                mw2.Write();

                Assert.AreEqual<string>(expected, sw.ToString());
                Assert.AreNotEqual<string>(expected, con.ToString());
                Assert.AreNotEqual<string>(unexpected, con.ToString());
            }
        }
    }
}

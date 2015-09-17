using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SedcServer;

namespace SedcServerTests
{
    [TestClass]
    public class WebServerTests
    {
        [TestMethod]
        public void ConstructorTest_One()
        {
            ushort port = 123;
            WebServer ws = new WebServer(port);

            Assert.AreEqual(port, ws.Port);
        }

        [TestMethod]
        public void ConstructorTest_Zero()
        {
            ushort port = 0;
            WebServer ws = new WebServer(port);

            Assert.AreEqual(port, ws.Port);
        }

        [TestMethod]
        public void StartTest_FirstCall()
        {
            ushort port = 0;
            WebServer ws = new WebServer(port);

            var result = ws.Start();
            Assert.AreEqual("Started...",result);
        }

        [TestMethod]
        public void StartTest_SecondCall()
        {
            ushort port = 0;
            WebServer ws = new WebServer(port);

            ws.Start();
            var result = ws.Start();
            Assert.AreEqual("Already started!", result);
        }



    }
}

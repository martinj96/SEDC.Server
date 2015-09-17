using Microsoft.VisualStudio.TestTools.UnitTesting;
using SedcServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SedcServerTests
{
    [TestClass]
    public class RequestParserTests
    {
        [TestMethod]
        public void mozillaRequest()
        {
            var requestString = @"GET / HTTP/1.1
                                Host: localhost:5050
                                User-Agent: Mozilla/5.0 (Windows NT 6.3; WOW64; rv:38.0) Gecko/20100101 Firefox/38.0
                                Accept: text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8
                                Accept-Language: en-US,en;q=0.5
                                Accept-Encoding: gzip, deflate
                                Connection: keep-alive";
            var parser = new RequestParser(requestString);
            Assert.AreEqual(RequestKind.Action,parser.Kind);
            Assert.AreEqual("localhost:5050", parser.Host);
            Assert.AreEqual("GET", parser.Method);
            Assert.AreEqual("/", parser.PathString);
            Assert.AreEqual(null, parser.Extension);
            Assert.AreEqual(null, parser.FileName);
            Assert.AreEqual(null, parser.QueryParameters);
            Assert.AreEqual("", parser.Action);
            Assert.AreEqual("", parser.Parameter);
            Assert.AreEqual(5050, parser.Port);
            Assert.AreEqual("Mozilla/5.0 (Windows NT 6.3; WOW64; rv:38.0) Gecko/20100101 Firefox/38.0", parser.UserAgent);
        }
        [TestMethod]
        public void mozillaWithAction()
        {
            var requestString = @"GET /sayhello/trajko HTTP/1.1
                                Host: localhost:5050
                                User-Agent: Mozilla/5.0 (Windows NT 6.3; WOW64; rv:38.0) Gecko/20100101 Firefox/38.0
                                Accept: text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8
                                Accept-Language: en-US,en;q=0.5
                                Accept-Encoding: gzip, deflate
                                Connection: keep-alive";
            var parser = new RequestParser(requestString);
            Assert.AreEqual(RequestKind.Action, parser.Kind);
            Assert.AreEqual("localhost:5050", parser.Host);
            Assert.AreEqual("GET", parser.Method);
            Assert.AreEqual("/sayhello/trajko", parser.PathString);
            Assert.AreEqual(null, parser.Extension);
            Assert.AreEqual(null, parser.FileName);
            Assert.AreEqual(null, parser.QueryParameters);
            Assert.AreEqual("sayhello", parser.Action);
            Assert.AreEqual("trajko", parser.Parameter);
            Assert.AreEqual(5050, parser.Port);
            Assert.AreEqual("Mozilla/5.0 (Windows NT 6.3; WOW64; rv:38.0) Gecko/20100101 Firefox/38.0", parser.UserAgent);
        }
        [TestMethod]
        public void mozillaWithParameters()
        {
            var requestString = @"GET /sayhello/trajko?surname=trajkovski&birthplace=konopishte HTTP/1.1
                                Host: localhost:5050
                                User-Agent: Mozilla/5.0 (Windows NT 6.3; WOW64; rv:38.0) Gecko/20100101 Firefox/38.0
                                Accept: text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8
                                Accept-Language: en-US,en;q=0.5
                                Accept-Encoding: gzip, deflate
                                Connection: keep-alive";
            var parser = new RequestParser(requestString);
            Assert.AreEqual(RequestKind.Action, parser.Kind);
            Assert.AreEqual("localhost:5050", parser.Host);
            Assert.AreEqual("GET", parser.Method);
            Assert.AreEqual("/sayhello/trajko", parser.PathString);
            Assert.AreEqual(null, parser.Extension);
            Assert.AreEqual(null, parser.FileName);
            var expected = new List<QueryParameter> { 
                new QueryParameter{ Name = "surname", Value = "trajkovski"},
                new QueryParameter{ Name = "birthplace", Value = "konopishte"}
            };
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i], parser.QueryParameters[i]);
            }
            Assert.AreEqual("sayhello", parser.Action);
            Assert.AreEqual("trajko", parser.Parameter);
            Assert.AreEqual(5050, parser.Port);
            Assert.AreEqual("Mozilla/5.0 (Windows NT 6.3; WOW64; rv:38.0) Gecko/20100101 Firefox/38.0", parser.UserAgent);
        }
        [TestMethod]
        public void mozillaWithExistingFile()
        {
            var requestString = @"GET /test.html HTTP/1.1
                                Host: localhost:5050
                                User-Agent: Mozilla/5.0 (Windows NT 6.3; WOW64; rv:40.0) Gecko/20100101 Firefox/40.0
                                Accept: text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8
                                Accept-Language: en-US,en;q=0.5
                                Accept-Encoding: gzip, deflate
                                Connection: keep-alive";
            var parser = new RequestParser(requestString);
            Assert.AreEqual(RequestKind.File, parser.Kind);
            Assert.AreEqual("localhost:5050", parser.Host);
            Assert.AreEqual("GET", parser.Method);
            Assert.AreEqual("/test.html", parser.PathString);
            Assert.AreEqual("html", parser.Extension);
            Assert.AreEqual("test.html", parser.FileName);
            Assert.AreEqual(null, parser.Action);
            Assert.AreEqual(null, parser.Parameter);
            Assert.AreEqual(5050, parser.Port);
            Assert.AreEqual("Mozilla/5.0 (Windows NT 6.3; WOW64; rv:40.0) Gecko/20100101 Firefox/40.0", parser.UserAgent);
        }
        [TestMethod]
        public void mozillaWithNonExistingFile()
        {
            var requestString = @"GET /someNonExistingName.html HTTP/1.1
                                Host: localhost:5050
                                User-Agent: Mozilla/5.0 (Windows NT 6.3; WOW64; rv:40.0) Gecko/20100101 Firefox/40.0
                                Accept: text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8
                                Accept-Language: en-US,en;q=0.5
                                Accept-Encoding: gzip, deflate
                                Connection: keep-alive";
            var parser = new RequestParser(requestString);
            Assert.AreEqual(RequestKind.File, parser.Kind);
            Assert.AreEqual("localhost:5050", parser.Host);
            Assert.AreEqual("GET", parser.Method);
            Assert.AreEqual("/someNonExistingName.html", parser.PathString);
            Assert.AreEqual("html", parser.Extension);
            Assert.AreEqual("someNonExistingName.html", parser.FileName);
            Assert.AreEqual(null, parser.Action);
            Assert.AreEqual(null, parser.Parameter);
            Assert.AreEqual(5050, parser.Port);
            Assert.AreEqual("Mozilla/5.0 (Windows NT 6.3; WOW64; rv:40.0) Gecko/20100101 Firefox/40.0", parser.UserAgent);
        }
    }

}

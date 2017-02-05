using System;
using NUnit.Framework;

namespace AOUT.CH3.LogAnStubAndMock.UnitTests
{
    [TestFixture]
    public class LogAnalyzer2Tests
    {        
        [Test]
        public void Analyze_WebServiceThrows_SendsEmail()
        {
            FakeWebService stubService = new FakeWebService();
            stubService.ToThrow = new Exception("fake exception");
            FakeEmailService mockEmail = new FakeEmailService();
            LogAnalyzer2 log = new LogAnalyzer2(stubService, mockEmail);

            string tooShortFileName = "abc.ext";
            log.Analyze(tooShortFileName);

            StringAssert.Contains("someone@somewhere.com", mockEmail.To);
            StringAssert.Contains("fake exception", mockEmail.Body);
            StringAssert.Contains("can’t log", mockEmail.Subject);

        }

        public class FakeWebService : IWebService
        {
            public Exception ToThrow;

            public void LogError(string message)
            {
                if (ToThrow != null)
                {
                    throw ToThrow;
                }
            }
        }

        public class FakeEmailService : IEmailService
        {
            public string To;
            public string Subject;
            public string Body;

            public void SendEmail(string to, string subject, string body)
            {
                To = to;
                Subject = subject;
                Body = body;
            }
        }
    }
}

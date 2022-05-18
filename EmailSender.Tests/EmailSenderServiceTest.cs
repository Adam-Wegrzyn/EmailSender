using ElasticEmail.Model;
using NUnit.Framework;
using EmailSender.Exceptions;

namespace EmailSender.Tests
{
    public class EmailSenderServiceTest
    {
        [Test]
        public void EmailCreation_DataInEmailShouldBeEqualWithInput()
        {
            //arrange
            var senderService = new EmailSenderService();

            //mock user input
            var input = String.Join(Environment.NewLine, new[]
            {
                //sender email
                "john@gmail.com",
                //sender name
                "John",
                //recipient
                "kate@gmail.com",
                //subject
                "Test subject",
                //content
                "Test content",
                //is plain/text
                "N"

            });

            //act
            Console.SetIn(new System.IO.StringReader(input));
            var emailData = senderService.EmailCreation();

            //assert
            Assert.AreEqual("John <john@gmail.com>", emailData.Content.From);
            Assert.AreEqual("kate@gmail.com", emailData.Recipients[0].Email);
            Assert.AreEqual("Test subject", emailData.Content.Subject);
            Assert.AreEqual("Test content", emailData.Content.Body[0].Content);
            Assert.AreEqual(BodyContentType.HTML, emailData.Content.Body[0].ContentType);
            
        }

        [Test]
        public void MergeCsvEmailCreation_DataInEmailShouldBeEqualWithInput()
        {
            //arrange
            var sender = new EmailSenderService();

            //mock user input
            var input = String.Join(Environment.NewLine, new[]
            {
                //file path
                "Files/mergeEmail_Valid.csv",
                //sender email
                "john@gmail.com",
                //sender name
                "John",
                //subject
                "Test subject",
                //content
                "Test content",
                //is plain/text
                "Y"

            });

            //act
            Console.SetIn(new System.IO.StringReader(input));
            var emailData = sender.MergeCsvEmailCreation();

            //assert;
            Assert.AreEqual("John <john@gmail.com>", emailData.Content.From);
            Assert.AreEqual("Test subject", emailData.Content.Subject);
            Assert.AreEqual("Test content", emailData.Content.Body[0].Content);
            Assert.AreEqual(BodyContentType.PlainText, emailData.Content.Body[0].ContentType);
        }

    }
}
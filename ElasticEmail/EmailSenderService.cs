using CsvHelper;
using ElasticEmail.Api;
using ElasticEmail.Client;
using ElasticEmail.Model;
using System.Diagnostics;
using System.Globalization;
using EmailSender;


namespace EmailSender
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly EmailsApi _apiInstance;


        public EmailSenderService()
        {
            var config = new Configuration();
            config.ApiKey.Add("X-ElasticEmail-ApiKey", "~");
            _apiInstance = new EmailsApi(config);
        }

        public void SendEmailsFromCsv(MergeEmailPayload messageDataCsv)
        {
            try
            {
                var apiResponse = _apiInstance.EmailsMergefilePostWithHttpInfo(messageDataCsv);

                Debug.Print(apiResponse.StatusCode.ToString());
                Console.WriteLine(apiResponse.StatusCode.ToString());
            }
            catch (Exception ex)
            {
                Debug.Print(ex.ToString());
                Console.WriteLine(ex.Message);
            }

        }

        public void SendEmail(EmailMessageData messageData)
        {
            string message = "Mail has been sent!";
            try
            {
                var apiResponse = _apiInstance.EmailsPostWithHttpInfo(messageData);

                Debug.Print(apiResponse.StatusCode.ToString());
                Console.WriteLine(apiResponse.StatusCode.ToString());
            }
            catch (ApiException ex)
            {
                Debug.Print($"An exception {ex} occured with message: {ex.Message}");
                Debug.Print($"Status code: {ex.ErrorCode}");
                Debug.Print(ex.StackTrace);
                message = $"Mail cannot be sent! /n An Error occured: {ex.Message}";
                Console.WriteLine(ex.Message);
            }
            
        }

        

        public EmailMessageData EmailCreation()
        {

            Console.WriteLine("Provide email sender email: ");
            string senderEmail = EmptyInputValidator.Validate(Console.ReadLine());
            Console.WriteLine("Provide email sender name: ");
            string sender = EmptyInputValidator.Validate(Console.ReadLine());
            Console.WriteLine("Provide email recipient: ");
            string recipient = EmptyInputValidator.Validate(Console.ReadLine());
            Console.WriteLine("Provide email subject: ");
            string subject = EmptyInputValidator.Validate(Console.ReadLine());
            Console.WriteLine("Provide email content: ");
            string contentt = EmptyInputValidator.Validate(Console.ReadLine());
            Console.WriteLine("Send as text/plain ? \n Y - yes \n N - no");
            string contentTypeSelect = EmptyInputValidator.Validate(Console.ReadLine());
            var contentType = GetContentType(contentTypeSelect);

            var createdEmail = new EmailMessageData(
                new List<EmailRecipient>()
                {
                    new EmailRecipient(recipient)
                },
                new EmailContent(
                    new List<BodyPart>()
                    {
                        new BodyPart()
                        {
                            ContentType = contentType,
                            Content = contentt
                        }
                    },
                    subject: subject,
                    from: $"{sender} <{senderEmail}>"
                )
            );

            return createdEmail;
        }


        private BodyContentType GetContentType(string input)
        {
            BodyContentType contentType = BodyContentType.HTML;
            if (input == "Y") { contentType = BodyContentType.PlainText; }

            return contentType;
        }

        public MergeEmailPayload MergeCsvEmailCreation()
        {
            // var filePath = @"Files\merge_email1.csv";

            Console.WriteLine("Provide file path for CSV file: ");
            string filePath = EmptyInputValidator.Validate(Console.ReadLine());
            using var file = File.OpenRead(filePath);
            CsvValidator.Validate(file);

            var readedFile = File.ReadAllText(filePath);
            var csvEncoded = System.Text.Encoding.ASCII.GetBytes(readedFile);

            Console.WriteLine("Provide email sender email: ");
            string senderEmail = EmptyInputValidator.Validate(Console.ReadLine());
            Console.WriteLine("Provide email sender name: ");
            string sender = EmptyInputValidator.Validate(Console.ReadLine());
            Console.WriteLine("Provide email subject: ");
            string subject = EmptyInputValidator.Validate(Console.ReadLine());
            Console.WriteLine("Provide email content: ");
            string contentt = EmptyInputValidator.Validate(Console.ReadLine());
            Console.WriteLine("Send as text/plain ? \n Y - yes \n N - no");
            string contentTypeSelect = EmptyInputValidator.Validate(Console.ReadLine());
            var contentType = GetContentType(contentTypeSelect);

            var messageDataCsv = new MergeEmailPayload(
                new MessageAttachment(csvEncoded)
                {
                    Name = "CSV merge",
                    ContentType = "text/csv"
                },
                new EmailContent(
                    new List<BodyPart>()
                    {
                        new BodyPart()
                        {
                            ContentType = contentType,
                            Content = contentt
                        },
                    },
                    subject: subject,
                    from: $"{sender} <{senderEmail}>"
                )
            );
            return messageDataCsv;
        }
    }
}

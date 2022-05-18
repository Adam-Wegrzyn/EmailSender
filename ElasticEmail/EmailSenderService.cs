using CsvHelper;
using ElasticEmail.Api;
using ElasticEmail.Client;
using ElasticEmail.Model;
using System.Diagnostics;
using System.Globalization;

namespace EmailSender
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly EmailsApi _apiInstance;


        public EmailSenderService()
        {
            var config = new Configuration();
            config.ApiKey.Add("X-ElasticEmail-ApiKey", "358BA71D2C63FDF4541BEEE28EDB727F9C8F5B1A2669805EDFD81A94190F5D70E93174B73AFFE25B430418831E059F8D");
            _apiInstance = new EmailsApi(config);
        }

        public void SendEmailsFromCsv(MergeEmailPayload messageDataCsv)
        {
            try
            {
                var apiResponse = _apiInstance.EmailsMergefilePostWithHttpInfo(messageDataCsv);

                Debug.Print(apiResponse.StatusCode.ToString());
            }
            catch (Exception ex)
            {
                Debug.Print(ex.ToString());
                Console.WriteLine(ex.Message);
            }

        }

        public string SendEmail(EmailMessageData messageData)
        {
            string message = "Mail has been sent!";
            try
            {
                var apiResponse = _apiInstance.EmailsPostWithHttpInfo(messageData);

                Debug.Print(apiResponse.StatusCode.ToString());
            }
            catch (ApiException ex)
            {
                Debug.Print($"An exception {ex} occured with message: {ex.Message}");
                Debug.Print($"Status code: {ex.ErrorCode}");
                Debug.Print(ex.StackTrace);
                message = $"Mail cannot be sent! /n An Error occured: {ex.Message}";
            }
            return message;
        }

        public EmailMessageData EmailCreation()
        {

            Console.WriteLine("Provide email sender email: ");
            string senderEmail = ValidateEmptyInput(Console.ReadLine());
            Console.WriteLine("Provide email sender name: ");
            string sender = ValidateEmptyInput(Console.ReadLine());
            Console.WriteLine("Provide email recipient: ");
            string recipient = ValidateEmptyInput(Console.ReadLine());
            Console.WriteLine("Provide email subject: ");
            string subject = ValidateEmptyInput(Console.ReadLine());
            Console.WriteLine("Provide email content: ");
            string contentt = ValidateEmptyInput(Console.ReadLine());
            Console.WriteLine("Send as text/plain ? /n Y - yes /n N - no");
            string contentTypeSelect = ValidateEmptyInput(Console.ReadLine());
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

        string ValidateEmptyInput(string input)
        {
            while (String.IsNullOrEmpty(input))
            {
                Console.WriteLine("Input cannot be empty!");
                input = Console.ReadLine();
            }
            return input;
        }

        BodyContentType GetContentType(string input)
        {
            BodyContentType contentType;
            bool isPlainText = false;

            if (input == "T") { isPlainText |= true; } else if (input == "N") { isPlainText |= false; };
            if (isPlainText) { contentType = BodyContentType.PlainText; } else { contentType = BodyContentType.HTML; };
            return contentType;
        }

        public MergeEmailPayload MergeCsvEmailCreation()
        {
            // var filePath = @"Files\merge_email1.csv";

            Console.WriteLine("Provide file path for CSV file: ");
            string filePath = ValidateEmptyInput(Console.ReadLine());
            using var file = File.OpenRead(filePath);
            CsvValidator.Validate(file);

            var readedFile = File.ReadAllText(filePath);
            var csvEncoded = System.Text.Encoding.ASCII.GetBytes(readedFile);

            Console.WriteLine("Provide email sender email: ");
            string senderEmail = ValidateEmptyInput(Console.ReadLine());
            Console.WriteLine("Provide email sender name: ");
            string sender = ValidateEmptyInput(Console.ReadLine());
            Console.WriteLine("Provide email subject: ");
            string subject = ValidateEmptyInput(Console.ReadLine());
            Console.WriteLine("Provide email content: ");
            string contentt = ValidateEmptyInput(Console.ReadLine());
            Console.WriteLine("Send as text/plain ? \n Y - yes \n N - no");
            string contentTypeSelect = ValidateEmptyInput(Console.ReadLine());
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
                    ContentType = contentType
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

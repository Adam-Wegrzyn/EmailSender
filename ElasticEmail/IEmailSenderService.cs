using ElasticEmail.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSender
{
    public interface IEmailSenderService
    {
        void SendEmailsFromCsv(MergeEmailPayload messageDataCsv);
        void SendEmail(EmailMessageData messageData);

        EmailMessageData EmailCreation();
        MergeEmailPayload MergeCsvEmailCreation();
    }
}

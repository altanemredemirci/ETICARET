using SendGrid;
using SendGrid.Helpers.Mail;

namespace ETICARET.WebUI.EmailServices
{
    public class EmailSender
    {
        //private const string SendGridKey = "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";

        //public static async Task Execute(string subject, string message, string email)
        //{
        //    var client = new SendGridClient(SendGridKey);
        //    var from = new EmailAddress("yazilim.ucuncubinyil@hotmail.com", "UBY12345");
        //    var to = new EmailAddress(email);
        //    var plainTextContent = message;
        //    var htmlContent = message;
        //    var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

        //    var response = await client.SendEmailAsync(msg);
        //}
    }
}

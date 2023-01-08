using DD_Business.Repository.IRepository;
using DD_Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
namespace DD_Business.Repository
{
    public class EmailRepository: IEmailRepository
    {
        public void SendEmail(OrderDTO orderDetails)
        {
            // create email message
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("info@ddbijnor.com"));
            email.To.Add(MailboxAddress.Parse("gauravr114@gmail.com"));
            email.Cc.Add(MailboxAddress.Parse("saurabhrajput142@gmail.com"));
            email.Subject = $"New Order Request {orderDetails.OrderHeader.Id} In DD";
            email.Body = new TextPart(TextFormat.Html) 
            { 
                Text = "We got a new order request please check in your app <br/>" +
                $"Order Details: <br/> <h1> " +
                $"Order Id {orderDetails.OrderHeader.Id}" +
                $" <br/> Customer Mobile {orderDetails.OrderHeader.PhoneNumber}</h1>"
            };

            // send email
            try
            {
                SmtpClient smtp = new();
                smtp.Connect("consent.herosite.pro", 25, SecureSocketOptions.StartTls);
                smtp.Authenticate("info@ddbijnor.com", "Gaurav12!@");
                smtp.Send(email);
                smtp.Disconnect(true);
            }
            catch (Exception ex)
            {

            }
           
        }

        public void SendOrderCheckedEmail(OrderDTO orderDetails)
        {
            // create email message
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("info@ddbijnor.com"));
            email.To.Add(MailboxAddress.Parse("gauravr114@gmail.com"));
            email.Cc.Add(MailboxAddress.Parse("saurabhrajput142@gmail.com"));
            email.Subject = $"order {orderDetails.OrderHeader.Id} has been checked by admin";
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = "This order has been checked by admin <br/> so you can ignore it." +
                $"Order Details: <br/> <h1> " +
                $"Order Id {orderDetails.OrderHeader.Id}" +
                $" <br/> Customer Mobile {orderDetails.OrderHeader.PhoneNumber}</h1>"
            };

            // send email
            try
            {
                SmtpClient smtp = new();
                smtp.Connect("consent.herosite.pro", 25, SecureSocketOptions.StartTls);
                smtp.Authenticate("info@ddbijnor.com", "Gaurav12!@");
                smtp.Send(email);
                smtp.Disconnect(true);
            }
            catch (Exception ex)
            {

            }

        }
    }
}

using System;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using System.IO;

namespace TestEmailClient
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create new mime message object which we are going to use to fill the message data
            MimeMessage message = new MimeMessage();
            // Add the sender infor that will appear in the email message
            message.From.Add(new MailboxAddress("Tester", "kerr.tymor@gmail.com"));

            // Add the reciever email address
            message.To.Add(MailboxAddress.Parse("kerr.tymor@gmail.com"));

            // Add the message subject
            message.Subject = "Test Email";

            // Add the message body as plain text
            // The 'plain' string passed to thte TextPart indicates the it's plain text and not HTML for example
            message.Body = new TextPart("plain")
            {
                Text = @"This is a test email using MailKit!"
            };

            var builder = new BodyBuilder();

            // Set the plain-text version of the message text
            builder.TextBody = @"This is a test email using MailKit!";

            // Full file path:
            // E:\Visual Studio 2019\Projects\TestEmailClient\TestEmailClient\bin\Debug\Test Document.txt

            // Add an attachement to the message
            builder.Attachments.Add(@"Test Document.txt");

            // Set the message body
            message.Body = builder.ToMessageBody();

            // Ask the user to enter their email
            Console.Write("Email: ");
            string emailAddress = Console.ReadLine();

            // Ask the user to enter their password
            Console.Write("Password: ");
            string password = Console.ReadLine();

            // Create a new SMTP client
            SmtpClient client = new SmtpClient();

            try
            {
                // Connect to the gmail SMTP server using port 465 with SSL enabled
                client.Connect("smtp.gmail.com", 465, true);
                // Note: only needed if the SMTP server requires authentication, like gmail for example
                client.Authenticate(emailAddress, password);
                client.Send(message);

                // Display a message if no exception was thrown
                Console.WriteLine("Email sent!");
            }
            catch (Exception ex)
            {
                // In the case of an error, display the message
                Console.WriteLine(ex);
            }
            finally
            {
                // At any case always disconnect from the client
                client.Disconnect(true);
                // Dispose of the client object
                client.Dispose();
            }

            Console.ReadLine();
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace BookShoppingCartMvcUI.Constants
{
    public class DummyEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            Console.WriteLine($"[Dummy Email] To: {email}, Subject: {subject}");
            return Task.CompletedTask;
        }
    }

}

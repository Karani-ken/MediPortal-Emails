using MediPortal_Emails.Data;
using MediPortal_Emails.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace MediPortal_Emails.Services
{
    public class EmailService
    {
        private DbContextOptions<ApplicationDbContext> options;
        public EmailService()
        {

        }
        public EmailService(DbContextOptions<ApplicationDbContext> options)
        {
            this.options = options;
        }
        public async Task SaveData(EmailLoggers emailLoggers)
        {
            //create _context

            var _context = new ApplicationDbContext(this.options);
            _context.EmailLoggers.Add(emailLoggers);
            await _context.SaveChangesAsync();
        }
    }
}

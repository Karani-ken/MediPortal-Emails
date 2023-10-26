using MediPortal_Emails.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;

namespace MediPortal_Emails.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<EmailLoggers> EmailLoggers { get; set; }
    }
}

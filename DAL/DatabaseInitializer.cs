
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IDatabaseInitializer
    {
        Task SeedAsync();
    }




    public class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly ApplicationDbContext _context;        
       private readonly ILogger _logger;

        public DatabaseInitializer(ApplicationDbContext context, ILogger<DatabaseInitializer> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            await _context.Database.MigrateAsync().ConfigureAwait(false);


            
                _logger.LogInformation("Seeding initial data");

            Meeting meet_1 = new Meeting
            {
                BeginMeeting = DateTime.Now,
                EndMeeting = DateTime.Now.AddHours(5),
                PatientName = "Teste",
                Observation = "Teste",
                Birth = DateTime.Now
                };

      
                _context.Meeting.Add(meet_1);
                
                await _context.SaveChangesAsync();

                _logger.LogInformation("Seeding initial data completed");
            
        }
        
    }
}

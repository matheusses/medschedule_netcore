using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
    public class MeetingRepository : Repository<Meeting>, IMeetingRepository
    {
        public MeetingRepository(DbContext context) : base(context)
        { }
        private ApplicationDbContext _appContext => (ApplicationDbContext)_context;


        public IEnumerable<Meeting> GetAll()
        {
            return _appContext.Meeting
                .OrderBy(m => m.BeginMeeting)
                .ToList();
        }

        public IEnumerable<Meeting> GetAllMeetingsByDate(DateTime date)
        {
            return _appContext.Meeting
                .Where(m =>  m.BeginMeeting.Date == date.Date)
                .OrderBy(m => m.BeginMeeting)
                .ToList();
        }
    }
}

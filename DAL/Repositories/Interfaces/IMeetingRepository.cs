using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories.Interfaces
{
    public interface IMeetingRepository : IRepository<Meeting>
    {
        public IEnumerable<Meeting> GetAllMeetingsByDate(DateTime date);
    }
}

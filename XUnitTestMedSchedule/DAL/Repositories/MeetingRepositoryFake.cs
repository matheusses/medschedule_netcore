using DAL.Models;
using DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace XUnitTestMedSchedule.DAL
{
    public class MeetingRepositoryFake : IMeetingRepository
    {

        private readonly List<Meeting> _meeting;

        public MeetingRepositoryFake()
        {
            _meeting = new List<Meeting>()
            {
                new Meeting() { Id = new Random(1000000).Next(),BeginMeeting = DateTime.Now,Birth = new DateTime(1990,7,1),EndMeeting = DateTime.Now.AddHours(1),Observation = "123", PatientName="P1" },
                new Meeting() { Id = new Random(1000000).Next(),BeginMeeting = DateTime.Now.AddDays(1),Birth = new DateTime(1992,7,1),EndMeeting = DateTime.Now.AddDays(1).AddHours(1),Observation = "1234", PatientName="P2" },
                new Meeting() { Id = new Random(1000000).Next(),BeginMeeting = DateTime.Now.AddDays(2),Birth = new DateTime(1980,3,1),EndMeeting = DateTime.Now.AddDays(2).AddHours(1),Observation = null, PatientName="P3" },
                
            };
        }

        public IEnumerable<Meeting> GetAllItems()
        {
            return _meeting;
        }

        public IEnumerable<Meeting> GetAllMeetingsByDate(DateTime date)
        {
            return _meeting.Where(x => x.BeginMeeting == date);
        }

        public void Add(Meeting entity)
        {
            entity.Id = new Random(1000000).Next();
            _meeting.Add(entity);
        }

        public void AddRange(IEnumerable<Meeting> entities)
        {
            throw new NotImplementedException();
        }

        public void Update(Meeting entity)
        {
            throw new NotImplementedException();
        }

        public void UpdateRange(IEnumerable<Meeting> entities)
        {
            throw new NotImplementedException();
        }

        public void Remove(Meeting entity)
        {
            _meeting.Remove(entity);
        }

        public void RemoveRange(IEnumerable<Meeting> entities)
        {
            throw new NotImplementedException();
        }

        public int Count()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Meeting> Find(Expression<Func<Meeting, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Meeting GetSingleOrDefault(Expression<Func<Meeting, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Meeting Get(int id)
        {
            return _meeting.First(a => a.Id == id);
        }

        public IEnumerable<Meeting> GetAll()
        {
            return _meeting;
        }
    }
}

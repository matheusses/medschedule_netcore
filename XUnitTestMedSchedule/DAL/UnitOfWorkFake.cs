using DAL;
using DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTestMedSchedule.DAL
{
    public class UnitOfWorkFake : IUnitOfWork
    {
        IMeetingRepository _meetings;

        public IMeetingRepository Meetings
        {
            get
            {
                if (_meetings == null)
                    _meetings = new MeetingRepositoryFake();

                return _meetings;
            }
        }

        public int SaveChanges()
        {
            return 1;
        }
    }
}

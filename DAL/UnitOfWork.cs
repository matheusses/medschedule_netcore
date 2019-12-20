
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Repositories;
using DAL.Repositories.Interfaces;

namespace DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        readonly ApplicationDbContext _context;

        IMeetingRepository _meetings;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IMeetingRepository Meetings
        {
            get
            {
                if (_meetings == null)
                    _meetings = new MeetingRepository(_context);

                return _meetings;
            }
        }


        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
    }
}

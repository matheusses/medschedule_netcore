
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Meeting 
    {
        public int Id { get; set; }
        public string PatientName { get; set; }
        public DateTime BeginMeeting { get; set; }
        public DateTime EndMeeting { get; set; }
        public string Observation { get; set; }
        public DateTime Birth { get; set; }
    }
}

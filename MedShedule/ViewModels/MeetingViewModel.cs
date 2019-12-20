using MedSchedule.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;


namespace MedSchedule.ViewModels
{
    public class MeetingViewModel
    {
        public int Id { get; set; }
        [Required]
        public string PatientName { get; set; }
        [Required]
        public DateTime BeginMeeting { get; set; }
        [Required]
        public DateTime Birth { get; set; }
        [Required]        
        public DateTime EndMeeting { get; set; }        
        public string Observation { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DAL;
using DAL.Models;
using MedSchedule.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedSchedule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetingController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public MeetingController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        // GET: api/Default
        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            var allmettings = _unitOfWork.Meetings.GetAll();
            return Ok(_mapper.Map<IEnumerable<MeetingViewModel>>(allmettings));
        }


        // GET: api/values
        [HttpGet("getByData/{data}")]
        public IActionResult Get(DateTime data)
        {
            try
            {
                var allmeetings = _unitOfWork.Meetings.GetAllMeetingsByDate(data);
                return Ok(_mapper.Map<IEnumerable<MeetingViewModel>>(allmeetings));
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Sorry. Contact support.");
            }
        }

        // GET: api/values
        [HttpGet("getById/{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var meeting = _unitOfWork.Meetings.Get(id);
                return Ok(_mapper.Map<MeetingViewModel>(meeting));
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Sorry. Contact support.");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(MeetingViewModel model)
        {
            try
            {                
                validationMeeting(model);
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var meeting_map = _mapper.Map<Meeting>(_mapper.Map<MeetingViewModel>(model));

                _unitOfWork.Meetings.Update(meeting_map);
                _unitOfWork.SaveChanges();
                var newmeeting = _mapper.Map<Meeting>(_mapper.Map<MeetingViewModel>(meeting_map));

                return Created($"api/getById/{model.Id}", newmeeting);

            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Sorry. Contact support.");
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> delete(int id)
        {
            try
            {
                var meeting = _unitOfWork.Meetings.Get(id);

                if (meeting == null)
                    return NotFound();

                _unitOfWork.Meetings.Remove(meeting);
                _unitOfWork.SaveChanges();
                return Ok();

            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(MeetingViewModel model)
        {
            try
            {
                correctDateZone(model);
                validationMeeting(model);
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var meeting = _mapper.Map<Meeting>(_mapper.Map<MeetingViewModel>(model));

                _unitOfWork.Meetings.Add(meeting);
                _unitOfWork.SaveChanges();
                var newmeeting = _mapper.Map<Meeting>(_mapper.Map<MeetingViewModel>(meeting));

                return Created($"api/getById/{meeting.Id}", newmeeting);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Sorry. Contact support.");
            }
        }

        private void validationMeeting(MeetingViewModel model)
        {

            if (model.BeginMeeting > model.EndMeeting)
                ModelState.AddModelError("endMeeting", "End Meeting must be greater then Begin.");

           if( _unitOfWork.Meetings.GetAll().Count(x => x.BeginMeeting <= model.BeginMeeting && model.EndMeeting <= x.EndMeeting && x.Id != model.Id) > 0)
                ModelState.AddModelError("beginMeeting", "There is meeting in this date.");
        }

        private void correctDateZone(MeetingViewModel model)
        {
            if(model.BeginMeeting != null)
                model.BeginMeeting = model.BeginMeeting.AddHours(-3);
            if (model.EndMeeting != null)
                model.EndMeeting = model.EndMeeting.AddHours(-3);
        }
    }
}

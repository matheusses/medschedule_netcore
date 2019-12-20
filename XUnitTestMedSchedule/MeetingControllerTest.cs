using AutoMapper;
using DAL;
using DAL.Models;
using MedSchedule.Controllers;
using MedSchedule.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Xunit;
using XUnitTestMedSchedule.DAL;

namespace XUnitTestMedSchedule
{
    public class MeetingControllerTest
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        MeetingController _controller;

        public MeetingControllerTest()
        {
            _unitOfWork = new UnitOfWorkFake();
            _mapper = new MapperConfiguration(c =>
                 c.AddProfile<AutoMapperProfile>()).CreateMapper();
            _controller = new MeetingController(_mapper, _unitOfWork);
        }

        [Fact]
        public void Get_WhenCalled_ReturnsOkResult()
        {
            // Act
            var okResult = _controller.GetAll();

            // Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void Get_WhenCalled_ReturnsAllItems()
        {
            // Act
            var okResult = _controller.GetAll() as OkObjectResult;

            // Assert
            var items = Assert.IsType<List<MeetingViewModel>>(okResult.Value);
            Assert.Equal(3, items.Count);
        }

        [Fact]
        public void GetById_UnknownGuidPassed_ReturnsNotFoundResult()
        {
            int unKnownId = 2109384;
            // Act
            var notFoundResult = _controller.Get(unKnownId);

            // Assert
            Assert.Equal(((ObjectResult)notFoundResult).StatusCode,500);
        }

        [Fact]
        public void GetById_ExistingGuidPassed_ReturnsOkResult()
        {
            // Arrange
            var testId = ((List<Meeting>)_unitOfWork.Meetings.GetAll())[0].Id;

            // Act
            var okResult = _controller.Get(testId);

            // Assert
            Assert.IsType<OkObjectResult>(okResult);
        }


        [Fact]
        public void Add_InvalidObjectPassedByIntervalDateWrong_ReturnsBadRequest()
        {
            // Arrange
            var intervalDateWrongItem = new MeetingViewModel()
            {
                BeginMeeting = DateTime.Now,
                EndMeeting = DateTime.Now.AddHours(-2),
                Birth = new DateTime(1970, 7, 1),
                PatientName = "P4"
            };

            // Act
            var badResponse = _controller.Post(intervalDateWrongItem);

            // Assert
            Assert.IsType<BadRequestObjectResult>(badResponse.Result);
        }

        [Fact]
        public void Add_InvalidObjectPassedByExistingMeeting_ReturnsBadRequest()
        {
            var meeting = ((List<Meeting>)_unitOfWork.Meetings.GetAll())[0];
            // Arrange
            var existingMeetingItem = new MeetingViewModel()
            {
                BeginMeeting = meeting.BeginMeeting.AddMinutes(10),
                EndMeeting = meeting.EndMeeting.AddMinutes(-10),
                Birth = new DateTime(1970, 7, 1),
                PatientName = "P5"
            };

            // Act
            var badResponse = _controller.Post(existingMeetingItem);

            // Assert
            Assert.IsType<BadRequestObjectResult>(badResponse.Result);
        }


        [Fact]
        public void Add_ValidObjectPassed_ReturnsOkResponse()
        {
            // Arrange
            MeetingViewModel testItem = new MeetingViewModel()
            {
                BeginMeeting = DateTime.Now.AddDays(3),
                EndMeeting = DateTime.Now.AddDays(3).AddHours(2),
                Birth = new DateTime(1970, 8, 1),
                PatientName = "P5"
            };

            // Act
            var createdResponse = _controller.Post(testItem);

            // Assert
            Assert.IsNotType<BadRequestObjectResult>(createdResponse);
        }

        [Fact]
        public void Update_InvalidObjectPassedByIntervalDateWrong_ReturnsBadRequest()
        {
            var intervalDateWrongItem = ((List<Meeting>)_unitOfWork.Meetings.GetAll())[0];
            var viewModel = _mapper.Map<MeetingViewModel>(_mapper.Map<Meeting>(intervalDateWrongItem));
            // Arrange
            viewModel.EndMeeting = viewModel.BeginMeeting.AddHours(-1);

            // Act
            var badResponse = _controller.Put(viewModel);

            // Assert
            Assert.IsType<BadRequestObjectResult>(badResponse.Result);
        }

        [Fact]
        public void Update_InvalidObjectPassedByExistingMeeting_ReturnsBadRequest()
        {
            var existingMeetingItem1 = ((List<Meeting>)_unitOfWork.Meetings.GetAll())[0];
            var existingMeetingItem2 = ((List<Meeting>)_unitOfWork.Meetings.GetAll())[1];

            var viewModel1 = _mapper.Map<MeetingViewModel>(_mapper.Map<Meeting>(existingMeetingItem1));
            var viewModel2 = _mapper.Map<MeetingViewModel>(_mapper.Map<Meeting>(existingMeetingItem2));
            // Arrange

            viewModel2.BeginMeeting = viewModel1.BeginMeeting.AddMinutes(10);

            // Act
            var badResponse = _controller.Put(viewModel2);

            // Assert
            Assert.IsType<BadRequestObjectResult>(badResponse.Result);
        }

        [Fact]
        public void Update_ValidObjectPassed_ReturnsOkResponse()
        {
            var testItem = ((List<Meeting>)_unitOfWork.Meetings.GetAll())[0];
            // Arrange
            var viewModel = _mapper.Map<MeetingViewModel>(_mapper.Map<Meeting>(testItem));
            viewModel.PatientName = "123";
            // Act
            var createdResponse = _controller.Put(viewModel);

            // Assert
            Assert.IsNotType<BadRequestObjectResult>(createdResponse);
        }

        [Fact]
        public void Remove_ValidObjectPassed_ReturnsOkResponse()
        {
            // Arrange;
            var testItem = ((List<Meeting>)_unitOfWork.Meetings.GetAll())[0];
        
            // Act
            var createdResponse = _controller.delete(testItem.Id);

            // Assert
            Assert.IsNotType<BadRequestObjectResult>(createdResponse);
        }

        [Fact]
        public void Remove_InvalidObjectPassed_ReturnsNotFoundResponse()
        {
            // Arrange;
            int id = 11234167;

            // Act
            var notFoundResponse = _controller.delete(id);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResponse.Result);
        }

    }
}

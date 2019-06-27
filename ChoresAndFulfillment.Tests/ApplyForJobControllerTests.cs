using System;
using ChoresAndFulfillment.Controllers;
using ChoresAndFulfillment.Web.Services.Interfaces;
using Moq;
using NUnit;
using NUnit.Framework;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using ChoresAndFulfillment.Models;
using System.Collections.Generic;
using ChoresAndFulfillment.Web.Data.ViewModels;
using ChoresAndFulfillment.Web.Data.BindModels;

namespace ChoresAndFulfillment.Tests
{
    [TestFixture]
    public class ApplyForJobControllerTests
    {
        [Test]
        public void ReturnsCorrectlyWhenJobIdIsInvalid()
        {
            Mock<IApplyForJobService> mockedService =
                new Mock<IApplyForJobService>();
            ApplyForJobBindModel applyForJobBindModel = new ApplyForJobBindModel()
            {
                Id = 0,
                JobApplicationMessage = "asdfasdfasdf"
            };
            mockedService.Setup(a => a.JobExists(applyForJobBindModel.Id)).Returns(false);
            mockedService.Setup(a => a.IsWorker()).Returns(true);
            ApplyForJobController applyForJobController =
            new ApplyForJobController(mockedService.Object);
            IActionResult result = applyForJobController.Apply(applyForJobBindModel);
            Assert.That(result is RedirectResult);

        }
        [Test]
        public void ReturnsCorrectlyWhenJobIsValidTriesToSignUpForJobButNotAWorker()
        {
            Mock<IApplyForJobService> mockedService =
                new Mock<IApplyForJobService>();
            ApplyForJobBindModel applyForJobBindModel = new ApplyForJobBindModel()
            {
                Id = 4,
                JobApplicationMessage = "hello. Please accept my application"
            };
            mockedService.Setup(a => a.JobExists(applyForJobBindModel.Id)).Returns(true);
            mockedService.Setup(a => a.IsWorker()).Returns(false);

            ApplyForJobController applyForJobController =
            new ApplyForJobController(mockedService.Object);
            IActionResult result = applyForJobController.Apply(applyForJobBindModel);
            Assert.That(result is RedirectResult);
        }
        [Test]
        public void ReturnsCorrectlyWhenWorkerHasAlreadySignedUpForJob()
        {
            Mock<IApplyForJobService> mockedService =
                new Mock<IApplyForJobService>();
            ApplyForJobBindModel applyForJobBindModel = new ApplyForJobBindModel()
            {
                Id = 4,
                JobApplicationMessage = "asdfasdfasdf"
            };
            mockedService.Setup(a => a.JobExists(applyForJobBindModel.Id)).Returns(true);
            mockedService.Setup(a => a.IsWorker()).Returns(true);
            mockedService.Setup(a => a.AlreadyAppliedForJob(applyForJobBindModel.Id)).Returns(true);
            ApplyForJobController applyForJobController =
            new ApplyForJobController(mockedService.Object);
            ViewResult result = (ViewResult)applyForJobController.Apply(applyForJobBindModel);
            Assert.That(result.ViewData["error"].ToString()== "You have already applied for this job!");
        }
        [Test]
        public void ReturnsCorrectlyWhenApplicationMessageIsInvalid()
        {
            Mock<IApplyForJobService> mockedService =
                new Mock<IApplyForJobService>();
            ApplyForJobBindModel applyForJobBindModel = new ApplyForJobBindModel()
            {
                Id = 4,
                JobApplicationMessage = ""
            };
            mockedService.Setup(a => a.JobExists(applyForJobBindModel.Id)).Returns(true);
            mockedService.Setup(a => a.IsWorker()).Returns(true);
            mockedService.Setup(a => a.AlreadyAppliedForJob(applyForJobBindModel.Id)).Returns(false);
            ApplyForJobController applyForJobController =
            new ApplyForJobController(mockedService.Object);
            ViewResult result = (ViewResult)applyForJobController.Apply(applyForJobBindModel);
            Assert.That(result.ViewData["error"].ToString() == "Job Application must be at least 5 symbols!");
        }

    }
}
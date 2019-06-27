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
using ChoresAndFulfillment.Data.BindModels;

namespace ChoresAndFulfillment.Tests
{
    [TestFixture]
    public class CreateJobControllerTests
    {
        [Test]
        public void ReturnsCorrectlyWhenViewerIsAWorker()
        {
            Mock<IUserAndContextRepository> mockedService =
                new Mock<IUserAndContextRepository>();
            CreateJobBindModel createJobBindModel = new CreateJobBindModel()
            {
                Description="asdfasdf",
                JobName="hello",
                Payment=123
            };
            mockedService.Setup(a => a.IsWorker()).Returns(true);
            CreateJobController createJobController =
            new CreateJobController(mockedService.Object);
            IActionResult result = createJobController.Index(createJobBindModel);
            Assert.That(result is RedirectResult);

        }
        [Test]
        public void ReturnsCorrectlyWhenJobPaymentIsZero()
        {
            Mock<IUserAndContextRepository> mockedService =
                            new Mock<IUserAndContextRepository>();
            CreateJobBindModel createJobBindModel = new CreateJobBindModel()
            {
                Description = "asdfasdf",
                JobName = "hello",
                Payment = 0
            };
            mockedService.Setup(a => a.IsWorker()).Returns(false);
            CreateJobController createJobController =
            new CreateJobController(mockedService.Object);
            ViewResult result = (ViewResult)createJobController.Index(createJobBindModel);
            Assert.That(result.ViewData["Error"].ToString()== "Invalid payment!");
        }
    }
}
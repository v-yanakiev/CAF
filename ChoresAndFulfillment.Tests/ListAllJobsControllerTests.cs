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

namespace ChoresAndFulfillment.Tests
{
    [TestFixture]
    public class ListAllJobsControllerTests
    {
        [Test]
        public void ReturnsCorrectlyWhenNoActiveJobsArePresentInDatabase()
        {
            Mock<IListAllJobsService> mockedService =
                new Mock<IListAllJobsService>();
            mockedService.Setup(a => a.AnyActiveJobs()).Returns(false);
            ListAllJobsController listAllJobsController = 
                new ListAllJobsController(mockedService.Object);
            ViewResult result = (ViewResult)listAllJobsController.Index();
            string toReturn = result.ViewData["Jobs"].ToString();
            Assert.That(toReturn== "<h1 style=\"text-align:center;\">No jobs found!</h1>");
        }
        [Test]
        public void ReturnsCorrectlyWhenEmployerViewsActiveJobs()
        {
            Mock<IListAllJobsService> mockedService =
            new Mock<IListAllJobsService>();
            mockedService.Setup(a => a.AnyActiveJobs()).Returns(true);
            mockedService.Setup(a => a.IsEmployer()).Returns(true);

            List<ActiveJobViewModel> activeJobs = this.GenerateJobs();
            mockedService.Setup(a => a.ViewAllActiveJobs()).Returns(activeJobs);
            ListAllJobsController listAllJobsController =
                new ListAllJobsController(mockedService.Object);

            ViewResult result = (ViewResult)listAllJobsController.Index();
            string toReturn = result.ViewData["Jobs"].ToString();
            Assert.That(
                (!toReturn.Contains("<th scope=\"col\">Apply for Job</th>")) &&
                (toReturn.Contains("<table class=\"table\"\">"))
                );
        }
        private List<ActiveJobViewModel> GenerateJobs()
        {
            return new List<ActiveJobViewModel>()
            {
                new ActiveJobViewModel()
                {
                    Name ="asdf",
                    Description ="23423f3f",
                    Id=1,
                    PayUponCompletion =500,
                    JobCreatorName ="asdf",
                    JobCreatorRating=null
                },
                new ActiveJobViewModel()
                {
                    Name ="agggf",
                    Description ="5t4i5jg4ig",
                    Id=3,
                    PayUponCompletion =400,
                    JobCreatorName ="asddf",
                    JobCreatorRating=null
                },
            };
        }
    }
}

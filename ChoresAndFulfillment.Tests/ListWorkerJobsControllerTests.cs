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
    public class ListWorkerJobsControllerTests
    {
        [Test]
        public void ReturnsCorrectlyWhenNonWorkerViewsPage()
        {
            Mock<IListWorkerJobsService> mockedService =
                new Mock<IListWorkerJobsService>();
            mockedService.Setup(a => a.IsWorker()).Returns(false);
            ListWorkerJobsController listWorkerJobsController =
                new ListWorkerJobsController(mockedService.Object);
            var result = listWorkerJobsController.Index();
            Assert.That(result is RedirectResult);
        }
        [Test]
        public void ReturnsCorrectlyWhenWorkerHasNoActiveJobs()
        {
            Mock<IListWorkerJobsService> mockedService =
                            new Mock<IListWorkerJobsService>();
            mockedService.Setup(a => a.IsWorker()).Returns(true);
            mockedService.Setup(a => a.HasActiveJobs()).Returns(false);
            ListWorkerJobsController listWorkerJobsController =
                new ListWorkerJobsController(mockedService.Object);
            var result = (ViewResult)listWorkerJobsController.Index();
            string toReturn = result.ViewData["Jobs"].ToString();
            Assert.That(toReturn== "<h2>You have no active jobs!</h2>");
        }
        [Test]
        public void ReturnsCorrectlyWhenWorkerHasActiveJobs()
        {
            Mock<IListWorkerJobsService> mockedService =
                            new Mock<IListWorkerJobsService>();
            mockedService.Setup(a => a.IsWorker()).Returns(true);
            mockedService.Setup(a => a.HasActiveJobs()).Returns(true);
            mockedService.Setup(a => a.ActiveJobs()).Returns(this.GenerateJobs());
            ListWorkerJobsController listWorkerJobsController =
                new ListWorkerJobsController(mockedService.Object);
            var result = (ViewResult)listWorkerJobsController.Index();
            string toReturn = result.ViewData["Jobs"].ToString();
            Assert.That(
                toReturn.Contains
                ("<a href=\"/WorkerActiveJob/Index/1\">asdf</a>") &&
                toReturn.Contains
                ("<a href=\"/WorkerActiveJob/Index/5\">agdfghrht</a>")
                );
        }

        private List<Job> GenerateJobs()
        {
            return new List<Job>()
            {
                new Job()
                {
                    Name ="asdf",
                    Description ="lkjhkflp",
                    Id=1,
                    PayUponCompletion =500,
                },
                new Job()
                {
                    Name ="agdfghrht",
                    Description ="2323444f3f",
                    Id=5,
                    PayUponCompletion =432
                }
            };
        }
    }
}

using System;
using ChoresAndFulfillment.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ChoresAndFulfillment.Models;

[assembly: HostingStartup(typeof(ChoresAndFulfillment.Areas.Identity.IdentityHostingStartup))]
namespace ChoresAndFulfillment.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}
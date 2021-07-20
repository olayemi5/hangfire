using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HangFire.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HangFireController : ControllerBase
    {
        [HttpPost]
        [Route("[action]")]
        public IActionResult Welcome() 
        {
            var jobId = BackgroundJob.Enqueue(() => SendWelcomeEmail("Welcome to our app api"));

            return Ok($"Job ID: {jobId} Welcome email sent to user!");
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult Discount()
        {
            int timeInSeconds = 15;
            var jobId = BackgroundJob.Schedule(() => SendWelcomeEmail("Discount voucher has been sent to user"), TimeSpan.FromSeconds(timeInSeconds));

            return Ok($"Job ID: {jobId} Discount voucher has been sent to user in {timeInSeconds} seconds!");
        }
        
       [HttpPost]
        [Route("[action]")]
        public IActionResult DatabaseUpdate()
        {
            RecurringJob.AddOrUpdate(() => Console.WriteLine("Data base Updated"), Cron.Minutely);

            return Ok($"Databse Update Initiated");
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult Confirm()
        {
            int timeInSeconds = 15;
            var parentJobId = BackgroundJob.Schedule(() => Console.WriteLine("You asked to be unsubscribe"), TimeSpan.FromSeconds(timeInSeconds));

            BackgroundJob.ContinueJobWith(parentJobId, () => Console.WriteLine("You have unsubscribe"));

            return Ok("Job Done");
        }

        public void SendWelcomeEmail(string text) 
        {
            Console.WriteLine(text);
        }
    }
}

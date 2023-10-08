using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using teste.service;

namespace teste.job;

public class Job
{
    public static void JobBook()
    {
        // RecurringJob.AddOrUpdate<BookServicePostGre>("Job",
        // x => x.JobBook(),
        // Cron.Minutely
        // );
    }
}

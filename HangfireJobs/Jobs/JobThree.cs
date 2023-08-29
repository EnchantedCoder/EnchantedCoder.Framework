﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EnchantedCoder.HangfireJobs.Jobs
{
    public class JobThree : IJobThree
    {
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine(this.GetType().Name + ": " + i);
                await Task.Delay(1000, cancellationToken);
            }
        }
    }
}
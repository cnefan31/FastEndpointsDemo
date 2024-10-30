// Copyright (c) 2024 Vanderlande Industries GmbH All rights reserved. 
//  
// The copyright to the computer program(s) herein is the property of 
// Vanderlande Industries. The program(s) may be used and/or copied 
// only with the written permission of the owner or in accordance with 
// the terms and conditions stipulated in the contract under which the 
// program(s) have been supplied. 
// File: RemoteWorkerJob.cs 
// History: 
//

namespace Serve;

internal class RemoteWorkerJob : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromSeconds(2));
        int i = 1;
        while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
        {
            CreateOrderCommand remoteCommand = new CreateOrderCommand();
            try
            {
                //var result = await remoteCommand.RemoteExecuteAsync();
                //Console.WriteLine(result.EventTime.ToString());
                new SomethingHappened
                {
                    Id = i,
                    Description = "I am a test event!"
                }.Broadcast(stoppingToken);
                i++;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}

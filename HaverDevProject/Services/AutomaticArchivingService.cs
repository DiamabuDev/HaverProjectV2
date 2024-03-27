using HaverDevProject.Configurations;
using HaverDevProject.Data;
using HaverDevProject.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HaverDevProject.Services
{
    public class AutomaticNcrArchivingService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly INumYearsService _numYearsService;

        public AutomaticNcrArchivingService(IServiceProvider serviceProvider, INumYearsService numYearsService)
        {
            _serviceProvider = serviceProvider;
            _numYearsService = numYearsService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var numOfYears = _numYearsService.NumOfYears;

                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<HaverNiagaraContext>();

                    try
                    {
                        var archiveStartDate = DateTime.Now.AddYears(-numOfYears);

                        var ncrsToArchive = dbContext.Ncrs
                            .Where(n => n.NcrPhase == NcrPhase.Closed && n.NcrPhase != NcrPhase.Archive &&
                                        n.NcrQa.NcrQacreationDate.Date < archiveStartDate)
                            .ToList();

                        foreach (var ncr in ncrsToArchive)
                        {
                            ncr.NcrPhase = NcrPhase.Archive;
                            dbContext.Update(ncr);
                        }

                        await dbContext.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        // Handle exception
                        Console.WriteLine($"Error occurred while archiving NCR objects: {ex.Message}");
                    }
                }

                // Wait for a specified interval before checking again
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }
    }
}
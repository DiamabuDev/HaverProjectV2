using HaverDevProject.Configurations;
using HaverDevProject.Data;
using HaverDevProject.Models;
using Microsoft.EntityFrameworkCore;
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
        private readonly ITargetYearService _targetYearService;

        public AutomaticNcrArchivingService(IServiceProvider serviceProvider, ITargetYearService targetYearService)
        {
            _serviceProvider = serviceProvider;
            _targetYearService = targetYearService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var targetYear = _targetYearService.TargetYear;

                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<HaverNiagaraContext>();

                    try
                    {
                        var ncrsToArchive = dbContext.Ncrs
                            .Where(n => n.NcrPhase == NcrPhase.Closed && n.NcrPhase != NcrPhase.Archive &&
                                        n.NcrQa.NcrQacreationDate.Year <= targetYear)
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
using HaverDevProject.Controllers;
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
        public AutomaticNcrArchivingService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Execute the archiving process after 5 years
                await ArchiveNcrsAfter5Years();

                // Wait for 24 hours before checking again
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }

        private async Task ArchiveNcrsAfter5Years()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<HaverNiagaraContext>();

                try
                {
                    int archiveYear = DateTime.Now.Year - 5;
                    var archiveStartDate = new DateTime(archiveYear, 1, 1);

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
        }
    }
}

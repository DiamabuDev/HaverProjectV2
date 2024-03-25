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
    public class NcrArchivingService
    {
        private readonly HaverNiagaraContext _context;

        public NcrArchivingService(HaverNiagaraContext context)
        {
            _context = context;
        }

        // Method triggered by user action to archive NCRs based on the specified number of years
        public async Task<int> ArchiveNcrsByYear(int yearsBeforeCurrentYear)
        {
            try
            {
                int archiveYear = DateTime.Now.Year - yearsBeforeCurrentYear;
                var archiveStartDate = new DateTime(archiveYear, 1, 1);

                var ncrsToArchive = _context.Ncrs
                    .Where(n => n.NcrPhase == NcrPhase.Closed && n.NcrPhase != NcrPhase.Archive &&
                                n.NcrQa.NcrQacreationDate.Date < archiveStartDate)
                    .ToList();

                foreach (var ncr in ncrsToArchive)
                {
                    ncr.NcrPhase = NcrPhase.Archive;
                    _context.Update(ncr);
                }

                await _context.SaveChangesAsync();

                return ncrsToArchive.Count;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred while archiving NCR objects: {ex.Message}", ex);
            }
        }
    }
}
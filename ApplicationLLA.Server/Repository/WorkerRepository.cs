using ApplicationLLA.Server.DBC;
using ApplicationLLA.Server.Dtos.Worker;
using ApplicationLLA.Server.Interfaces;
using ApplicationLLA.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace ApplicationLLA.Server.Repository
{
    public class WorkerRepository : IWorkerRepository
    {
        private readonly ApplicationDBContext _context;

        public WorkerRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<Worker?> GetByUserIdAsync(string id)
        {
            var worker = await _context.Workers.FirstOrDefaultAsync(i => i.AppUserId == id);
            if (worker == null) { return null; }
            return worker;
        }


        public async Task<Worker?> CreateAsync(string id)
        {
            var worker = new Worker
            {
                AppUserId = id,
                FullName = string.Empty,
                PhoneNumber = 5000000000L,
            };
            await _context.Workers.AddAsync(worker);
            await _context.SaveChangesAsync();

            if (worker == null) { return null; }

            return worker;
        }


        public async Task<Worker?> UpdateAsync(string id, UpdateWorkerDto workerDto)
        {
            if (workerDto == null) return null;

            var existsWorker = await _context.Workers.FirstOrDefaultAsync(x => x.AppUserId == id);
            if (existsWorker == null) return null;
            

            existsWorker.FullName = workerDto.FullName;
            existsWorker.PhoneNumber = workerDto.PhoneNumber;
            existsWorker.BirthDate = workerDto.BirthDate;
            existsWorker.Occupation = workerDto.Occupation;
            existsWorker.BirthDate = workerDto.BirthDate;
            existsWorker.Description = workerDto.Description;

            await _context.SaveChangesAsync();
            return existsWorker;
        }

        public async Task<bool> IsWorkerExistsAsync(string id)
        {
            return await _context.Workers.AnyAsync(x => x.AppUserId == id);
        }

        public async Task<Worker?> UpdateProfilePicAsync(string id, string createdImageName)
        {
            if(createdImageName == null) return null;

            var existsWorker = await _context.Workers.FirstOrDefaultAsync(x => x.AppUserId == id);
            if (existsWorker == null) return null;

            existsWorker.PictureLink = createdImageName;

            await _context.SaveChangesAsync();
            return existsWorker;
        }

        public async Task<bool> DeleteProfilePicAsync(string id)
        {
            var existsWorker = await _context.Workers.FirstOrDefaultAsync(x => x.AppUserId == id);
            if (existsWorker == null) return false;

            existsWorker.PictureLink = null;
            await _context.SaveChangesAsync();

            return (existsWorker.PictureLink == null);
        }

    }
}

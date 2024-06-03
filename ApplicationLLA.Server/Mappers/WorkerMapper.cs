using ApplicationLLA.Server.Dtos.Worker;
using ApplicationLLA.Server.Models;

namespace ApplicationLLA.Server.Mappers
{
    public static class WorkerMapper
    {
        public static WorkerDto ToWorkerDto(this Worker worker)
        {
            return new WorkerDto
            {
                WorkerId = worker.AppUserId,
                FullName = worker.FullName,
                PhoneNumber = worker.PhoneNumber,
                BirthDate = worker.BirthDate,
                Description = worker.Description,
                Occupation = worker.Occupation,
                PictureLink = "http://localhost:5279/resources/" + worker.PictureLink,
            };
        }

        public static Worker ToWorkerFromUpdate(this UpdateWorkerDto workerDto, string id)
        {
            return new Worker
            {
                AppUserId = id,
                FullName = workerDto.FullName,
                PhoneNumber = workerDto.PhoneNumber,
                BirthDate = workerDto.BirthDate,
                Description = workerDto.Description,
                Occupation = workerDto.Occupation,
            };
        }

    }
}

using AppointmentSystem.DTOs.Service;
using AppointmentSystem.Models;
using AppointmentSystem.Repositories.Interfaces;
using AppointmentSystem.Services.Interfaces;

namespace AppointmentSystem.Services
{
    public class ServiceService : IServiceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServiceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ServiceResponseDto>> GetAllAsync()
        {
            var services = await _unitOfWork.Services.GetAllAsync();
            return services.Select(MapToDto);
        }

        public async Task<ServiceResponseDto?> GetByIdAsync(int id)
        {
            var service = await _unitOfWork.Services.GetByIdAsync(id);
            if (service == null) return null;
            return MapToDto(service);
        }

        public async Task<ServiceResponseDto> CreateAsync(CreateServiceDto dto)
        {
            var service = new Service
            {
                Name = dto.Name,
                Description = dto.Description,
                DurationInMinutes = dto.DurationInMinutes
            };

            await _unitOfWork.Services.AddAsync(service);
            await _unitOfWork.SaveChangesAsync();
            return MapToDto(service);
        }

        public async Task<bool> UpdateAsync(int id, UpdateServiceDto dto)
        {
            var service = await _unitOfWork.Services.GetByIdAsync(id);
            if (service == null) return false;

            if (dto.Name != null) service.Name = dto.Name;
            if (dto.Description != null) service.Description = dto.Description;
            if (dto.DurationInMinutes.HasValue) service.DurationInMinutes = dto.DurationInMinutes.Value;

            service.UpdatedAt = DateTime.UtcNow;
            _unitOfWork.Services.Update(service);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var service = await _unitOfWork.Services.GetByIdAsync(id);
            if (service == null) return false;

            _unitOfWork.Services.Delete(service);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        private static ServiceResponseDto MapToDto(Service service) => new()
        {
            Id = service.Id,
            Name = service.Name,
            Description = service.Description,
            DurationInMinutes = service.DurationInMinutes
        };
    }
}
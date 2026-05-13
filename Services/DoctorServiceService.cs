// Services/DoctorServiceService.cs
using AppointmentSystem.DTOs.DoctorService;
using AppointmentSystem.Repositories.Interfaces;
using AppointmentSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using AppointmentSystem.Data;

namespace AppointmentSystem.Services
{
    public class DoctorServiceService : IDoctorServiceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _context;

        public DoctorServiceService(IUnitOfWork unitOfWork, ApplicationDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public async Task<IEnumerable<DoctorServiceResponseDto>> GetAllAsync()
        {
            return await _context.DoctorServices
                .Include(ds => ds.Doctor).ThenInclude(d => d.User)
                .Include(ds => ds.Service)
                .Select(ds => new DoctorServiceResponseDto
                {
                    Id = ds.Id,
                    DoctorId = ds.DoctorId,
                    DoctorName = ds.Doctor.User.FullName,
                    ServiceId = ds.ServiceId,
                    ServiceName = ds.Service.Name,
                    DurationInMinutes = ds.DurationInMinutes,
                    Price = ds.Price
                }).ToListAsync();
        }

        public async Task<IEnumerable<DoctorServiceResponseDto>> GetByDoctorIdAsync(int doctorId)
        {
            return await _context.DoctorServices
                .Include(ds => ds.Doctor).ThenInclude(d => d.User)
                .Include(ds => ds.Service)
                .Where(ds => ds.DoctorId == doctorId)
                .Select(ds => new DoctorServiceResponseDto
                {
                    Id = ds.Id,
                    DoctorId = ds.DoctorId,
                    DoctorName = ds.Doctor.User.FullName,
                    ServiceId = ds.ServiceId,
                    ServiceName = ds.Service.Name,
                    DurationInMinutes = ds.DurationInMinutes,
                    Price = ds.Price
                }).ToListAsync();
        }

        public async Task<DoctorServiceResponseDto?> GetByIdAsync(int id)
        {
            var ds = await _context.DoctorServices
                .Include(ds => ds.Doctor).ThenInclude(d => d.User)
                .Include(ds => ds.Service)
                .FirstOrDefaultAsync(ds => ds.Id == id);

            if (ds == null) return null;

            return new DoctorServiceResponseDto
            {
                Id = ds.Id,
                DoctorId = ds.DoctorId,
                DoctorName = ds.Doctor.User.FullName,
                ServiceId = ds.ServiceId,
                ServiceName = ds.Service.Name,
                DurationInMinutes = ds.DurationInMinutes,
                Price = ds.Price
            };
        }

        public async Task<DoctorServiceResponseDto> CreateAsync(CreateDoctorServiceDto dto)
        {
            // تحقق إن الدكتور موجود
            var doctor = await _unitOfWork.Doctors.GetByIdAsync(dto.DoctorId);
            if (doctor == null)
                throw new Exception("Doctor not found");

            // تحقق إن الـ Service موجودة
            var service = await _unitOfWork.Services.GetByIdAsync(dto.ServiceId);
            if (service == null)
                throw new Exception("Service not found");

            // تحقق إن هالـ DoctorService مو مضاف مسبقاً
            var exists = await _context.DoctorServices
                .AnyAsync(ds => ds.DoctorId == dto.DoctorId && ds.ServiceId == dto.ServiceId);
            if (exists)
                throw new Exception("This service is already assigned to this doctor");

            var doctorService = new Models.DoctorService
            {
                DoctorId = dto.DoctorId,
                ServiceId = dto.ServiceId,
                DurationInMinutes = dto.DurationInMinutes,
                Price = dto.Price
            };

            await _unitOfWork.DoctorServices.AddAsync(doctorService);
            await _unitOfWork.SaveChangesAsync();

            return (await GetByIdAsync(doctorService.Id))!;
        }

        public async Task<bool> UpdateAsync(int id, UpdateDoctorServiceDto dto)
        {
            var doctorService = await _unitOfWork.DoctorServices.GetByIdAsync(id);
            if (doctorService == null) return false;

            if (dto.DurationInMinutes.HasValue) doctorService.DurationInMinutes = dto.DurationInMinutes.Value;
            if (dto.Price.HasValue) doctorService.Price = dto.Price.Value;

            doctorService.UpdatedAt = DateTime.UtcNow;
            _unitOfWork.DoctorServices.Update(doctorService);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var doctorService = await _unitOfWork.DoctorServices.GetByIdAsync(id);
            if (doctorService == null) return false;

            _unitOfWork.DoctorServices.Delete(doctorService);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
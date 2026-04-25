using AppointmentSystem.DTOs.Doctor;
using AppointmentSystem.Models;
using AppointmentSystem.Repositories.Interfaces;
using AppointmentSystem.Services.Interfaces;

namespace AppointmentSystem.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DoctorService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<DoctorResponseDto>> GetAllDoctorsAsync()
        {
            var doctors = await _unitOfWork.Doctors.GetAllDoctorsWithDetailsAsync();
            return doctors.Select(MapToDto);
        }

        public async Task<DoctorResponseDto?> GetDoctorByIdAsync(int id)
        {
            var doctor = await _unitOfWork.Doctors.GetDoctorWithDetailsAsync(id);
            if (doctor == null) return null;
            return MapToDto(doctor);
        }

        public async Task<DoctorResponseDto> CreateDoctorAsync(CreateDoctorDto dto)
        {
            //   انشاء مستخدم جديد
            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Phone = dto.Phone,
                Address = dto.Address,
                RoleId = 2 // Doctor Role
            };
            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            //  انشاء طبيب مرتبط بالمستخدم
            var doctor = new Doctor
            {
                UserId = user.Id,
                ExperienceYears = dto.ExperienceYears
            };
            await _unitOfWork.Doctors.AddAsync(doctor);
            await _unitOfWork.SaveChangesAsync();

            // 3. اضافة التخصصات للطبيب
            foreach (var specialtyId in dto.SpecialtyIds)
            {
                await _unitOfWork.DoctorSpecialties.AddAsync(new DoctorSpecialty
                {
                    DoctorId = doctor.Id,
                    SpecialtyId = specialtyId
                });
            }
            await _unitOfWork.SaveChangesAsync();

            var created = await _unitOfWork.Doctors.GetDoctorWithDetailsAsync(doctor.Id);
            return MapToDto(created!);
        }

        public async Task<bool> UpdateDoctorAsync(int id, UpdateDoctorDto dto)
        {
            var doctor = await _unitOfWork.Doctors.GetDoctorWithDetailsAsync(id);
            if (doctor == null) return false;

            if (dto.FullName != null) doctor.User.FullName = dto.FullName;
            if (dto.Phone != null) doctor.User.Phone = dto.Phone;
            if (dto.Address != null) doctor.User.Address = dto.Address;
            if (dto.ExperienceYears.HasValue) doctor.ExperienceYears = dto.ExperienceYears.Value;

            doctor.User.UpdatedAt = DateTime.UtcNow;
            doctor.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Doctors.Update(doctor);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteDoctorAsync(int id)
        {
            var doctor = await _unitOfWork.Doctors.GetByIdAsync(id);
            if (doctor == null) return false;

            _unitOfWork.Doctors.Delete(doctor);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        //  Helper 
        private static DoctorResponseDto MapToDto(Doctor doctor) => new()
        {
            Id = doctor.Id,
            FullName = doctor.User.FullName,
            Email = doctor.User.Email,
            Phone = doctor.User.Phone,
            Address = doctor.User.Address,
            ExperienceYears = doctor.ExperienceYears,
            Specialties = doctor.DoctorSpecialties
                .Select(ds => ds.Specialty.Name)
                .ToList(),
            Services = doctor.DoctorServices.Select(ds => new DoctorServiceDto
            {
                ServiceId = ds.ServiceId,
                ServiceName = ds.Service.Name,
                DurationInMinutes = ds.DurationInMinutes,
                Price = ds.Price
            }).ToList()
        };
    }
}
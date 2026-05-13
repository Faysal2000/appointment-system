using AppointmentSystem.DTOs.Specialty;
using AppointmentSystem.Models;
using AppointmentSystem.Repositories.Interfaces;
using AppointmentSystem.Services.Interfaces;

namespace AppointmentSystem.Services
{
    public class SpecialtyService : ISpecialtyService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SpecialtyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<SpecialtyResponseDto>> GetAllAsync()
        {
            var specialties = await _unitOfWork.Specialties.GetAllAsync();
            return specialties.Select(s => new SpecialtyResponseDto
            {
                Id = s.Id,
                Name = s.Name
            });
        }

        public async Task<SpecialtyResponseDto?> GetByIdAsync(int id)
        {
            var specialty = await _unitOfWork.Specialties.GetByIdAsync(id);
            if (specialty == null) return null;
            return new SpecialtyResponseDto { Id = specialty.Id, Name = specialty.Name };
        }

        public async Task<SpecialtyResponseDto> CreateAsync(CreateSpecialtyDto dto)
        {
            var specialties = await _unitOfWork.Specialties.GetAllAsync();
            if (specialties.Any(s => s.Name.ToLower() == dto.Name.ToLower()))
                throw new Exception("Specialty already exists");

            var specialty = new Specialty { Name = dto.Name };
            await _unitOfWork.Specialties.AddAsync(specialty);
            await _unitOfWork.SaveChangesAsync();

            return new SpecialtyResponseDto { Id = specialty.Id, Name = specialty.Name };
        }

        public async Task<bool> UpdateAsync(int id, UpdateSpecialtyDto dto)
        {
            var specialty = await _unitOfWork.Specialties.GetByIdAsync(id);
            if (specialty == null) return false;

            specialty.Name = dto.Name;
            _unitOfWork.Specialties.Update(specialty);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var specialty = await _unitOfWork.Specialties.GetByIdAsync(id);
            if (specialty == null) return false;

            _unitOfWork.Specialties.Delete(specialty);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
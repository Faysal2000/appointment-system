using AppointmentSystem.DTOs.Patient;
using AppointmentSystem.Models;
using AppointmentSystem.Models.Enums;
using AppointmentSystem.Repositories.Interfaces;
using AppointmentSystem.Services.Interfaces;

namespace AppointmentSystem.Services
{
    public class PatientService : IPatientService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PatientService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<PatientResponseDto>> GetAllPatientsAsync()
        {
            var patients = await _unitOfWork.Patients.GetAllAsync();
            return patients.Select(MapToDto);
        }

        public async Task<PatientResponseDto?> GetPatientByIdAsync(int id)
        {
            var patient = await _unitOfWork.Patients.GetByIdAsync(id);
            if (patient == null) return null;
            return MapToDto(patient);
        }

        public async Task<PatientResponseDto> CreatePatientAsync(CreatePatientDto dto)
        {
            // Check if phone exists
            var patients = await _unitOfWork.Patients.GetAllAsync();
            if (!string.IsNullOrEmpty(dto.Phone) && patients.Any(p => p.Phone == dto.Phone))
                throw new Exception("Phone number already exists");

            var patient = new Patient
            {
                FullName = dto.FullName,
                DateOfBirth = dto.DateOfBirth,
                Gender = Enum.Parse<Gender>(dto.Gender, ignoreCase: true),
                Phone = dto.Phone
            };

            await _unitOfWork.Patients.AddAsync(patient);
            await _unitOfWork.SaveChangesAsync();

            return MapToDto(patient);
        }

        public async Task<bool> UpdatePatientAsync(int id, UpdatePatientDto dto)
        {
            var patient = await _unitOfWork.Patients.GetByIdAsync(id);
            if (patient == null) return false;

            if (dto.FullName != null) patient.FullName = dto.FullName;
            if (dto.DateOfBirth.HasValue) patient.DateOfBirth = dto.DateOfBirth.Value;
            if (dto.Phone != null) patient.Phone = dto.Phone;

            patient.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Patients.Update(patient);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePatientAsync(int id)
        {
            var patient = await _unitOfWork.Patients.GetByIdAsync(id);
            if (patient == null) return false;

            _unitOfWork.Patients.Delete(patient);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        private static PatientResponseDto MapToDto(Patient patient) => new()
        {
            Id = patient.Id,
            FullName = patient.FullName,
            DateOfBirth = patient.DateOfBirth,
            Gender = patient.Gender.ToString(),
            Phone = patient.Phone,
            CreatedAt = patient.CreatedAt
        };
    }
}
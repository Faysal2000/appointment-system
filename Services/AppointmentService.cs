using AppointmentSystem.DTOs.Appointment;
using AppointmentSystem.Models;
using AppointmentSystem.Repositories.Interfaces;
using AppointmentSystem.Services.Interfaces;

namespace AppointmentSystem.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AppointmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<AppointmentResponseDto>> GetAllAsync()
        {
            var appointments = await _unitOfWork.Appointments.GetAllAppointmentsWithDetailsAsync();
            return appointments.Select(MapToDto);
        }

        public async Task<AppointmentResponseDto?> GetByIdAsync(int id)
        {
            var appointment = await _unitOfWork.Appointments.GetAppointmentWithDetailsAsync(id);
            if (appointment == null) return null;
            return MapToDto(appointment);
        }

        public async Task<IEnumerable<AppointmentResponseDto>> GetByDoctorIdAsync(int doctorId)
        {
            var appointments = await _unitOfWork.Appointments.GetAppointmentsByDoctorIdAsync(doctorId);
            return appointments.Select(MapToDto);
        }

        public async Task<IEnumerable<AppointmentResponseDto>> GetByPatientIdAsync(int patientId)
        {
            var appointments = await _unitOfWork.Appointments.GetAppointmentsByPatientIdAsync(patientId);
            return appointments.Select(MapToDto);
        }

        public async Task<AppointmentResponseDto> CreateAsync(CreateAppointmentDto dto)
        {
            // 1. تحقق من وجود DoctorService
            var doctorService = await _unitOfWork.DoctorServices.GetByIdAsync(dto.DoctorServiceId);
            if (doctorService == null)
                throw new Exception("Doctor service not found");

            // 2. احسب EndTime بناءً على DurationInMinutes
            var endTime = dto.StartTime.AddMinutes(doctorService.DurationInMinutes);

            // 3. تحقق من توفر الوقت
            var isAvailable = await _unitOfWork.Appointments.IsTimeSlotAvailableAsync(
                dto.DoctorId, dto.AppointmentDate, dto.StartTime, endTime);

            if (!isAvailable)
                throw new Exception("This time slot is not available");

            // 4. أنشئ الموعد
            var appointment = new Appointment
            {
                DoctorId = dto.DoctorId,
                PatientId = dto.PatientId,
                DoctorServiceId = dto.DoctorServiceId,
                AppointmentDate = dto.AppointmentDate,
                StartTime = dto.StartTime,
                EndTime = endTime,
                Notes = dto.Notes
            };

            await _unitOfWork.Appointments.AddAsync(appointment);
            await _unitOfWork.SaveChangesAsync();

            var created = await _unitOfWork.Appointments.GetAppointmentWithDetailsAsync(appointment.Id);
            return MapToDto(created!);
        }

        public async Task<bool> UpdateAsync(int id, UpdateAppointmentDto dto)
        {
            var appointment = await _unitOfWork.Appointments.GetAppointmentWithDetailsAsync(id);
            if (appointment == null) return false;

            if (dto.AppointmentDate.HasValue)
                appointment.AppointmentDate = dto.AppointmentDate.Value;

            if (dto.StartTime.HasValue)
            {
                var endTime = dto.StartTime.Value.AddMinutes(
                    appointment.DoctorService.DurationInMinutes);

                var isAvailable = await _unitOfWork.Appointments.IsTimeSlotAvailableAsync(
                    appointment.DoctorId, appointment.AppointmentDate,
                    dto.StartTime.Value, endTime, id);

                if (!isAvailable)
                    throw new Exception("This time slot is not available");

                appointment.StartTime = dto.StartTime.Value;
                appointment.EndTime = endTime;
            }

            if (dto.Notes != null) appointment.Notes = dto.Notes;

            appointment.UpdatedAt = DateTime.UtcNow;
            _unitOfWork.Appointments.Update(appointment);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateStatusAsync(int id, UpdateAppointmentStatusDto dto)
        {
            var appointment = await _unitOfWork.Appointments.GetByIdAsync(id);
            if (appointment == null) return false;

            appointment.Status = dto.Status;
            appointment.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Appointments.Update(appointment);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var appointment = await _unitOfWork.Appointments.GetByIdAsync(id);
            if (appointment == null) return false;

            _unitOfWork.Appointments.Delete(appointment);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        private static AppointmentResponseDto MapToDto(Appointment a) => new()
        {
            Id = a.Id,
            DoctorName = a.Doctor.User.FullName,
            PatientName = a.Patient.FullName,
            ServiceName = a.DoctorService.Service.Name,
            Price = a.DoctorService.Price,
            AppointmentDate = a.AppointmentDate,
            StartTime = a.StartTime.ToString("HH:mm"),
            EndTime = a.EndTime.ToString("HH:mm"),
            Status = a.Status.ToString(),
            Notes = a.Notes
        };
    }
}
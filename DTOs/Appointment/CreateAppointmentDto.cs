namespace AppointmentSystem.DTOs.Appointment
{
    public class CreateAppointmentDto
    {
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public int DoctorServiceId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public string? Notes { get; set; }
    }
}
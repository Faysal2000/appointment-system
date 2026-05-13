namespace AppointmentSystem.DTOs.Appointment
{
    public class UpdateAppointmentDto
    {
        public DateTime? AppointmentDate { get; set; }
        public TimeOnly? StartTime { get; set; }
        public string? Notes { get; set; }
    }
}
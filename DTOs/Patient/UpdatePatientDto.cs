namespace AppointmentSystem.DTOs.Patient
{
    public class UpdatePatientDto
    {
        public string? FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Phone { get; set; }
    }
}
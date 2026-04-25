using AppointmentSystem.Data;
using AppointmentSystem.Models;
using AppointmentSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AppointmentSystem.Repositories
{
    public class DoctorRepository : GenericRepository<Doctor>, IDoctorRepository
    {
        public DoctorRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Doctor?> GetDoctorWithDetailsAsync(int id)
        {
            return await _context.Doctors
                .Include(d => d.User)
                .Include(d => d.DoctorSpecialties)
                    .ThenInclude(ds => ds.Specialty)
                .Include(d => d.DoctorServices)
                    .ThenInclude(ds => ds.Service)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<IEnumerable<Doctor>> GetAllDoctorsWithDetailsAsync()
        {
            return await _context.Doctors
                .Include(d => d.User)
                .Include(d => d.DoctorSpecialties)
                    .ThenInclude(ds => ds.Specialty)
                .Include(d => d.DoctorServices)
                    .ThenInclude(ds => ds.Service)
                .ToListAsync();
        }
    }
}
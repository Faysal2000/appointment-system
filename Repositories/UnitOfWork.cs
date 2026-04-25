using AppointmentSystem.Data;
using AppointmentSystem.Models;
using AppointmentSystem.Repositories.Interfaces;
using AppointmentSystem.Repositories;

namespace AppointmentSystem.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IDoctorRepository Doctors { get; private set; }
        public IGenericRepository<Patient> Patients { get; private set; }
        public IGenericRepository<Appointment> Appointments { get; private set; }
        public IGenericRepository<Invoice> Invoices { get; private set; }
        public IGenericRepository<Payment> Payments { get; private set; }
        public IGenericRepository<Service> Services { get; private set; }
        public IGenericRepository<Specialty> Specialties { get; private set; }
        public IGenericRepository<DoctorService> DoctorServices { get; private set; }
        public IGenericRepository<DoctorSchedule> DoctorSchedules { get; private set; }
        public IGenericRepository<User> Users { get; private set; }
        public IGenericRepository<Role> Roles { get; private set; }
        public IGenericRepository<DoctorSpecialty> DoctorSpecialties { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Doctors = new DoctorRepository(context);
            Patients = new GenericRepository<Patient>(context);
            Appointments = new GenericRepository<Appointment>(context);
            Invoices = new GenericRepository<Invoice>(context);
            Payments = new GenericRepository<Payment>(context);
            Services = new GenericRepository<Service>(context);
            Specialties = new GenericRepository<Specialty>(context);
            DoctorServices = new GenericRepository<DoctorService>(context);
            DoctorSchedules = new GenericRepository<DoctorSchedule>(context);
            Users = new GenericRepository<User>(context);
            Roles = new GenericRepository<Role>(context);
        }

        public async Task<int> SaveChangesAsync() =>
            await _context.SaveChangesAsync();

        public void Dispose() =>
            _context.Dispose();
    }
}
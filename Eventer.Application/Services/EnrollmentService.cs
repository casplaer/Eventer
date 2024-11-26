using Eventer.Application.Contracts.Enrollments;
using Eventer.Application.Interfaces.Repositories;
using Eventer.Application.Interfaces.Services;
using Eventer.Domain.Models;
using System.Net.Http;

namespace Eventer.Application.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EnrollmentService(IUnitOfWork unitOfWork, HttpClient httpClient)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task EnrollOnEventAsync(EnrollRequest request, Guid userId)
        {
            var registrationToCreate = new EventRegistration
            {
                Name = request.Name,
                Surname = request.SurName,
                Email = request.Email,
                EventId = request.EventId,
                UserId = userId,
                RegistrationDate = DateTime.UtcNow,
                DateOfBirth = request.DateOfBirth
            };

            var eventToEnrollOn = await _unitOfWork.Events.GetByIdAsync(request.EventId);
            eventToEnrollOn!.Registrations.Add(registrationToCreate);

            await _unitOfWork.Events.UpdateAsync(eventToEnrollOn);

            var userToEnroll = await _unitOfWork.Users.GetByIdAsync(userId);
            if (userToEnroll.EventRegistrations == null)
            {
                userToEnroll.EventRegistrations = [];
            }
            userToEnroll.EventRegistrations.Add(registrationToCreate);

            await _unitOfWork.Users.UpdateAsync(userToEnroll);

            await _unitOfWork.Registrations.AddAsync(registrationToCreate);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<Guid> IsUserEnrolledAsync(Guid eventId, Guid userId)
        {
            var enrollment = (await _unitOfWork.Registrations.GetAllAsync()).FirstOrDefault(r => r.EventId == eventId && r.UserId == userId);
            if (enrollment == null)
                throw new Exception();

            return enrollment.Id;
        }

        public async Task UpdateEnrollmentAsync(UpdateEnrollRequest request)
        {
            var enrollmentToUpdate = await _unitOfWork.Registrations.GetByIdAsync(request.EnrollmentId);
            if (enrollmentToUpdate == null)
                throw new Exception("No enrollment to update.");

            if (!string.IsNullOrEmpty(request.Name))
                enrollmentToUpdate.Name = request.Name;
            else throw new Exception("Некорректное значение имени.");

            if (!string.IsNullOrEmpty(request.Surname))
                enrollmentToUpdate.Surname = request.Surname;
            else throw new Exception("Некорректное значение фамилии."); ;

            enrollmentToUpdate.DateOfBirth = request.DateOfBirth;

            await _unitOfWork.Registrations.UpdateAsync(enrollmentToUpdate);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<SingleEnrollmentResponse?> GetSingleEnrollmentById(Guid id)
        {
            var enrollment = await _unitOfWork.Registrations.GetByIdAsync(id);
            if (enrollment == null)
            {
                return null;
            }
            return new SingleEnrollmentResponse(
                enrollment.Id,
                enrollment.EventId,
                enrollment.Name,
                enrollment.Surname,
                enrollment.Email,
                enrollment.DateOfBirth
                );
        }

        public async Task<bool> DeleteEnrollmentAsync(Guid id)
        {
            var enrollment = await _unitOfWork.Registrations.GetByIdAsync(id);

            if (enrollment == null)
            {
                return false;
            }

            await _unitOfWork.Registrations.DeleteAsync(id);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }


        public async Task<IEnumerable<EventRegistration>> GetAllEnrollmentsAsync(Guid eventId)
        {
            var eventWithRegistrations = await _unitOfWork.Events.GetByIdAsync(eventId);

            if (eventWithRegistrations == null)
            {
                throw new KeyNotFoundException($"Event with ID {eventId} not found.");
            }

            return eventWithRegistrations.Registrations;
        }
    }
}

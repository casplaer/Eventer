namespace Eventer.Contracts.Responses.Enrollments
{
    public record SingleEnrollmentResponse
    {
        public Guid EnrollmentId { get; set; }
        public Guid EventId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public DateOnly DateOfBirth { get; set; }
    }
}

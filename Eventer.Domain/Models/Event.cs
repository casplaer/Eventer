namespace Eventer.Domain.Models
{
    public class Event
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty; //Название
        public string Description { get; set; } = string.Empty; //Описание
        
        public DateOnly StartDate { get; set; } //Дата проведения
        public TimeOnly StartTime { get; set; } //Время проведения
        
        public string Venue {  get; set; } = string.Empty; //Место проведения
        
        public EventCategory Category { get; set; } //Категория

        public int MaxParticipants { get; set; } //Максимальное количество участников
        public List<EventRegistration> Registrations { get; set; } = []; //Список участников

        public List<string>? ImageURLs { get; set; } //Пути к изображениям
    }
}

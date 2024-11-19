using Eventer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventer.Application.Contracts
{
    public record CreateEventRequest(
        string Title,
        string Description,
        DateOnly StartDate,
        TimeOnly StartTime, 
        string Venue,
        double Latitude, 
        double Longitude, 
        EventCategory Category, 
        int MaxParticipants);
}

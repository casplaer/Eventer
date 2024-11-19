using Eventer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventer.Application.Contracts
{
    public record GetEventsRequest(
        DateOnly? Date,          
        string? Venue,          
        EventCategory? Category
        );
}

using System;
using webapi.Domain.Models;

namespace webapi.Domain.Helpers
{
    public static class EscalationTimerExtensions
    {
        public static TimeSpan ToTimeSpan(this HdEscalationTimers timer)
        {
            int days = timer.Days ?? 0;
            int hours = timer.Hours ?? 0;
            int minutes = timer.Minutes ?? 0;
            return new TimeSpan(days, hours, minutes, 0);
        }
    }
}

using System;
using System.Collections.Generic;

namespace AirlineReservationSystem.Services
{
    public class NotificationService
    {
        public delegate void NotificationHandler(string message);
        public event NotificationHandler? NotifyAdmin;

        private readonly List<string> _notifications = new List<string>();

        public void AddNotification(string message)
        {
            _notifications.Add(message);
            NotifyAdmin?.Invoke(message);
        }

        public List<string> GetNotifications()
        {
            return new List<string>(_notifications);
        }

        public void ClearNotifications()
        {
            _notifications.Clear();
        }
    }
}
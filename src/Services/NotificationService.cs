using System;
using System.Collections.Generic;

namespace AirlineReservationSystem.Services
{
    public class NotificationService
    {
        public delegate void NotificationHandler(string message);
        public event NotificationHandler? NotifyAdmin;
        public event NotificationHandler? NotifyUser;

        private readonly List<string> _adminNotifications = new List<string>();
        private readonly List<string> _userNotifications = new List<string>();

        public void AddAdminNotification(string message)
        {
            _adminNotifications.Add(message);
            NotifyAdmin?.Invoke(message);
        }

        public void AddUserNotification(string message)
        {
            _userNotifications.Add(message);
            NotifyUser?.Invoke(message);
        }

        public List<string> GetAdminNotifications()
        {
            return new List<string>(_adminNotifications);
        }

        public List<string> GetUserNotifications()
        {
            return new List<string>(_userNotifications);
        }

        public void ClearAdminNotifications()
        {
            _adminNotifications.Clear();
        }

        public void ClearUserNotifications()
        {
            _userNotifications.Clear();
        }
    }
}
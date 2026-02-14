using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WorkoutManagerNoam.Models
{
    public class ObservableUser : INotifyPropertyChanged
    {
        private User _user;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableUser(User user)
        {
            _user = user;
        }

        public User User
        {
            get { return _user; }
        }

        public int Id
        {
            get { return _user.Id; }
            set
            {
                if (_user.Id != value)
                {
                    _user.Id = value;
                    OnPropertyChanged();
                }
            }
        }

        public string? FirstName
        {
            get { return _user.FirstName; }
            set
            {
                if (_user.FirstName != value)
                {
                    _user.FirstName = value;
                    OnPropertyChanged();
                }
            }
        }

        public string? LastName
        {
            get { return _user.LastName; }
            set
            {
                if (_user.LastName != value)
                {
                    _user.LastName = value;
                    OnPropertyChanged();
                }
            }
        }

        public string? UEmail
        {
            get { return _user.UEmail; }
            set
            {
                if (_user.UEmail != value)
                {
                    _user.UEmail = value;
                    OnPropertyChanged();
                }
            }
        }

        public string? UMobile
        {
            get { return _user.UMobile; }
            set
            {
                if (_user.UMobile != value)
                {
                    _user.UMobile = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime RegDate
        {
            get { return _user.RegDate; }
            set
            {
                if (_user.RegDate != value)
                {
                    _user.RegDate = value;
                    OnPropertyChanged();
                }
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

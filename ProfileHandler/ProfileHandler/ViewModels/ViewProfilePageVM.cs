using Xamarin.Forms;
using ProfileHandler.Models;
using System.Threading.Tasks;
using System;
using System.Diagnostics;
using Xamarin.Essentials;
using Plugin.Media;
using Plugin.Media.Abstractions;

namespace ProfileHandler.ViewModels
{
    public class ViewProfilePageVM : BindableObject
    {
        #region Constructor

        public ViewProfilePageVM(User user)
        {
            this.User = user;
        }

        #endregion

        #region Private & Public Properties
        private string _profilePicture;
        public string ProfilePicture
        {
            get => _profilePicture;
            set
            {
                if (value == _profilePicture)
                    return;
                _profilePicture = value;
                OnPropertyChanged(nameof(ProfilePicture));

            }
        }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (value == _name)
                    return;
                _name = value;
                OnPropertyChanged(nameof(Name));

            }
        }

        private string _sex;
        public string Sex
        {
            get => _sex;
            set
            {
                if (value == _sex)
                    return;
                _sex = value;
                OnPropertyChanged(nameof(Sex));

            }
        }


        private string _address;
        public string Address
        {
            get => _address;
            set
            {
                if (value == _address)
                    return;
                _address = value;
                OnPropertyChanged(nameof(Address));

            }
        }

        private string _mobile;
        public string Mobile
        {
            get => _mobile;
            set
            {
                if (value == _mobile)
                    return;
                _mobile = value;
                OnPropertyChanged(nameof(Mobile));

            }
        }

        private User _user;
        public User User
        {
            get => _user;
            set
            {
                if (value == _user)
                    return;
                _user = value;
                OnPropertyChanged(nameof(User));

            }
        }


        private MediaFile _mediaFile;
        public MediaFile MediaFile
        {
            get => _mediaFile;
            set
            {
                if (value == _mediaFile)
                    return;
                _mediaFile = value;
                OnPropertyChanged(nameof(MediaFile));

            }
        }

        private string _buttonText;
        public string ButtonText
        {
            get => _buttonText;
            set
            {
                if (value == _buttonText)
                    return;
                _buttonText = value;
                OnPropertyChanged(nameof(ButtonText));

            }
        }

        #endregion

        #region Commands
        public Command CloseCommand => new Command(async () => await CloseProfile());

        #endregion

        #region Methods


        private async Task CloseProfile()
        {
            try
            {
                await App.Current.MainPage.Navigation.PopAsync(); 
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Failed", "We are unable to perform action, please try again later.", "Ok");
                Debug.WriteLine($"error  in CreateProfile {ex.Message}");
            }
        }

        
        #endregion
    }
}

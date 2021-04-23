using ProfileHandler.Models;
using System.Threading.Tasks;
using System;
using System.Diagnostics;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace ProfileHandler.ViewModels
{
    public class ProfileListPageVM : BindableObject
    {
        #region Constructor

        public ProfileListPageVM()
        {
            MessagingCenter.Unsubscribe<App>((App)Application.Current, "RefreshList");
            MessagingCenter.Subscribe<App>((App)Application.Current, "RefreshList", (sender) =>
            {
                FillProfiles();
            });
            FillProfiles();
        }

        #endregion

        #region Private & Public Properties

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

        private ObservableCollection<User> _users;
        public ObservableCollection<User> Users
        {
            get => _users;
            set
            {
                if (value == _users)
                    return;
                _users = value;
                OnPropertyChanged(nameof(Users));

            }
        }
        #endregion

        #region Commands
        public Command<User> EditProfileCommand => new Command<User>(async (user) => await EditProfile(user));
        public Command<User> ViewProfileCommand => new Command<User>(async (user) => await ViewProfile(user));

        #endregion

        #region Methods
        private async void FillProfiles()
        {
            try
            {
                Users = new ObservableCollection<User>(await App.SqlDataManager.GetUser());
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Failed", "We are unable to perform action, please try again later.", "Ok");
                Debug.WriteLine($"error  in CreateProfile {ex.Message}");
            }
        }

        private async Task EditProfile(User user)
        {
            try
            {
                await App.Current.MainPage.Navigation.PushAsync(new Views.UpdateProfilePage(user));

            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Failed", "We are unable to perform action, please try again later.", "Ok");
                Debug.WriteLine($"error  in EditProfile {ex.Message}");
            }
        }

        private async Task ViewProfile(User user)
        {
            try
            {
                await App.Current.MainPage.Navigation.PushAsync(new Views.ViewProfilePage(user));

            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Failed", "We are unable to perform action, please try again later.", "Ok");
                Debug.WriteLine($"error  in EditProfile {ex.Message}");
            }
        }
        #endregion
    }
}

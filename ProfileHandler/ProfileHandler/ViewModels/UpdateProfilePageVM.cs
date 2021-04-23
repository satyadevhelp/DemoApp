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
    public class UpdateProfilePageVM : BindableObject
    {
        #region Constructor

        public UpdateProfilePageVM(User user)
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
        public Command UpdateProfileCommand => new Command(async () => await UpdateProfile());

        public Command SelectOrChoosePhotoCommand => new Command(async () => await SelectOrChoosePhoto());
        #endregion

        #region Methods


        private async Task UpdateProfile()
        {
            try
            {
                if (await CheckFormValidation())
                {
                    if (await App.SqlDataManager.SaveOrUpdateUser(User) > 0)
                    {
                        await App.Current.MainPage.DisplayAlert("", "Profile updated successfully.", "Ok");
                    }
                    MessagingCenter.Send<App>((App)Application.Current, "RefreshList");
                    await App.Current.MainPage.Navigation.PopAsync();
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Failed", "We are unable to perform action, please try again later.", "Ok");
                Debug.WriteLine($"error  in CreateProfile {ex.Message}");
            }
        }

        private async Task SelectOrChoosePhoto()
        {
            try
            {
                if (!await CheckPermissions())
                {
                    await App.Current.MainPage.DisplayAlert("Permissions Required", "App required Camera and Storage pemissions.", "Ok");
                    return;
                }
                if (await App.Current.MainPage.DisplayActionSheet("Select or Click a photo", "Camera", "Gallery") == "Camera")
                {
                    MediaFile = await OpenCamera();
                }
                else
                {
                    MediaFile = await OpenGallery();
                }
                if (MediaFile != null)
                    User.ProfilePicture = MediaFile.Path;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Failed", "We are unable to perform action, please try again later.", "Ok");
                Debug.WriteLine($"error  in CreateProfile {ex.Message}");
            }
        }

        //Check Camera permissions 
        private async Task<bool> CheckPermissions()
        {
            try
            {
                var cameraStatus = await Permissions.CheckStatusAsync<Permissions.Camera>();
                while (!cameraStatus.Equals(PermissionStatus.Granted))
                {
                    cameraStatus = await Permissions.RequestAsync<Permissions.Camera>();
                }
                var storageReadStatus = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
                while (!storageReadStatus.Equals(PermissionStatus.Granted))
                {
                    storageReadStatus = await Permissions.RequestAsync<Permissions.StorageRead>();
                }
                var storageWriteStatus = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();
                while (!storageWriteStatus.Equals(PermissionStatus.Granted))
                {
                    storageWriteStatus = await Permissions.RequestAsync<Permissions.StorageWrite>();
                }

                if (cameraStatus.Equals(PermissionStatus.Granted)
                    && storageReadStatus.Equals(PermissionStatus.Granted)
                    && storageWriteStatus.Equals(PermissionStatus.Granted))
                {
                    return true;
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Failed", "Camera & Storage permissions are required.", "Ok");
                }
            }
            catch (Exception Ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", Ex.Message, "Ok");
            }
            return false;
        }

        //Check Form Validation 
        private async Task<bool> CheckFormValidation()
        {
            try
            {
                string msg = string.Empty;
                if (string.IsNullOrWhiteSpace(User.Name))
                    msg += "\n* Name required.";
                if (string.IsNullOrWhiteSpace(User.Sex))
                    msg += "\n* Sex type required.";
                if (string.IsNullOrWhiteSpace(User.Address))
                    msg += "\n* Address required.";
                if (string.IsNullOrWhiteSpace(User.Mobile))
                    msg += "\n* Mobile required.";
                if (string.IsNullOrWhiteSpace(User.ProfilePicture))
                    msg += "\n* Profile picture required.";
                if (msg == string.Empty) return true;
                else
                {
                    await App.Current.MainPage.DisplayAlert("Failed", msg, "Ok");
                    return false;
                }
            }
            catch (Exception Ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", Ex.Message, "Ok");
            }
            return false;
        }

        //Select photo from Galllery
        private async Task<MediaFile> OpenGallery()
        {
            try
            {
                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await App.Current.MainPage.DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
                    return null;
                }
                return await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {
                    PhotoSize = PhotoSize.Small,

                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Click photo and save in Gallery
        private async Task<MediaFile> OpenCamera()
        {
            try
            {
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await App.Current.MainPage.DisplayAlert("No Camera", ":( No camera available.", "OK");
                    return null;
                }
                return await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    Directory = "Test",
                    SaveToAlbum = true,
                    CompressionQuality = 75,
                    CustomPhotoSize = 50,
                    PhotoSize = PhotoSize.MaxWidthHeight,
                    MaxWidthHeight = 2000,
                    DefaultCamera = CameraDevice.Front
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}

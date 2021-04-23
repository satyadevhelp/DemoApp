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
    public class AddProfilePageVM : BindableObject
    {
        #region Constructor

        public AddProfilePageVM()
        {

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


        #endregion

        #region Commands
        public Command CreateProfileCommand => new Command(async () => await CreateProfile());

        public Command SelectOrChoosePhotoCommand => new Command(async () => await SelectOrChoosePhoto());
        #endregion

        #region Methods
        private async Task CreateProfile()
        {
            try
            {
                if (await CheckFormValidation())
                {
                    var user = new User
                    {
                        Name = Name,
                        Address = Address,
                        Mobile = Mobile,
                        ProfilePicture = ProfilePicture,
                        Sex = Sex
                    };
                    if (await App.SqlDataManager.SaveOrUpdateUser(user) > 0)
                    {
                        ClearFields();
                        MessagingCenter.Send<App>((App)Application.Current, "RefreshList");
                        await App.Current.MainPage.DisplayAlert("", "Profile saved successfully.", "Ok");
                    }

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
                    ProfilePicture = MediaFile.Path;
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
                if (string.IsNullOrWhiteSpace(Name))
                    msg += "\n* Name required.";
                if (string.IsNullOrWhiteSpace(Sex))
                    msg += "\n* Sex type required.";
                if (string.IsNullOrWhiteSpace(Address))
                    msg += "\n* Address required.";
                if (string.IsNullOrWhiteSpace(Mobile))
                    msg += "\n* Mobile required.";
                if (string.IsNullOrWhiteSpace(ProfilePicture))
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

        //Clear all fields
        private void ClearFields()
        {
            Name = string.Empty;
            Sex = string.Empty;
            Address = string.Empty;
            Mobile = string.Empty;
            ProfilePicture = string.Empty;
            ProfilePicture = null;
        }
        #endregion
    }
}

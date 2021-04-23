

using ProfileHandler.ViewModels;
using ProfileHandler.Views;
using Xamarin.Forms;

namespace ProfileHandler
{
    public partial class App : Application
    {
        static Data.SqlDataManager database;


        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new Views.MainTabbedPage());
        }


        public static Data.SqlDataManager SqlDataManager
        {
            get
            {
                if (database == null)
                {
                    database = new Data.SqlDataManager();
                }
                return database;
            }
        }


        //protected override void RegisterTypes(IContainerRegistry containerRegistry)
        //{
        //    containerRegistry.RegisterForNavigation<MainTabbedPage, MainTabbedPageVM>();
        //    containerRegistry.RegisterForNavigation<AddProfilePage, AddProfilePageVM>();
        //    containerRegistry.RegisterForNavigation<ProfileListPage, ProfileListPageVM>();
        //    containerRegistry.RegisterForNavigation<ViewProfilePage, ViewProfilePageVM>();
        //    containerRegistry.RegisterForNavigation<UpdateProfilePage, UpdateProfilePageVM>();
        //}

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

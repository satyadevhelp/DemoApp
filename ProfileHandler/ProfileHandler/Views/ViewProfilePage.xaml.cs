
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProfileHandler.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewProfilePage : ContentPage
    {
        public ViewProfilePage(Models.User user)
        {
            InitializeComponent();
            this.BindingContext = new ViewModels.ViewProfilePageVM(user);
        }
    }
}
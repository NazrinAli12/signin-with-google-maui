using signinwithgoogle.GoogleAuth;

namespace signinwithgoogle
{
    public partial class MainPage : ContentPage
    {
		private readonly IGoogleAuthService _googleAuthService;

		public MainPage(IGoogleAuthService googleAuthService)
		{
			InitializeComponent();
			_googleAuthService = googleAuthService;

		}

		async void Button_Clicked(System.Object sender, System.EventArgs e)
		{
			var loggedUser = await _googleAuthService.GetCurrentUserAsync();

			if (loggedUser == null)
			{
				loggedUser = await _googleAuthService.AuthenticateAsync();
			}

			await Application.Current.MainPage.DisplayAlert("Login Message", "Welcome " + loggedUser.FullName, "Ok");
		}

		async void Button_Clicked_1(System.Object sender, System.EventArgs e)
		{
			await _googleAuthService?.LogoutAsync();

			await Application.Current.MainPage.DisplayAlert("Login Message", "Goodbye", "Ok");

		}
	}

}

using Android.App;
using Android.Gms.Auth.Api.SignIn;
using Android.Widget;
using signinwithgoogle.GoogleAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace signinwithgoogle.Platforms.Android
{
	public class GoogleAuthService:IGoogleAuthService
	{
		public static Activity _activity;
		public static GoogleSignInOptions _gso;
		public static GoogleSignInClient _googleSignInClient;

		private TaskCompletionSource<GoogleUserModel> _taskCompletionSource = new();
		private Task<GoogleUserModel> GoogleAuthentication
		{
			get => _taskCompletionSource.Task;
		}

		public GoogleAuthService()
		{
			_activity = Platform.CurrentActivity;

			//Google Auth Option
			_gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
							.RequestIdToken("YOUR_CLIENT_ID_HERE.apps.googleusercontent.com")
							.RequestEmail()
							.RequestId()
							.RequestProfile()
							.Build();

			_googleSignInClient = GoogleSignIn.GetClient(_activity, _gso);


			MainActivity.ResultGoogleAuth += MainActivity_ResultGoogleAuth;
		}

		private void MainActivity_ResultGoogleAuth(object sender, (bool Success, GoogleSignInAccount Account) e)
		{
			if (e.Success)
			{
				try
				{
					var currentAccount = e.Account;
					Toast.MakeText(Platform.AppContext, $"{currentAccount.DisplayName}", ToastLength.Long).Show();
					_taskCompletionSource.SetResult(
						new GoogleUserModel
						{
							Email = currentAccount.Email,
							FullName = currentAccount.DisplayName,
							TokenId = currentAccount.IdToken,
							UserName = currentAccount.GivenName,
						});
				}
				catch (Exception ex)
				{
					_taskCompletionSource.SetException(ex);
				}
			}

		}

		public Task<GoogleUserModel> AuthenticateAsync()
		{
			_taskCompletionSource = new TaskCompletionSource<GoogleUserModel>();

			_activity.StartActivityForResult(_googleSignInClient.SignInIntent, 9001);

			return GoogleAuthentication;
		}

		public async Task<GoogleUserModel> GetCurrentUserAsync()
		{
			try
			{
				var user = await _googleSignInClient.SilentSignInAsync();
				return new GoogleUserModel
				{
					Email = user.Email,
					FullName = $"{user.DisplayName}",
					TokenId = user.IdToken,
					UserName = user.GivenName
				};

			}
			catch (Exception)
			{
				return null;
			}
		}

		public Task LogoutAsync() => _googleSignInClient.SignOutAsync();
	}
}

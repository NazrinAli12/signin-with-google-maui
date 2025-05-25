using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace signinwithgoogle.GoogleAuth
{
	public interface IGoogleAuthService
	{
		public Task<GoogleUserModel> AuthenticateAsync();
		public Task<GoogleUserModel> GetCurrentUserAsync();
		public Task LogoutAsync();
	}
}

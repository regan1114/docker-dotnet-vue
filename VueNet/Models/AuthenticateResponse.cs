using System;
using System.Text.Json.Serialization;

namespace VueNet.Models
{
	public class AuthenticateResponse
	{
        public int Id { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }

        public AuthenticateResponse(UserModel user)
        {
            Id = user.Id;
            Username = user.Username;
            Token = user.Token;
        }
    }
}


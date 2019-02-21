using System.Collections.Generic;
using System.Security.Claims;
using IdentityServer4.Test;

namespace AccessTokenServer.Configuration
{
    public class TestUsers
    {
        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId="1",                    
                    IsActive=true,
                    Password="password",
                    Username="Opossum",
                    Claims=
                    {
                        new Claim(ClaimTypes.Name, "Opossum"),
                        new Claim(ClaimTypes.GivenName, "Admin")
                    }
                }
            };
        }
    }
}
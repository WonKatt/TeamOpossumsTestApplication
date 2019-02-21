using System.Collections.Generic;
using IdentityServer4.Models;

namespace AccessTokenServer.Configuration
{
    public class Clients
    {
        public static List<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                  ClientId = "opossums", //будет идентифицировать тип клиента и какой доступ у него есть
                  AllowedGrantTypes = GrantTypes.ResourceOwnerPassword, //как в зависимости каких данных будет
                                                                        //выдаватся токен
                  AllowOfflineAccess = true, //refresh token
                  ClientSecrets = {new Secret("secret".Sha256())}, // 
                  AllowedScopes= {"opossumsAPI"} // к каким ресурсам выдается доступ                 
                }
            };
        }
    }
}
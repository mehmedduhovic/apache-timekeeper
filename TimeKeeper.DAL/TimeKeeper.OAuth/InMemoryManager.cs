using IdentityServer3.Core;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace TimeKeeper.OAuth
{
    public class InMemoryManager
    {
        public List<InMemoryUser> GetUsers()
        {
            return new List<InMemoryUser>
            {
                new InMemoryUser
                {
                    Subject = "hhuskic@school.edu",
                    Username = "hhuskic@school.edu",
                    Password = "andromeda",
                    Claims = new[]
                    {
                        new Claim(Constants.ClaimTypes.Name, "Husein Huskic")
                    }
                },
                new InMemoryUser
                {
                    Subject = "adelic@school.edu",
                    Username = "adelic@school.edu",
                    Password = "andromeda",
                    Claims = new[]
                    {
                        new Claim(Constants.ClaimTypes.Name, "Amel Delic")
                    }
                }
            };
        }

        public IEnumerable<Scope> GetScopes()
        {
            return new[]
            {
                StandardScopes.OpenId,
                StandardScopes.Profile,
                StandardScopes.OfflineAccess,
                new Scope
                {
                    Name = "read",
                    DisplayName="Read Employee Data"
                }
            };
        }

        public IEnumerable<Client> GetClients()
        {
            return new[]
            {
                new Client
                {
                    ClientId = "timekeeper",
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("$ch00l".Sha256())
                    },
                    ClientName = "TimeKeeper",
                    Flow = Flows.ResourceOwner,
                    AllowedScopes = new List<string>
                    {
                        Constants.StandardScopes.OpenId,
                        "read"
                    },
                    Enabled = true
                }
            };
        }
    }
}
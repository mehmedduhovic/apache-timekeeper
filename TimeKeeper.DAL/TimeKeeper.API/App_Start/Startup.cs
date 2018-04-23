using IdentityServer3.AccessTokenValidation;
using Microsoft.Owin;
using Microsoft.Owin.Security.Jwt;
using Owin;
using System;
using System.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;

[assembly: OwinStartup(typeof(TimeKeeper.API.App_Start.Startup))]

namespace TimeKeeper.API.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //var certificate = new X509Certificate2(Convert.FromBase64String("MIIC1DCCAbygAwIBAgIQGU8bZgi257BN+dMzrNaQSDANBgkqhkiG9w0BAQUFADATMREwDwYDVQQDEwhGaWxpcC1QQzAeFw0xNjAyMjEwNjQ4MzdaFw0xNzAyMjEwMDAwMDBaMBMxETAPBgNVBAMTCEZpbGlwLVBDMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA2/Ze/ru74n5YRTQKQujQOx4P7poPDuVfSi7aiBa7BV4pbBGXtzU8Mwt7LhewJnvbtJVdOj3S/4ndD3Zl65zV4RtqPAtGI0MJnMLxPibKaqnvikhLj/K5EEJ4yqXXlRSbH1VwwHzFtHmxnZd2KlmpNKF4WHaOzInoYmi36sffoAaikP7vmvUcO88X4tMP/KWxp5JZo5cQmLcKO3XiRDq532gezItq/p/iucHukF3WRMOL/73wB9bUcBU2/GIkFyB7Ne0YmJfhUopyCZnRh0UQP3DKrO1iKCy1Lje+TMi8hOoCfok8u1ZaJuueXgSf/J2S+AEe3M8D4OoYo6W0p+ZebwIDAQABoyQwIjALBgNVHQ8EBAMCBDAwEwYDVR0lBAwwCgYIKwYBBQUHAwEwDQYJKoZIhvcNAQEFBQADggEBANT5ltvrZJMHZNVO8juAO+PxyCSYmvIKNO2vBIglewmoF4vfdyABnAoIzHgKn5uvq1oPJCeiUHoNpzBMQiWqGW+NNL6wfTsZyfM24+EMv0ZDvkdm/B356tTZbPi/Pg/4vqDqAxbS6eE+VpBlZPHfDqCzlYKL+Ahhaq+xS4G0FJCvjWFt/EncwnVijuur3VYV+KxteAE+2ClI3N60nBH4UiOyigZ3Mwk0ONYu2R8X/AVMNpjKYXyXEGSi/JrCCNvINmnP4+SWpfFjVD8DDFK9VVsM6tl0HPM8qy3VkipCCnLZ6MRRIhrDnj8FnOZxCq7aI5fP7WDiwHKC/2zsX6LcOGs="));

            //app.UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions
            //{
            //    AllowedAudiences = new[] { "http://localhost:53812/resources" },
            //    TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidAudience = "http://localhost:53812/resources",
            //        ValidIssuer = "http://localhost:53812",
            //        IssuerSigningKey = new X509SecurityKey(certificate),
            //    }
            //});

            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                Authority = "http://localhost:53812"
            });
        }
    }
}
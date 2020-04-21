﻿namespace Infrastructure.Identity
{
    public interface IAppSettings
    {
        string Secret { get; set; }
        string JwtIssuer { get; set; }
        string StripeSecretKey { get; set; }
        string StripePublicKey { get; set; }
    }
}
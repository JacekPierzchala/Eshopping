// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;

namespace Eshopping.Identity
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("catalogapi"),
    
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
            {
                //lis of microservices
                new ApiResource("Catalog","Catalog.API")
                {
                    Scopes={ "catalogapi" }
                }
            };
        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                //m2m flow
               new Client
               {
                   ClientName = "Catalog API Client",
                   ClientId="CatalogAPIClient",
                   ClientSecrets={new Secret("f449d97a-1bbf-45db-8761-ad8905c7fee2".Sha256()) },
                   AllowedGrantTypes=GrantTypes.ClientCredentials,
                   AllowedScopes={ "catalogapi" }
               }
            };
    }
}
﻿namespace BeerAppreciation.Services.Identity.API.Models.AccountViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using IdentityServer4.Models;

    public class ConsentViewModel : ConsentInputModel
    {
        public ConsentViewModel(ConsentInputModel model, string returnUrl, AuthorizationRequest request, Client client, Resources resources)
        {
            this.RememberConsent = model?.RememberConsent ?? true;
            this.ScopesConsented = model?.ScopesConsented ?? Enumerable.Empty<string>();

            this.ReturnUrl = returnUrl;

            this.ClientName = client.ClientName;
            this.ClientUrl = client.ClientUri;
            this.ClientLogoUrl = client.LogoUri;
            this.AllowRememberConsent = client.AllowRememberConsent;

            this.IdentityScopes = resources.IdentityResources.Select(x => new ScopeViewModel(x, this.ScopesConsented.Contains(x.Name) || model == null)).ToArray();
            this.ResourceScopes = resources.ApiResources.SelectMany(x => x.Scopes).Select(x => new ScopeViewModel(x, this.ScopesConsented.Contains(x.Name) || model == null)).ToArray();
        }

        public string ClientName { get; set; }
        public string ClientUrl { get; set; }
        public string ClientLogoUrl { get; set; }
        public bool AllowRememberConsent { get; set; }

        public IEnumerable<ScopeViewModel> IdentityScopes { get; set; }
        public IEnumerable<ScopeViewModel> ResourceScopes { get; set; }
    }

    public class ScopeViewModel
    {
        public ScopeViewModel(Scope scope, bool check)
        {
            this.Name = scope.Name;
            this.DisplayName = scope.DisplayName;
            this.Description = scope.Description;
            this.Emphasize = scope.Emphasize;
            this.Required = scope.Required;
            this.Checked = check || scope.Required;
        }

        public ScopeViewModel(IdentityResource identity, bool check)
        {
            this.Name = identity.Name;
            this.DisplayName = identity.DisplayName;
            this.Description = identity.Description;
            this.Emphasize = identity.Emphasize;
            this.Required = identity.Required;
            this.Checked = check || identity.Required;
        }

        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool Emphasize { get; set; }
        public bool Required { get; set; }
        public bool Checked { get; set; }
    }
}

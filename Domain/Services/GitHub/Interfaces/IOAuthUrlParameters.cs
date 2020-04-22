using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Services.GitHub.Interfaces
{
    public interface IOAuthUrlParameters
    {
        /// <summary>
        /// An unguessable random string. It is used to protect against cross-site request forgery attacks.
        /// </summary>
        string state { get; set; }

        /// <summary>
        /// Suggests a specific account to use for signing in and authorizing the app.
        /// </summary>
        string login { get; set; }

        /// <summary>
        /// A space-delimited list of scopes. 
        /// If not provided, scope defaults to an empty list for users that have not authorized any scopes for the application. 
        /// For users who have authorized scopes for the application, the user won't be shown the OAuth authorization page with the list of scopes. 
        /// Instead, this step of the flow will automatically complete with the set of scopes the user has authorized for the application. 
        /// For example, if a user has already performed the web flow twice and has authorized one token with user scope and another token with repo scope, a third web flow that does not provide a scope will receive a token with user and repo scope.
        /// </summary>
        string scope { get; set; }

        /// <summary>
        /// Whether or not unauthenticated users will be offered an option to sign up for GitHub during the OAuth flow. The default is true. Use false when a policy prohibits signups.
        /// </summary>
        bool allow_signup { get; set; }
    }
}

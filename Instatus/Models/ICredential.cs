using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Instatus.Models;

namespace Instatus.Models
{
    public interface ICredential
    {
        string Key { get; }
        string Secret { get; }
        string Alias { get; }
        string Scope { get; }
        string Features { get; }
    }
}

namespace Instatus
{
    public static class CredentialExtensions
    {
        public static bool HasFeature(this ICredential credential, string name)
        {
            return credential.Features.ToList().Any(f => f.Match(name));
        }
    }
}

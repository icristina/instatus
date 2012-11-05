using Instatus.Integration.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Integration.Facebook
{
    // http://developers.facebook.com/docs/reference/login/signed-request/
    public class FacebookSignedRequest
    {
        public string Code { get; set; }
        public string Algorithm { get; set; }
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime IssuedAt { get; set; }
        public long UserId { get; set; }
        public SignedUser User { get; set; }
        public string OauthToken { get; set; }
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime Expires { get; set; }
        public string AppData { get; set; }
        public SignedPage Page { get; set; }

        public class SignedUser
        {
            public string Locale { get; set; }
            public string Country { get; set; }
            public SignedAge Age { get; set; }
        }

        public class SignedAge
        {
            public int Min { get; set; }
            public int Max { get; set; }
        }

        public class SignedPage
        {
            public long Id { get; set; }
            public bool Liked { get; set; }
            public bool Admin { get; set; }
        }
    }
}

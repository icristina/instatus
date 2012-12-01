using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Integration.Facebook
{
    public class FacebookSettings
    {
        public string AppId { get; set; }
        public string AppSecret { get; set; }
        public string ChannelUrl { get; set; }
        public string CanvasUrl { get; set; }
        public string[] MinimumClaims { get; set; }
    }
}

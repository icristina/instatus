using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Web
{
    public enum WebProvider
    {
        Internal, // Instatus database        
        Generated, // mock data
        Imported, // batch import
        External, // custom crm or database
        Facebook,
        Google,
        Microsoft,
        Twitter,
        Flickr,
        Vimeo,
        YouTube,
        Amazon,
        Typekit,
        Omniture,
        Adobe,
        Salesforce,
        CampaignMonitor
    }
}
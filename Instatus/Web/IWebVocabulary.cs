using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Web
{
    public interface IWebVocabulary
    {
        string GetItemType(object instance);
    }
}
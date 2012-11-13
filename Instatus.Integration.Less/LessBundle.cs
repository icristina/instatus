using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Optimization;

namespace Instatus.Integration.Less
{
    public class LessBundle : Bundle
    {       
        public LessBundle(string virtualPath)
            : this(virtualPath, string.Empty)
        {

        }
        
        public LessBundle(string virtualPath, string cdnPath) 
            : base(virtualPath, cdnPath, new LessBundleTransform(), new CssMinify())
        {

        }
    }
}

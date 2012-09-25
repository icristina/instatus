using Instatus.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Core
{
    public interface ICampaignManager
    {
        Campaign GetActiveCampaign();
    }
}

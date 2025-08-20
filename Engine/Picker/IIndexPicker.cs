using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YSPFrom.Helpers.Utilities
{
    public interface IIndexPicker
    {
        int PickIndex(string rewardName, int finalMultiplier);
    }
}

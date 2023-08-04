using HCEngine.DI;
using System.Collections.Generic;

namespace HCEngine.RewardSystem
{
    public interface IBox : IEnumerable<PairCountRewardable>, IIdentificator
    {
        BoxType Type { get; }
        
        void GenerateRewards();

        void ClaimAll();
    }
}

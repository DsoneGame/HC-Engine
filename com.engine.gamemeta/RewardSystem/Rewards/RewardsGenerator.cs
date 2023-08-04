using HCEngine.DI;
using HCEngine.Random;
using HCEngine.RewardSystem.Generic;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace HCEngine.RewardSystem
{
    public class RewardsGenerator : IRewardsGenerator
    {
        IEnumerator<PairCountRewardable> IRewardsGenerator.Generate(RewardRarityConfig[] configs, int rewardsCount, bool isRepeatable)
        {
            RewardRarityConfig[] randomsConfigs = configs.GenerateRandomRarities(rewardsCount, isRepeatable);

            for (int i = 0; i < rewardsCount; i++)
            {
                RewardRarityConfig config = randomsConfigs[i];

                yield return new PairCountRewardable(
                    UnityEngine.Random.Range(config.CountRange.x, config.CountRange.y),
                    FindRewardable(config.RewardableType)
                    );
            }
        }

        private IRewardable FindRewardable(RewardableType type)
        {
            IRewardable rewardable = DIContainer.WhereId<IRewardable>((int)type).LastOrDefault();
            Contract.Assert(rewardable != null, $"The rewardable of type {type} is not found!");

            return rewardable;
        }
    }
}
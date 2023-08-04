using UnityEngine;

namespace HCEngine.RewardSystem
{
    [CreateAssetMenu(fileName = "New Rewardables Cards Info", menuName = "Add/Reward/Rewardables Cards Info", order = 1)]
    public class RewardableCardInfo : ScriptableObject
    {
        public RewardableType RewardableType;
        public UpgradeableType UpgradeableType;
    }
}
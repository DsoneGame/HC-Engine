using UnityEngine;

namespace HCEngine.RewardSystem
{
    [CreateAssetMenu(fileName = "New Rewardables Currencies Info", menuName = "Add/Reward/Rewardables Currencies Info", order = 1)]
    public class RewardableCurrencyInfo : ScriptableObject
    {
        public RewardableType RewardableType;
        public CurrencyType CurrencyType;
    }
}
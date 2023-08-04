using HCEngine.Balance;
using HCEngine.Data;
using UnityEngine;

namespace HCEngine.Upgrade
{
    [CreateAssetMenu(fileName = "New Upgradable Info", menuName = "Add/Upgrades/Upgradable Info", order = 10)]
    public class UpgradableInfo : ScriptableObject
    {
        [Header("Save Parameters")]
        public UpgradeableType Type;
        public ValueField<int> Level;

        [Header("Info Parameters")]
        public UpgradableSettings[] UpgradeInfos;
        public BalanceInfo[] BalanceInfos;

        [Header("Reset Parameters")]
        [Min(0)] public int InitLevel = 0;
    }
}

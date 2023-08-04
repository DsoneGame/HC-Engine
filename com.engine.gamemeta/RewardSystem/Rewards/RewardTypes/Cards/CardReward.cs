using HCEngine.Upgrade;
using HCEngine.DI;
using System.Linq;
using System.Diagnostics.Contracts;

namespace HCEngine.RewardSystem
{
    public class CardReward : Rewardable, IAwake
    {
        private UpgradeableType _cardType;
        private IUpgradeableCard _upgradeable;

        public CardReward(RewardableType type, UpgradeableType cardType) : base(type)
        {
            _cardType = cardType;
        }

        public void Awake()
        {
            InitializeUpgradeable();
        }

        public void InitializeUpgradeable()
        {
            _upgradeable = DIContainer.WhereId<IUpgradeable>((int)_cardType).OfType<IUpgradeableCard>().LastOrDefault();
            Contract.Assert(_upgradeable != null, $"The card of type {_cardType} is not found!");
        }

        public override void Claim(int count)
        {
            _upgradeable.AddCard(count);
        }

    }
}
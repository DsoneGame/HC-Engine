using HCEngine.DI;
using System;
using UnityEngine;

namespace HCEngine.RewardSystem
{
    public abstract class Rewardable : IRewardable, IDependency
    {
        [SerializeField] protected RewardableType _rewardableType;

        public int Id => (int)_rewardableType;

        public Rewardable(RewardableType type)
        {
            _rewardableType = type;
        }

        public void Inject()
        {
            DIContainer.Register<IRewardable>(this);
        }

        public abstract void Claim(int count);

        public override int GetHashCode()
        {
            return HashCode.Combine(_rewardableType);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Rewardable))
            {
                return false;
            }

            Rewardable other = (Rewardable)obj;
            return _rewardableType == other._rewardableType;
        }

    }
}
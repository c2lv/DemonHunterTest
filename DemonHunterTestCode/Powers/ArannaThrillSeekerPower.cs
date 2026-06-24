using System;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace DemonHunterTest.DemonHunterTestCode.Powers;

public sealed class ArannaThrillSeekerPower : DemonHunterTestPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Single;

    public override Creature ModifyUnblockedDamageTarget(Creature target, decimal amount, ValueProp props, Creature? dealer)
    {
        if (target != base.Owner)
        {
            return target;
        }

        if (base.CombatState.CurrentSide != base.Owner.Side)
        {
            return target;
        }

        ArgumentNullException.ThrowIfNull(base.Owner.Player);
        Creature? randomEnemy = base.Owner.Player.RunState.Rng.CombatTargets.NextItem(base.CombatState.HittableEnemies);
        return randomEnemy ?? target;
    }
}

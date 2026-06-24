using System;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace DemonHunterTest.DemonHunterTestCode.Powers;

public sealed class BrutalAnnihilanPower : DemonHunterTestPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterCurrentHpChanged(Creature creature, decimal delta)
    {
        if (creature != base.Owner || delta >= 0m)
        {
            return;
        }

        if (base.CombatState.CurrentSide != base.Owner.Side)
        {
            return;
        }

        ArgumentNullException.ThrowIfNull(base.Owner.Player);
        Creature? target = base.Owner.Player.RunState.Rng.CombatTargets.NextItem(base.CombatState.HittableEnemies);
        if (target == null)
        {
            return;
        }

        Flash();
        await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), target, -delta, ValueProp.Unblockable | ValueProp.Unpowered, base.Owner, null);
    }
}

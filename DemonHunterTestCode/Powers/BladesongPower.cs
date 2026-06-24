using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace DemonHunterTest.DemonHunterTestCode.Powers;

public sealed class BladesongPower : DemonHunterTestPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (side != CombatSide.Player)
        {
            return;
        }

        IReadOnlyList<Creature> hittableEnemies = base.CombatState.HittableEnemies;
        if (hittableEnemies.Count == 0 || base.Owner.Player == null)
        {
            return;
        }

        var runState = base.Owner.Player.RunState;
        if (runState == null)
        {
            return;
        }

        Creature? target = runState.Rng.CombatTargets.NextItem(hittableEnemies);
        if (target is not null)
        {
            await CreatureCmd.Damage(
                choiceContext,
                target,
                base.Amount,
                ValueProp.Unpowered,
                base.Owner,
                null
            );
        }
    }
}

using System;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace DemonHunterTest.DemonHunterTestCode.Powers;

public sealed class InfernalStaplerPower : DemonHunterTestPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterDamageGiven(PlayerChoiceContext choiceContext, Creature? dealer, DamageResult result, ValueProp props, Creature target, CardModel? cardSource)
    {
        if (dealer != base.Owner || !props.IsPoweredAttack() || result.UnblockedDamage <= 0)
        {
            return;
        }

        ThornsPower? thornsPower = target.GetPower<ThornsPower>();
        if (thornsPower == null)
        {
            return;
        }

        decimal currentThorns = thornsPower.Amount;
        decimal reducedThorns = Math.Max(0m, currentThorns - base.Amount);
        decimal delta = reducedThorns - currentThorns;
        if (delta == 0m)
        {
            return;
        }

        Flash();
        await PowerCmd.ModifyAmount(choiceContext, thornsPower, (int)delta, base.Owner, cardSource);
        await PowerCmd.Apply<ThornsPower>(choiceContext, base.Owner, -delta, base.Owner, cardSource);
    }
}

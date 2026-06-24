using System;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace DemonHunterTest.DemonHunterTestCode.Powers;

public sealed class KaynSunfuryPower : DemonHunterTestPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Single;

    public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        if (card.Owner.Creature != base.Owner || card.Type != CardType.Attack)
        {
            return;
        }

        if (!card.Id.Entry.Contains("Illidari", StringComparison.OrdinalIgnoreCase))
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
        await CardCmd.AutoPlay(choiceContext, card, target, AutoPlayType.Default, skipXCapture: true);

        if (CombatManager.Instance.IsOverOrEnding || base.Owner.IsDead)
        {
            // See MakimaPower for why this is necessary: OnPlayWrapper can return early without
            // popping itself off the choice context's model stack if combat ends mid-play.
            choiceContext.PopModel(card);
        }
    }
}

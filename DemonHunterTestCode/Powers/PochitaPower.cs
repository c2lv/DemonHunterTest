using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace DemonHunterTest.DemonHunterTestCode.Powers;

public sealed class PochitaPower : DemonHunterTestPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Single;

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player.Creature != base.Owner)
        {
            return;
        }

        Flash();
        await PlayerCmd.LoseEnergy(1m, player);

        CardPile exhaustPile = PileType.Exhaust.GetPile(player);
        CardModel? card = player.RunState.Rng.CombatCardSelection.NextItem(exhaustPile.Cards);
        if (card == null)
        {
            return;
        }

        Creature? target = GetTarget(card);
        await CardCmd.AutoPlay(choiceContext, card, target, AutoPlayType.Default, skipXCapture: true);

        if (CombatManager.Instance.IsOverOrEnding || base.Owner.IsDead)
        {
            // See MakimaPower for why this is necessary: OnPlayWrapper can return early without
            // popping itself off the choice context's model stack if combat ends mid-play.
            choiceContext.PopModel(card);
        }
    }

    private Creature? GetTarget(CardModel card)
    {
        ICombatState? combatState = base.CombatState;
        if (combatState == null)
        {
            return null;
        }

        return card.TargetType switch
        {
            TargetType.AnyEnemy => combatState.HittableEnemies.FirstOrDefault(),
            TargetType.AnyPlayer => base.Owner,
            _ => null,
        };
    }
}

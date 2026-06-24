using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;

namespace DemonHunterTest.DemonHunterTestCode.Powers;

public sealed class MakimaPower : DemonHunterTestPower
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
        await PlayerCmd.GainEnergy(2m, player);

        using (CardSelectCmd.PushSelector(new VakuuCardSelector()))
        {
            int startTurn = player.PlayerCombatState?.TurnNumber ?? -1;
            for (int cardsPlayed = 0; cardsPlayed < 13; cardsPlayed++)
            {
                if (CombatManager.Instance.IsOverOrEnding)
                {
                    break;
                }

                if (CombatManager.Instance.IsPlayerReadyToEndTurn(player))
                {
                    break;
                }

                if (player.PlayerCombatState?.TurnNumber != startTurn)
                {
                    break;
                }

                CardPile pile = PileType.Hand.GetPile(player);
                CardModel? card = pile.Cards.FirstOrDefault((CardModel c) => c.CanPlay());
                if (card == null)
                {
                    break;
                }

                Creature? target = GetTarget(card);
                await card.SpendResources();
                await CardCmd.AutoPlay(choiceContext, card, target, AutoPlayType.Default, skipXCapture: true);

                if (CombatManager.Instance.IsOverOrEnding || base.Owner.IsDead)
                {
                    // OnPlayWrapper returns early without popping its own model when combat ends or the
                    // owner dies mid-play (e.g. this card's autoplay killed the last enemy), leaving the
                    // card on top of the choice context's model stack. Clean it up ourselves before
                    // returning, otherwise the engine's PopModel(this) call for this hook will fail.
                    choiceContext.PopModel(card);
                    break;
                }
            }
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
            TargetType.AnyAlly => base.Owner.Player?.RunState.Rng.CombatTargets.NextItem(combatState.Allies.Where((Creature c) => c != null && c.IsAlive && c.IsPlayer && c != base.Owner)),
            TargetType.AnyPlayer => base.Owner,
            _ => null,
        };
    }
}

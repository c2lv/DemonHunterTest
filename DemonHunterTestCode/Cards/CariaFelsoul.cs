using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using DemonHunterTest.DemonHunterTestCode.Character;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace DemonHunterTestCode.Cards;

[Pool(typeof(DemonHunterTestCardPool))]
public sealed class CariaFelsoul : DemonHunterTestCard
{
    public CariaFelsoul()
        : base(2, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        List<CardModel> drawPileCards = PileType.Draw.GetPile(base.Owner).Cards.ToList();
        if (drawPileCards.Count == 0)
        {
            return;
        }

        List<CardModel> shuffledForDisplay = drawPileCards.StableShuffle(base.Owner.RunState.Rng.CombatCardSelection);
        CardModel? chosen = (await CardSelectCmd.FromSimpleGrid(
            choiceContext, shuffledForDisplay, base.Owner, new CardSelectorPrefs(base.SelectionScreenPrompt, 1)
        )).FirstOrDefault();
        if (chosen == null)
        {
            return;
        }

        Creature? target = GetTarget(chosen);
        await CardCmd.AutoPlay(choiceContext, chosen, target, AutoPlayType.Default, skipXCapture: true);
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
            TargetType.AnyAlly => base.Owner.RunState.Rng.CombatTargets.NextItem(combatState.Allies.Where((Creature c) => c != null && c.IsAlive && c.IsPlayer && c != base.Owner.Creature)),
            TargetType.AnyPlayer => base.Owner.Creature,
            _ => null,
        };
    }

    protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }
}

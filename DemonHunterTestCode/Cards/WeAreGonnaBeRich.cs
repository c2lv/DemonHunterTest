using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using DemonHunterTest.DemonHunterTestCode.Character;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;

namespace DemonHunterTestCode.Cards;

[Pool(typeof(DemonHunterTestCardPool))]
public sealed class WeAreGonnaBeRich : DemonHunterTestCard
{
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
        new CardsVar(1)
    };

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip>
    {
        HoverTipFactory.FromCard<RenoJackson>()
    };

    public WeAreGonnaBeRich()
        : base(1, CardType.Skill, CardRarity.Rare, TargetType.AllAllies)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ICombatState? tempCombatState = base.CombatState;
        if (tempCombatState == null) throw new InvalidOperationException("Combat state is required to create Reno Jackson cards.");
        CombatState combatState = (CombatState)tempCombatState;
        ArgumentNullException.ThrowIfNull(base.Owner);

        int cardsToAdd = base.DynamicVars.Cards.IntValue;
        if (cardsToAdd <= 0)
        {
            return;
        }

        List<CardModel> generatedCards = new List<CardModel>();
        foreach (Creature creature in combatState.GetTeammatesOf(base.Owner.Creature))
        {
            if (creature.Player == null || !creature.IsAlive || !creature.IsPlayer)
            {
                continue;
            }

            for (int i = 0; i < cardsToAdd; i++)
            {
                generatedCards.Add(combatState.CreateCard<RenoJackson>(creature.Player));
            }
        }

        if (generatedCards.Count == 0)
        {
            return;
        }

        // Add all cards one by one
        foreach(CardModel card in generatedCards)
        {
            await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Draw, null, CardPilePosition.Random);
        }
    }

    protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }
}

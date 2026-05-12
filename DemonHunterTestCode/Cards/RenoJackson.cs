using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using BaseLib.Utils;
using DemonHunterTest.DemonHunterTestCode.Character;
using DemonHunterTest.DemonHunterTestCode.Cards;

namespace DemonHunterTestCode.Cards;

[Pool(typeof(DemonHunterTestCardPool))]
public sealed class RenoJackson() : DemonHunterTestCard(
    6, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust, DHKeyWords.Highlander];

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[1]
    {
        new HpLossVar(15m),
    };

    protected override bool IsPlayable => IsHighlander;

    protected override bool ShouldGlowGoldInternal => IsHighlander;

    public override bool TryModifyEnergyCostInCombat(CardModel card, decimal originalCost, out decimal modifiedCost)
    {
        modifiedCost = originalCost;
        if (card != this || Owner == null || !CombatManager.Instance.IsInProgress)
        {
            return false;
        }

        decimal lostHp = Owner.Creature.MaxHp - Owner.Creature.CurrentHp;
        decimal costReduction = decimal.Floor(lostHp / DynamicVars["HpLoss"].BaseValue);
        modifiedCost = Math.Max(0m, originalCost - costReduction);
        return true;
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.Heal(Owner.Creature, Owner.Creature.MaxHp - Owner.Creature.CurrentHp);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["HpLoss"].UpgradeValueBy(-5m);
    }
}
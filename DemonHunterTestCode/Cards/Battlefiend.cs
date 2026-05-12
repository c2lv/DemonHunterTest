using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using DemonHunterTest.DemonHunterTestCode.Character;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace DemonHunterTestCode.Cards;

[Pool(typeof(DemonHunterTestCardPool))]
public sealed class Battlefiend : DemonHunterTestCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(8m, ValueProp.Move),
        new DynamicVar("Increase", 4m)
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [
        CardKeyword.Exhaust
    ];

    public Battlefiend()
        : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        ICombatState? combatState = base.CombatState;
        ArgumentNullException.ThrowIfNull(combatState);
        // Deal damage
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);

        // Create a copy with increased damage and add to discard pile (Severance-style)
        Battlefiend copy = combatState.CreateCard<Battlefiend>(base.Owner);
        copy.DynamicVars.Damage.BaseValue += base.DynamicVars["Increase"].BaseValue;
        CardPileAddResult discardResult = await CardPileCmd.AddGeneratedCardToCombat(copy, PileType.Discard, base.Owner);
        CardCmd.PreviewCardPileAdd(new[] { discardResult });
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["Increase"].UpgradeValueBy(4m);
    }
}

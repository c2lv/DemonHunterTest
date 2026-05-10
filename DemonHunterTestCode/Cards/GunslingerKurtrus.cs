using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Models;
using BaseLib.Utils;
using DemonHunterTest.DemonHunterTestCode.Cards;
using DemonHunterTest.DemonHunterTestCode.Character;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Creatures;

namespace DemonHunterTestCode.Cards;

[Pool(typeof(DemonHunterTestCardPool))]
public sealed class GunslingerKurtrus : DemonHunterTestCard
{
    private const string _calculatedHitsKey = "CalculatedHits";

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(2m, ValueProp.Move),
        new CalculationBaseVar(0m),
        new CalculationExtraVar(1m),
        new CalculatedVar("CalculatedHits").WithMultiplier((CardModel card, Creature? _)
        => GetStatuses(card.Owner).Count())
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [DHKeyWords.Highlander];

    public GunslingerKurtrus()
        : base(2, CardType.Attack, CardRarity.Rare, TargetType.RandomEnemy)
    {
    }

    protected override bool IsPlayable => IsHighlander;

    protected override bool ShouldGlowGoldInternal => IsHighlander;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (!IsHighlander) return;
        ArgumentNullException.ThrowIfNull(Owner.PlayerCombatState);
        PlayerCombatState playerCombatState = Owner.PlayerCombatState;
        List<CardModel> cardsToDiscard = playerCombatState.DrawPile.Cards.ToList();
        int count = cardsToDiscard.Count;

        // 뽑을 카드 1장당 무작위 적에게 피해
        if (count > 0)
        {
            ArgumentNullException.ThrowIfNull(base.CombatState);
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
                .WithHitCount(count)
                .FromCard(this)
                .TargetingRandomOpponents(base.CombatState)
                .WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
                .Execute(choiceContext);
        }
        // 뽑을 카드 더미의 모든 카드 버리기
        foreach (CardModel card in cardsToDiscard)
        {
            await CardPileCmd.Add(card, PileType.Discard);
        }
    }

    private static IEnumerable<CardModel> GetStatuses(Player owner)
    {
        ArgumentNullException.ThrowIfNull(owner.PlayerCombatState);
        return owner.PlayerCombatState.AllCards.Where((CardModel c) => c.Pile.Type == PileType.Draw);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(1m);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using DemonHunterTest.DemonHunterTestCode.Cards;
using DemonHunterTest.DemonHunterTestCode.Character;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.ValueProps;

namespace DemonHunterTestCode.Cards;

[Pool(typeof(DemonHunterTestCardPool))]
public sealed class FeldoreiWarband : DemonHunterTestCard
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [
        DHKeyWords.Highlander
    ];

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(13m, ValueProp.Move),
        new CardsVar(4)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip>
    {
        HoverTipFactory.FromCard<Illidari>(base.IsUpgraded)
    };

    protected override bool IsPlayable => IsHighlander;

    protected override bool ShouldGlowGoldInternal => IsHighlander;

    public FeldoreiWarband()
        : base(0, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        ICombatState? tempCombatState = base.CombatState;
        ArgumentNullException.ThrowIfNull(tempCombatState);
        CombatState combatState = (CombatState)tempCombatState;

        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);

        List<Illidari> illidariCards = Illidari.Create(base.Owner, base.DynamicVars.Cards.IntValue, combatState).ToList();
        if (base.IsUpgraded)
        {
            CardCmd.Upgrade(illidariCards, CardPreviewStyle.None);
        }

        foreach (Illidari card in illidariCards)
        {
            await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, null);
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(4m);
    }
}

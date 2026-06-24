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

namespace DemonHunterTestCode.Cards;

[Pool(typeof(DemonHunterTestCardPool))]
public sealed class Security : DemonHunterTestCard
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [
        DHKeyWords.Outcast
    ];

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CardsVar(3)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip>
    {
        HoverTipFactory.FromCard<Illidari>(base.IsUpgraded)
    };

    public Security()
        : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ICombatState? tempCombatState = base.CombatState;
        ArgumentNullException.ThrowIfNull(tempCombatState);
        CombatState combatState = (CombatState)tempCombatState;

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
    }
}

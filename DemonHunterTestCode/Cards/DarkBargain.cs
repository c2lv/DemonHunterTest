using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using DemonHunterTest.DemonHunterTestCode.Character;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace DemonHunterTestCode.Cards;

[Pool(typeof(DemonHunterTestCardPool))]
public sealed class DarkBargain : DemonHunterTestCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CardsVar(1)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip>
    {
        HoverTipFactory.FromKeyword(CardKeyword.Exhaust)
    };

    public DarkBargain()
        : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // Select separately from Hand and Draw piles
        var handPile = PileType.Hand.GetPile(base.Owner);
        var drawPile = PileType.Draw.GetPile(base.Owner);
        int perPileAmount = base.DynamicVars.Cards.IntValue;

        int handAmount = Math.Min(perPileAmount, handPile.Cards.Count);
        int drawAmount = Math.Min(perPileAmount, drawPile.Cards.Count);

        if (handAmount <= 0 && drawAmount <= 0)
        {
            return;
        }

        foreach (CardModel item in await CardSelectCmd.FromHand(prefs: new CardSelectorPrefs(CardSelectorPrefs.ExhaustSelectionPrompt, 0, base.DynamicVars.Cards.IntValue), context: choiceContext, player: base.Owner, filter: null, source: this))
        {
            await CardCmd.Exhaust(choiceContext, item);
        }
        foreach (CardModel card in await CardSelectCmd.FromSimpleGrid(choiceContext, drawPile.Cards, base.Owner, new CardSelectorPrefs(base.SelectionScreenPrompt, drawAmount)))
        {
            await CardCmd.Exhaust(choiceContext, card);
        }
    }

    protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }
}

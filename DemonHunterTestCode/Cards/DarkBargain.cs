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
        foreach (CardModel item in await CardSelectCmd.FromHand(
            context: choiceContext, 
            player: base.Owner, 
            prefs: new CardSelectorPrefs(CardSelectorPrefs.ExhaustSelectionPrompt, base.DynamicVars.Cards.IntValue), 
            filter: null, 
            source: this
        ))
        {
            await CardCmd.Exhaust(choiceContext, item);
        }

        List<CardModel> cardsIn = (from c in PileType.Draw.GetPile(base.Owner).Cards
			orderby c.Rarity, c.Id
			select c).ToList();
		CardModel? cardModel = (await CardSelectCmd.FromSimpleGrid(
            choiceContext, 
            cardsIn, 
            base.Owner, 
            new CardSelectorPrefs(base.SelectionScreenPrompt, base.DynamicVars.Cards.IntValue)
            )).FirstOrDefault();
		if (cardModel != null)
		{
			await CardCmd.Exhaust(choiceContext, cardModel);
		}
    }

    protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }
}

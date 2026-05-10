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
using MegaCrit.Sts2.Core.Models;

namespace DemonHunterTestCode.Cards;

[Pool(typeof(DemonHunterTestCardPool))]
public sealed class SightlessWatcher : DemonHunterTestCard
{
	public SightlessWatcher()
		: base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		List<CardModel> drawPileCards = PileType.Draw.GetPile(base.Owner).Cards.ToList();
		if (drawPileCards.Count == 0)
		{
			return;
		}

		List<CardModel> cardsToChooseFrom = drawPileCards.Take(3).ToList();
		CardModel? cardModel = (await CardSelectCmd.FromSimpleGrid(
			choiceContext, cardsToChooseFrom, base.Owner, new CardSelectorPrefs(base.SelectionScreenPrompt, 1)
		)).FirstOrDefault();
		if (cardModel == null)
		{
			return;
		}
		foreach (CardModel card in cardsToChooseFrom)
		{
			if (card.Id == cardModel.Id)
			{
				await CardPileCmd.Add(card, PileType.Hand);
			}
			else
			{
				await CardPileCmd.Add(card, PileType.Discard);
			}
		}
	}

	protected override void OnUpgrade()
	{
		base.EnergyCost.UpgradeBy(-1);
	}
}

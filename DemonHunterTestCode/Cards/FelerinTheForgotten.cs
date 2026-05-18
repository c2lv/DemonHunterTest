using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using DemonHunterTest.DemonHunterTestCode.Cards;
using DemonHunterTest.DemonHunterTestCode.Character;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace DemonHunterTestCode.Cards;

[Pool(typeof(DemonHunterTestCardPool))]
public sealed class FelerinTheForgotten : DemonHunterTestCard
{
	protected override IEnumerable<DynamicVar> CanonicalVars => [];

	protected override IEnumerable<IHoverTip> ExtraHoverTips => [
		HoverTipFactory.FromKeyword(DHKeyWords.Outcast),
		HoverTipFactory.FromKeyword(CardKeyword.Exhaust)
	];

	public FelerinTheForgotten()
		: base(2, CardType.Skill, CardRarity.Rare, TargetType.Self)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
		IEnumerable<CardModel> cards = PileType.Exhaust.GetPile(base.Owner).Cards.Where((CardModel c) => c.Keywords.Contains(DHKeyWords.Outcast)).ToList();
		if (!cards.Any())
		{	
			return;
		}
		await AddToHandLeftmost(cards.First());
		if (cards.Count() > 1)
		{
			await CardPileCmd.Add(cards.ElementAt(1), PileType.Hand);
		}
	}

	protected override void OnUpgrade()
	{
		base.EnergyCost.UpgradeBy(-1);
	}
}

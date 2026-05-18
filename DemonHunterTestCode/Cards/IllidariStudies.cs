using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using DemonHunterTest.DemonHunterTestCode.Cards;
using DemonHunterTest.DemonHunterTestCode.Character;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace DemonHunterTestCode.Cards;

[Pool(typeof(DemonHunterTestCardPool))]
public sealed class IllidariStudies : DemonHunterTestCard
{
	protected override IEnumerable<DynamicVar> CanonicalVars => [
		new CardsVar(1)
	];

	protected override IEnumerable<IHoverTip> ExtraHoverTips => [
		HoverTipFactory.FromKeyword(DHKeyWords.Outcast),
	];

	public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

	public IllidariStudies()
		: base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
		if (base.RunState == null)
		{
			return;
		}
		IEnumerable<CardModel> distinctForCombat = CardFactory
		.GetDistinctForCombat(base.Owner, ModelDb.CardPool<DemonHunterTestCardPool>()
		.GetUnlockedCards(base.Owner.UnlockState, base.RunState.CardMultiplayerConstraint)
		.Where(c => c.Keywords.Contains(DHKeyWords.Outcast))
		, base.DynamicVars.Cards.IntValue, base.Owner.RunState.Rng.CombatCardGeneration);
		foreach (CardModel item in distinctForCombat)
		{
			if (base.IsUpgraded)
			{
				CardCmd.Upgrade(item);
			}
			await CardPileCmd.AddGeneratedCardToCombat(item, PileType.Hand, base.Owner);
		}
	}

	protected override void OnUpgrade()
	{
	}
}

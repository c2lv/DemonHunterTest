using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using DemonHunterTest.DemonHunterTestCode.Character;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
namespace DemonHunterTestCode.Cards;

[Pool(typeof(DemonHunterTestCardPool))]
public sealed class SoulciologistMalicia : DemonHunterTestCard
{
	protected override IEnumerable<DynamicVar> CanonicalVars => [];

	protected override IEnumerable<IHoverTip> ExtraHoverTips => new IHoverTip[]
	{
		HoverTipFactory.FromCard<SoulFragment>(),
		HoverTipFactory.FromCard<ReleasedSoul>(base.IsUpgraded)
	};

	public SoulciologistMalicia()
		: base(2, CardType.Skill, CardRarity.Rare, TargetType.Self)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		var combatState = base.CombatState;
		if (combatState == null)
		{
			return;
		}

		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);

		List<CardModel> list = GetSoulFragments(base.Owner).ToList();
		List<CardPileAddResult> results = new List<CardPileAddResult>();
		foreach (CardModel item in list)
		{
			var releasedSoul = combatState.CreateCard<ReleasedSoul>(base.Owner);
			if (base.IsUpgraded)
			{
				CardCmd.Upgrade(releasedSoul);
			}
			await CardCmd.Transform(item, releasedSoul, CardPreviewStyle.None);
			results.Add(await CardPileCmd.Add(releasedSoul, PileType.Draw, CardPilePosition.Random));
		}

		CardCmd.PreviewCardPileAdd(results);
	}

	private static IEnumerable<CardModel> GetSoulFragments(Player owner)
	{
		List<CardModel> fragments = new List<CardModel>();
		foreach (PileType pileType in new[]
		{
			PileType.Draw,
			PileType.Hand,
			PileType.Discard,
			PileType.Exhaust,
		})
		{
			CardPile? pile = pileType.GetPile(owner);
			if (pile != null)
			{
				fragments.AddRange(pile.Cards.Where((CardModel c) => c is SoulFragment));
			}
		}
		return fragments;
	}

	protected override void OnUpgrade()
	{
	}
}

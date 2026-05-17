using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using DemonHunterTest.DemonHunterTestCode.Character;
using DemonHunterTest.DemonHunterTestCode.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace DemonHunterTestCode.Cards;

[Pool(typeof(DemonHunterTestCardPool))]
public sealed class ExpendablePerformers : DemonHunterTestCard
{
	public override string PortraitPath => "expendable_performers.png".ImagePath();

	const int HAND_MAX = 10;

	protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
	{
		new CalculationBaseVar(0m),
		new CalculationExtraVar(1m),
		new CalculatedVar("CalculatedCards").WithMultiplier((CardModel card, Creature? _) => HAND_MAX - GetStatuses(card.Owner).Count() + 1) // +1 for the card being played
	};

	protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip> { HoverTipFactory.FromCard<Illidari>(true) };

	public ExpendablePerformers()
		: base(1, CardType.Skill, CardRarity.Ancient, TargetType.Self)
	{
	}

	private static IEnumerable<CardModel> GetStatuses(Player owner)
	{
		ArgumentNullException.ThrowIfNull(owner.PlayerCombatState);
		return owner.PlayerCombatState.AllCards.Where((CardModel c) => c.Pile != null && c.Pile.Type == PileType.Hand);
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		ICombatState? tempCombatState = base.CombatState;
		if (tempCombatState == null) throw new InvalidOperationException("Combat state is required to create Illidari cards.");
		CombatState combatState = (CombatState)tempCombatState;
		ArgumentNullException.ThrowIfNull(base.Owner);
		ArgumentNullException.ThrowIfNull(base.Owner.PlayerCombatState);

		int cardsToAdd = (int)((CalculatedVar)base.DynamicVars["CalculatedCards"]).Calculate(cardPlay.Target) - 1;
		if (cardsToAdd <= 0)
		{
			return;
		}

		// Create Illidari cards based on adjusted count
		List<Illidari> illidariCards = Illidari.Create(base.Owner, cardsToAdd, combatState).ToList();
		if (base.IsUpgraded)
		{
			CardCmd.Upgrade(illidariCards, CardPreviewStyle.None);
		}

		List<CardPileAddResult> results = new List<CardPileAddResult>();

		// Add all cards to hand (CardPileCmd will automatically distribute overflow to other piles)
		foreach (var card in illidariCards)
		{
			CardPileAddResult result = await CardPileCmd.AddGeneratedCardToCombat(
				card,
				PileType.Hand,
				null
			);
			results.Add(result);
		}
	}

	protected override void OnUpgrade()
	{
		base.EnergyCost.UpgradeBy(-1);
	}
}

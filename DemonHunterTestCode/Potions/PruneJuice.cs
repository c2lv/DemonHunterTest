using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using DemonHunterTest.DemonHunterTestCode.Character;
using DemonHunterTest.DemonHunterTestCode;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using DemonHunterTest.DemonHunterTestCode.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using DemonHunterTest.DemonHunterTestCode.Extensions;
using BaseLib.Extensions;

namespace DemonHunterTest.DemonHunterTestCode.Potions;

[Pool(typeof(DemonHunterTestPotionPool))]
public sealed class PruneJuice : DemonHunterTestPotion
{
	public override PotionRarity Rarity => PotionRarity.Uncommon;
	public override PotionUsage Usage => PotionUsage.CombatOnly;
	public override TargetType TargetType => TargetType.Self;

	public override IEnumerable<IHoverTip> ExtraHoverTips => new IHoverTip[] { HoverTipFactory.FromKeyword(DHKeyWords.Outcast) };

    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
	{
		PotionModel.AssertValidForTargetedPotion(target);
		// If hand already full, do nothing.
		if (CardPile.GetCards(base.Owner, PileType.Hand).Count() >= 10)
		{
			return;
		}

		while (true)
		{
			CardModel? card = await CardPileCmd.Draw(choiceContext, base.Owner);
			if (card == null)
			{
				// no more cards to draw
				break;
			}

			// If drawn card is Outcast, keep it and stop.
			if (card.Keywords != null && card.Keywords.Contains(DHKeyWords.Outcast))
			{
				break;
			}

			// Not Outcast: exhaust immediately and continue drawing.
			await CardCmd.Exhaust(choiceContext, card);

			// Stop if hand reached 10 after any draws (safety).
			if (CardPile.GetCards(base.Owner, PileType.Hand).Count() >= 10)
			{
				break;
			}
		}
	}
}
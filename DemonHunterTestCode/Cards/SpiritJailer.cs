using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using DemonHunterTest.DemonHunterTestCode.Character;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.HoverTips;

namespace DemonHunterTestCode.Cards;

[Pool(typeof(DemonHunterTestCardPool))]
public sealed class SpiritJailer : DemonHunterTestCard
{
	protected override IEnumerable<DynamicVar> CanonicalVars => [];

	protected override IEnumerable<IHoverTip> ExtraHoverTips => [
		HoverTipFactory.FromCard<SoulFragment>(base.IsUpgraded)
	];

	public SpiritJailer()
		: base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		var combatState = base.CombatState;
		if (combatState == null)
		{
			return;
		}
		var generated = SoulFragment.Create(base.Owner, 2, combatState);
		var results = await CardPileCmd.AddGeneratedCardsToCombat(generated, PileType.Draw, base.Owner, CardPilePosition.Random);
		if (base.IsUpgraded)
		{
			foreach (var r in results)
			{
				CardCmd.Upgrade(r.cardAdded);
			}
		}
		CardCmd.PreviewCardPileAdd(results);
		await CardPileCmd.Draw(choiceContext, 1, base.Owner);
	}

	protected override void OnUpgrade()
	{
	}
}

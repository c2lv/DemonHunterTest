using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using DemonHunterTest.DemonHunterTestCode.Character;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace DemonHunterTestCode.Cards;

[Pool(typeof(DemonHunterTestCardPool))]
public sealed class SoulshardLapidary : DemonHunterTestCard
{
	protected override IEnumerable<DynamicVar> CanonicalVars => [
		new PowerVar<StrengthPower>(2m)
	];

	protected override IEnumerable<IHoverTip> ExtraHoverTips => [
		HoverTipFactory.FromCard<SoulFragment>(),
		HoverTipFactory.FromKeyword(CardKeyword.Exhaust),
		HoverTipFactory.FromPower<StrengthPower>()
	];

	public SoulshardLapidary()
		: base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
	{
	}

	protected override bool IsPlayable => base.IsExhaustable;
	protected override bool ShouldGlowGoldInternal => base.IsExhaustable;

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		SoulFragment? soulFragment = SoulFragment.FindWorstInDrawPile(base.Owner);
		if (soulFragment != null)
		{
			await CardCmd.Exhaust(choiceContext, soulFragment);
		}
		await PowerCmd.Apply<StrengthPower>(choiceContext, base.Owner.Creature, base.DynamicVars.Strength.IntValue, base.Owner.Creature, null);
	}

	protected override void OnUpgrade()
	{
		base.DynamicVars.Strength.UpgradeValueBy(1m);
	}
}

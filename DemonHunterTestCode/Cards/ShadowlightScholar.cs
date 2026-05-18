using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using DemonHunterTest.DemonHunterTestCode.Character;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace DemonHunterTestCode.Cards;

[Pool(typeof(DemonHunterTestCardPool))]
public sealed class ShadowlightScholar : DemonHunterTestCard
{
	protected override IEnumerable<DynamicVar> CanonicalVars => [
		new DamageVar(16m, ValueProp.Move)
	];

	protected override IEnumerable<IHoverTip> ExtraHoverTips => [
		HoverTipFactory.FromCard<SoulFragment>(base.IsUpgraded),
		HoverTipFactory.FromKeyword(CardKeyword.Exhaust)
	];

	public ShadowlightScholar()
		: base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
	{
	}

	protected override bool IsPlayable => base.IsExhaustable;

	protected override bool ShouldGlowGoldInternal => base.IsExhaustable;

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
		SoulFragment? soulFragment = SoulFragment.FindWorstInDrawPile(base.Owner);
		if (soulFragment != null)
		{
			await CardCmd.Exhaust(choiceContext, soulFragment);
		}
		await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
			.WithHitFx("vfx/vfx_attack_slash")
			.Execute(choiceContext);
	}

	protected override void OnUpgrade()
	{
		base.DynamicVars.Damage.UpgradeValueBy(5m);
	}
}

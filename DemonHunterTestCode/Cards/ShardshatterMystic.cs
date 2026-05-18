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
public sealed class ShardshatterMystic : DemonHunterTestCard
{
	protected override IEnumerable<DynamicVar> CanonicalVars => [
		new DamageVar(12m, ValueProp.Move)
	];

	protected override IEnumerable<IHoverTip> ExtraHoverTips => [
		HoverTipFactory.FromCard<SoulFragment>(),
		HoverTipFactory.FromKeyword(CardKeyword.Exhaust)
	];

	public ShardshatterMystic()
		: base(0, CardType.Attack, CardRarity.Common, TargetType.AllEnemies)
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
		if (base.CombatState != null)
		{
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).TargetingAllOpponents(base.CombatState)
			.WithHitFx("vfx/vfx_attack_slash")
			.Execute(choiceContext);
		}
	}

	protected override void OnUpgrade()
	{
		base.DynamicVars.Damage.UpgradeValueBy(4m);
	}
}

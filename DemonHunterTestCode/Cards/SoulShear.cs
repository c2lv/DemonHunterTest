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
public sealed class SoulShear : DemonHunterTestCard
{
	protected override IEnumerable<DynamicVar> CanonicalVars => [
		new DamageVar(9m, ValueProp.Move),
		new CardsVar(1)
	];

	protected override IEnumerable<IHoverTip> ExtraHoverTips => [
		HoverTipFactory.FromCard<SoulFragment>(base.IsUpgraded)
	];

	public SoulShear()
		: base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
		ArgumentNullException.ThrowIfNull(base.CombatState, "base.CombatState");
		await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
			.WithHitFx("vfx/vfx_attack_slash")
			.Execute(choiceContext);
		IEnumerable<SoulFragment> enumerable = SoulFragment.Create(base.Owner, base.DynamicVars.Cards.IntValue, base.CombatState);
		if (base.IsUpgraded)
		{
			foreach (SoulFragment item in enumerable)
			{
				CardCmd.Upgrade(item);
			}
		}
		CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardsToCombat(enumerable, PileType.Draw, base.Owner, CardPilePosition.Random));
	}

	protected override void OnUpgrade()
	{
		base.DynamicVars.Damage.UpgradeValueBy(2m);
	}
}

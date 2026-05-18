using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using DemonHunterTest.DemonHunterTestCode.Cards;
using DemonHunterTest.DemonHunterTestCode.Character;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace DemonHunterTestCode.Cards;

[Pool(typeof(DemonHunterTestCardPool))]
public sealed class KurtrusAshfallen : DemonHunterTestCard
{
	protected override IEnumerable<DynamicVar> CanonicalVars => [
		new DamageVar(11m, ValueProp.Move)
	];

	protected override IEnumerable<IHoverTip> ExtraHoverTips => [
		HoverTipFactory.FromPower<IntangiblePower>()
	];

	public override IEnumerable<CardKeyword> CanonicalKeywords => [
		DHKeyWords.Outcast,
		CardKeyword.Exhaust
	];

	public KurtrusAshfallen()
		: base(2, CardType.Attack, CardRarity.Rare, TargetType.TargetedNoCreature)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Attack", base.Owner.Character.AttackAnimDelay);

		await PowerCmd.Apply<IntangiblePower>(choiceContext, base.Owner.Creature, 1m, base.Owner.Creature, this);
		if (base.CombatState == null)
		{
			return;
		}
		base.CombatState.SortEnemiesBySlotName();
		var enemies = base.CombatState.HittableEnemies.ToList();
		if (enemies.Count == 0)
		{
			return;
		}
		await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).Targeting(enemies.First())
			.WithHitFx("vfx/vfx_attack_blunt")
			.Execute(choiceContext);
		await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).Targeting(enemies.Last())
			.WithHitFx("vfx/vfx_attack_blunt")
			.Execute(choiceContext);
	}

	protected override void OnUpgrade()
	{
		base.DynamicVars.Damage.UpgradeValueBy(5m);
	}
}

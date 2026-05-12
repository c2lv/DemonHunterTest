using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using DemonHunterTest.DemonHunterTestCode.Character;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Entities.Players;

namespace DemonHunterTestCode.Cards;

[Pool(typeof(ColorlessCardPool))]
public sealed class Illidari : DemonHunterTestCard
{
	protected override IEnumerable<DynamicVar> CanonicalVars => [
		new DamageVar(3m, ValueProp.Move),
		new PowerVar<RegenPower>(1m),
	];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [
        CardKeyword.Exhaust
    ];


	protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip> { 
        HoverTipFactory.FromPower<RegenPower>() 
    };

	public Illidari()
		: base(0, CardType.Attack, CardRarity.Token, TargetType.AnyEnemy)
	{
	}

	public static IEnumerable<Illidari> Create(Player owner, int amount, CombatState combatState)
	{
		List<Illidari> list = new List<Illidari>();
		for (int i = 0; i < amount; i++)
		{
			list.Add(combatState.CreateCard<Illidari>(owner));
		}
		return list;
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
		await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
			.WithHitFx("vfx/vfx_attack_slash")
			.Execute(choiceContext);
		if (base.Owner != null && base.Owner.Creature != null)
		{
			await PowerCmd.Apply<RegenPower>(new ThrowingPlayerChoiceContext(), base.Owner.Creature, base.DynamicVars["RegenPower"].BaseValue, base.Owner.Creature, this);
		}
	}

	protected override void OnUpgrade()
	{
		base.DynamicVars.Damage.UpgradeValueBy(2m);
	}
}
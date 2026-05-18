using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using DemonHunterTest.DemonHunterTestCode.Character;
using DemonHunterTest.DemonHunterTestCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace DemonHunterTestCode.Cards;

[Pool(typeof(DemonHunterTestCardPool))]
public sealed class ChaosStrike : DemonHunterTestCard
{
	protected override IEnumerable<DynamicVar> CanonicalVars => [
		new PowerVar<StrengthPower>(5m),
		new CardsVar(1)
	];

	protected override IEnumerable<IHoverTip> ExtraHoverTips => [
		HoverTipFactory.FromPower<StrengthPower>()
	];

	public ChaosStrike()
		: base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
		await PowerCmd.Apply<ChaosStrikePower>(choiceContext, base.Owner.Creature, base.DynamicVars.Strength.BaseValue, base.Owner.Creature, this);
		await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.IntValue, base.Owner);
	}

	protected override void OnUpgrade()
	{
		base.DynamicVars.Strength.UpgradeValueBy(3m);
	}
}

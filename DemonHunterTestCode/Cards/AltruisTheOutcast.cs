using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using DemonHunterTest.DemonHunterTestCode;
using DemonHunterTest.DemonHunterTestCode.Cards;
using DemonHunterTest.DemonHunterTestCode.Character;
using DemonHunterTest.DemonHunterTestCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;

namespace DemonHunterTestCode.Cards;

[Pool(typeof(DemonHunterTestCardPool))]
public sealed class AltruisTheOutcast : DemonHunterTestCard
{
	protected override IEnumerable<DynamicVar> CanonicalVars => [
		new DamageVar(6m, ValueProp.Move)
	];

	public AltruisTheOutcast()
		: base(3, CardType.Power, CardRarity.Rare, TargetType.None)
	{
	}

	protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip>
	{
		HoverTipFactory.FromKeyword(DHKeyWords.Outcast)
	};

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		ICombatState? combatState = base.CombatState;
		ArgumentNullException.ThrowIfNull(combatState);
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
		await PowerCmd.Apply<OutcastTriggerPower>(
			choiceContext,
			base.Owner.Creature,
			base.DynamicVars["Damage"].BaseValue,
			base.Owner.Creature,
			this);
	}

	protected override void OnUpgrade()
	{
		base.EnergyCost.UpgradeBy(-1);
	}
}

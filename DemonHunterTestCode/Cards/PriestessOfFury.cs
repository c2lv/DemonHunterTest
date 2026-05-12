using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using DemonHunterTest.DemonHunterTestCode.Character;
using DemonHunterTest.DemonHunterTestCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;

namespace DemonHunterTestCode.Cards;

[Pool(typeof(DemonHunterTestCardPool))]
public sealed class PriestessOfFury : DemonHunterTestCard
{
	protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
	{
		new DynamicVar("Deal", 6m)
	};

	public PriestessOfFury()
		: base(1, CardType.Power, CardRarity.Uncommon, TargetType.None)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		ICombatState? combatState = base.CombatState;
		ArgumentNullException.ThrowIfNull(combatState);
		await PowerCmd.Apply<BladesongPower>(
			choiceContext,
			base.Owner.Creature,
			base.DynamicVars["Deal"].BaseValue,
			base.Owner.Creature,
			this);
	}

	protected override void OnUpgrade()
	{
		base.DynamicVars["Deal"].UpgradeValueBy(3m);
	}
}

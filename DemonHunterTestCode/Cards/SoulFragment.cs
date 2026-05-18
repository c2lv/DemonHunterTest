using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace DemonHunterTestCode.Cards;

[Pool(typeof(ColorlessCardPool))]
public sealed class SoulFragment : DemonHunterTestCard
{
	protected override IEnumerable<DynamicVar> CanonicalVars => [
		new HealVar(2m),
		new CardsVar(1)
	];

	public override IEnumerable<CardKeyword> CanonicalKeywords => [
		CardKeyword.Exhaust
	];

	public SoulFragment()
		: base(0, CardType.Skill, CardRarity.Token, TargetType.Self)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
		await CreatureCmd.Heal(base.Owner.Creature, base.DynamicVars.Heal.BaseValue);
		await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.IntValue, base.Owner);
	}

	protected override void OnUpgrade()
	{
		base.DynamicVars.Heal.UpgradeValueBy(1m);
	}
}

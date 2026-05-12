using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using DemonHunterTest.DemonHunterTestCode.Character;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;

namespace DemonHunterTestCode.Cards;

[Pool(typeof(DemonHunterTestCardPool))]
public sealed class CoordinatedStrike : DemonHunterTestCard
{
	protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(8m, ValueProp.Move),
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip> { HoverTipFactory.FromCard<Illidari>() };
	public CoordinatedStrike()
		: base(1, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
		await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
			.WithHitFx("vfx/vfx_attack_slash")
			.Execute(choiceContext);
		ICombatState? tempCombatState = base.CombatState;
		if (tempCombatState == null) throw new InvalidOperationException("Combat state is required to create Illidari cards.");
		CombatState combatState = (CombatState)tempCombatState;
		List<Illidari> illidariCards = Illidari.Create(base.Owner, 3, combatState).ToList();
		CardPileAddResult drawResult = await CardPileCmd.AddGeneratedCardToCombat(illidariCards[0], PileType.Draw, null, CardPilePosition.Random);
		CardPileAddResult discardResult = await CardPileCmd.AddGeneratedCardToCombat(illidariCards[1], PileType.Discard, null);
		await CardPileCmd.AddGeneratedCardToCombat(illidariCards[2], PileType.Hand, null);
		CardCmd.PreviewCardPileAdd(new List<CardPileAddResult> { drawResult, discardResult });
	}

	protected override void OnUpgrade()
	{
		base.EnergyCost.UpgradeBy(-1);
	}
}

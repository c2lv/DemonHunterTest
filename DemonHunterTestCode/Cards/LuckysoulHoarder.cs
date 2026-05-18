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
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;

namespace DemonHunterTestCode.Cards;

[Pool(typeof(DemonHunterTestCardPool))]
public sealed class LuckysoulHoarder : DemonHunterTestCard
{
	protected override IEnumerable<DynamicVar> CanonicalVars => [
		new CardsVar(3)
	];

	protected override IEnumerable<IHoverTip> ExtraHoverTips => [
		HoverTipFactory.FromCard<SoulFragment>()
	];

	public LuckysoulHoarder()
		: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
	{
	}

	public void RandomizeCards()
    {
        if (Owner?.RunState == null)
        {
            return;
        }
        base.DynamicVars.Cards.BaseValue = Owner.RunState.Rng.CombatEnergyCosts.NextInt(6); // 0..5
        NCard.FindOnTable(this)?.UpdateVisuals(Pile?.Type ?? PileType.Hand, CardPreviewMode.Normal);
    }

    public override Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        if (card != this)
        {
            return Task.CompletedTask;
        }
        RandomizeCards();
        return Task.CompletedTask;
    }

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		ArgumentNullException.ThrowIfNull(base.CombatState);

		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
		var generated = SoulFragment.Create(base.Owner, (int)base.DynamicVars.Cards.BaseValue, base.CombatState);
		var results = await CardPileCmd.AddGeneratedCardsToCombat(generated, PileType.Draw, base.Owner, CardPilePosition.Random);
		CardCmd.PreviewCardPileAdd(results);
	}

	protected override void OnUpgrade()
	{
		base.EnergyCost.UpgradeBy(-1);
	}
}

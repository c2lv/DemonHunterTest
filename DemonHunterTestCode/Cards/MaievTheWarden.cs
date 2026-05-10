using System.Collections.Generic;
using DemonHunterTest.DemonHunterTestCode.Cards;
using MegaCrit.Sts2.Core.Entities.Cards;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace DemonHunterTestCode.Cards;

[Pool(typeof(CurseCardPool))]
public sealed class MaievTheWarden : DemonHunterTestCard
{
	public override bool CanBeGeneratedByModifiers => false;

	protected override bool ShouldGlowRedInternal => true;

	public override int MaxUpgradeLevel => 0;

	public override IEnumerable<CardKeyword> CanonicalKeywords => [
        CardKeyword.Eternal, DHKeyWords.Outcast, DHKeyWords.Taunt
    ];
	public MaievTheWarden()
		: base(0, CardType.Curse, CardRarity.Curse, TargetType.None, true)
	{
	}

	public override bool ShouldPlay(CardModel card, AutoPlayType autoPlayType)
	{
		if (card.Owner != base.Owner)
		{
			return true;
		}
		CardPile? pile = base.Pile;
		if (pile == null || pile.Type != PileType.Hand)
		{
			return true;
		}
		if (card is MaievTheWarden)
		{
			return true;
		}
		if (autoPlayType != AutoPlayType.None)
		{
			return true;
		}
		return false;
	}
}

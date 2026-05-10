using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using DemonHunterTest.DemonHunterTestCode.Character;
using BaseLib.Utils;
using DemonHunterTestCode.Cards;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace DemonHunterTest.DemonHunterTestCode.Relics;

[Pool(typeof(DemonHunterTestRelicPool))]
public sealed class SourGrapes : DemonHunterTestRelic
{
    public override RelicRarity Rarity => RelicRarity.Rare;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromCardWithCardHoverTips<MaievTheWarden>();

    public override async Task AfterObtained()
    {
        if (Owner != null)
        {
            CardModel maiev = Owner.RunState.CreateCard(ModelDb.Card<MaievTheWarden>(), Owner);
            var addResult = await CardPileCmd.Add(maiev, PileType.Deck);
            CardCmd.PreviewCardPileAdd(new List<CardPileAddResult> { addResult }, 2f);
            
            await RelicCmd.Obtain<CrystalPrison>(Owner);
        }
    }
}

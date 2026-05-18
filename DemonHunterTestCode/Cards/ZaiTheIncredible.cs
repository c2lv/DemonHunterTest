using System.Threading.Tasks;
using BaseLib.Utils;
using DemonHunterTest.DemonHunterTestCode.Character;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace DemonHunterTestCode.Cards;

[Pool(typeof(DemonHunterTestCardPool))]
public sealed class ZaiTheIncredible : DemonHunterTestCard
{
    public ZaiTheIncredible()
        : base(3, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CardPile hand = PileType.Hand.GetPile(base.Owner);
        if (hand.Cards.Count == 0) return;

        // 손의 맨 왼쪽과 맨 오른쪽 카드 식별
        CardModel leftmost = hand.Cards[0];
        CardModel rightmost = hand.Cards[hand.Cards.Count - 1];

        // 맨 왼쪽 카드 복사 및 추가
        CardModel leftClone = leftmost.CreateClone();
        await AddGeneratedCardToHandLeftmost(leftClone);

        // 맨 오른쪽 카드 복사 및 추가
        CardModel rightClone = rightmost.CreateClone();
        await CardPileCmd.AddGeneratedCardToCombat(rightClone, PileType.Hand, null);
    }

    protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }
}

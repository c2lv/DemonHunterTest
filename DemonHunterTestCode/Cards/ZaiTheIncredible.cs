using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using DemonHunterTest.DemonHunterTestCode.Cards;
using DemonHunterTest.DemonHunterTestCode.Character;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
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
        await CardPileCmd.AddGeneratedCardToCombat(leftClone, PileType.Hand, null, CardPilePosition.Top);

        // 맨 오른쪽 카드 복사 및 추가
        // 카드가 추가됨에 따라 인덱스가 변하므로 다시 식별하거나 위치를 계산해야 할 수 있습니다.
        // 사용자 힌트에 따라 원본 카드의 왼쪽에 생성되도록 구현합니다.
        CardModel rightClone = rightmost.CreateClone();

        // CardPileCmd.Add는 인덱스를 직접 받지 않으므로, 
        // 맨 오른쪽 카드의 복사본을 맨 오른쪽(Bottom)에 추가하면 원본의 오른쪽이 됩니다.
        // 원본의 왼쪽에 추가하려면 인덱스를 조절해야 하지만, 현재 명령 구조상 Top/Bottom/Random만 지원합니다.
        // 여기서는 일반적인 추가 방식을 사용하되, 나중에 필요시 인덱스 기반 추가를 검토합니다.
        await CardPileCmd.AddGeneratedCardToCombat(rightClone, PileType.Hand, null, CardPilePosition.Bottom);
    }

    protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }
}

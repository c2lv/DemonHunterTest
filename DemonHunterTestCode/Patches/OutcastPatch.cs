using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Cards;
using DemonHunterTest.DemonHunterTestCode.Cards;
using System.Linq;

namespace DemonHunterTest.DemonHunterTestCode.Patches;

[HarmonyPatch]
public static class OutcastPatch
{
    // 추방자 카드 사용 가능 여부 체크 패치 (IsPlayable)
    [HarmonyPatch(typeof(CardModel), "IsPlayable", MethodType.Getter)]
    [HarmonyPostfix]
    public static void IsPlayablePostfix(CardModel __instance, ref bool __result)
    {
        // 이미 사용 불가 상태라면 무시
        if (!__result) return;

        // 추방자 키워드가 있는 경우에만 체크
        if (__instance.Keywords.Contains(DHKeyWords.Outcast))
        {
            // 손패에 있고, 양 끝이 아니면 사용 불가 처리
            if (__instance.Pile?.Type == PileType.Hand)
            {
                if (!IsAtEdgeOfHand(__instance))
                {
                    __result = false;
                }
            }
        }
    }

    // 추방자 카드가 사용 가능할 때만 후광 표시 (ShouldGlowGoldInternal)
    [HarmonyPatch(typeof(CardModel), "ShouldGlowGoldInternal", MethodType.Getter)]
    [HarmonyPostfix]
    public static void ShouldGlowGoldInternalPostfix(CardModel __instance, ref bool __result)
    {
        // 추방자 키워드가 있는 경우에만 체크
        if (__instance.Keywords.Contains(DHKeyWords.Outcast))
        {
            if (__instance.Pile?.Type == PileType.Hand)
            {
                if (IsAtEdgeOfHand(__instance))
                {
                    __result = true;  // 손패 끝에 있으면 후광 활성화
                }
                else
                {
                    __result = false;  // 손패 중간에 있으면 후광 비활성화
                }
            }
        }
    }

    private static bool IsAtEdgeOfHand(CardModel card)
    {
        var hand = card.Pile;
        if (hand == null || hand.Type != PileType.Hand) return false;

        var cards = hand.Cards;
        if (cards.Count == 0) return false;

        // 맨 왼쪽(0) 또는 맨 오른쪽(Count-1)인지 확인
        return card == cards[0] || card == cards[cards.Count - 1];
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using DemonHunterTest.DemonHunterTestCode.Cards;
using DemonHunterTest.DemonHunterTestCode.Character;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace DemonHunterTestCode.Cards;

[Pool(typeof(DemonHunterTestCardPool))]
public sealed class EldritchBeing : DemonHunterTestCard
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [DHKeyWords.Outcast];

    public EldritchBeing()
        : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        List<CardModel> handCards = PileType.Hand.GetPile(base.Owner).Cards.Where((CardModel c) => c != this).ToList();
        List<CardModel> shuffled = handCards.StableShuffle(base.Owner.RunState.Rng.Shuffle);

        NPlayerHand? handUi = NCombatRoom.Instance?.Ui.Hand;
        if (handUi != null)
        {
            foreach (CardModel card in shuffled)
            {
                NCardHolder? holder = handUi.GetCardHolder(card);
                if (holder is NHandCardHolder)
                {
                    handUi.CardHolderContainer.MoveChildSafely(holder, -1);
                }
            }

            handUi.ForceRefreshCardIndices();
        }

        await Task.CompletedTask;
    }

    protected override void OnUpgrade()
    {
        RemoveKeyword(DHKeyWords.Outcast);
    }
}

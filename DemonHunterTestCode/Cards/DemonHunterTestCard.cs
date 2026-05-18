using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using DemonHunterTest.DemonHunterTestCode.Character;
using DemonHunterTest.DemonHunterTestCode.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using System.Linq;
using System.Threading.Tasks;

namespace DemonHunterTestCode.Cards;

[Pool(typeof(DemonHunterTestCardPool))]
public abstract class DemonHunterTestCard(int cost, CardType type, CardRarity rarity, TargetType target, bool showInCardLibrary = true, bool autoAdd = true) :
    CustomCardModel(cost, type, rarity, target, showInCardLibrary, autoAdd)
{
    //Image size:
    //Normal art: 1000x760 (Using 500x380 should also work, it will simply be scaled.)
    //Full art: 606x852
    public override string CustomPortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigCardImagePath();

    //Smaller variants of card images for efficiency:
    //Smaller variant of fullart: 250x350
    //Smaller variant of normalart: 250x190

    //Uses card_portraits/card_name.png as image path. These should be smaller images.
    public override string PortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();

    public bool IsHighlander =>
        Owner != null &&
        CombatManager.Instance.IsInProgress &&
        CardPile.GetCards(Owner, PileType.Draw)
            .GroupBy(card => card.Id)
            .All(group => group.Count() == 1);

    public bool IsExhaustable =>
        Owner != null &&
        CombatManager.Instance.IsInProgress &&
        CardPile.GetCards(Owner, PileType.Draw)
            .Any(card => card is SoulFragment);

    protected async Task<CardPileAddResult> AddToHandLeftmost(CardModel card, CardPilePosition position = CardPilePosition.Top, AbstractModel? source = null, bool skipVisuals = false)
    {
        CardPileAddResult addResult = await CardPileCmd.Add(card, PileType.Hand, position, source, skipVisuals);
        if (!addResult.success)
        {
            return addResult;
        }

        NPlayerHand? handUi = NCombatRoom.Instance?.Ui.Hand;
        NCardHolder? holder = handUi?.GetCardHolder(addResult.cardAdded);
        if (handUi is not null && holder is NHandCardHolder)
        {
            handUi.CardHolderContainer.MoveChildSafely(holder, 0);
            handUi.ForceRefreshCardIndices();
        }

        return addResult;
    }

    protected async Task AddGeneratedCardToHandLeftmost(CardModel card)
{
    await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, null, CardPilePosition.Top);
    
    NPlayerHand? handUi = NCombatRoom.Instance?.Ui.Hand;
    NCardHolder? holder = handUi?.GetCardHolder(card);
    if (handUi is not null && holder is NHandCardHolder)
    {
        handUi.CardHolderContainer.MoveChildSafely(holder, 0);
        handUi.ForceRefreshCardIndices();
    }
}
}
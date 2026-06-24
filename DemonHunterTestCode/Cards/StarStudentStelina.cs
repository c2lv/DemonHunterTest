using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using DemonHunterTest.DemonHunterTestCode.Character;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace DemonHunterTestCode.Cards;

[Pool(typeof(DemonHunterTestCardPool))]
public sealed class StarStudentStelina : DemonHunterTestCard
{
    public StarStudentStelina()
        : base(2, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        List<CardModel> discardCards = PileType.Discard.GetPile(base.Owner).Cards.ToList();
        if (discardCards.Count == 0)
        {
            return;
        }

        List<CardModel> cardsToChooseFrom = discardCards.StableShuffle(base.Owner.RunState.Rng.CombatCardSelection).Take(3).ToList();
        CardModel? chosen = (await CardSelectCmd.FromSimpleGrid(
            choiceContext, cardsToChooseFrom, base.Owner, new CardSelectorPrefs(base.SelectionScreenPrompt, 1)
        )).FirstOrDefault();
        if (chosen != null)
        {
            await CardPileCmd.Add(chosen, PileType.Draw, CardPilePosition.Random, this);
        }
    }

    protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }
}

using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace DemonHunterTest.DemonHunterTestCode.Cards;

public class DHKeyWords
{
    [CustomEnum(null)]
    [KeywordProperties(AutoKeywordPosition.Before, true)]
    public static CardKeyword Outcast;

    [CustomEnum(null)]
    [KeywordProperties(AutoKeywordPosition.Before, true)]
    public static CardKeyword Highlander;

    [CustomEnum(null)]
    [KeywordProperties(AutoKeywordPosition.Before, true)]
    public static CardKeyword Taunt;
}

using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using DemonHunterTest.DemonHunterTestCode.Character;
using DemonHunterTest.DemonHunterTestCode.Extensions;

namespace DemonHunterTest.DemonHunterTestCode.Potions;

[Pool(typeof(DemonHunterTestPotionPool))]
public abstract class DemonHunterTestPotion : CustomPotionModel
{
    public override string? CustomPackedImagePath =>  
    $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PotionImagePath();
	public override string? CustomPackedOutlinePath => 
    $"{Id.Entry.RemovePrefix().ToLowerInvariant()}_outline.png".PotionImagePath();
}
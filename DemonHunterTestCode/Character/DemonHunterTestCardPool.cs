using BaseLib.Abstracts;
using DemonHunterTest.DemonHunterTestCode.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;

namespace DemonHunterTest.DemonHunterTestCode.Character;

public class DemonHunterTestCardPool : CustomCardPoolModel
{
    public override string Title => DemonHunterTest.CharacterId; //This is not a display name.
    
    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();

    // 아이언클래드 frame에 적용된 hsv: HSV (0.3457, 0.6171, 0.6863)


    /* These HSV values will determine the color of your card back.
    They are applied as a shader onto an already colored image,
    so it may take some experimentation to find a color you like.
    Generally they should be values between 0 and 1. */
    public override float H => 1f; //Hue; changes the color.
    public override float S => 1f; //Saturation
    public override float V => 1f; //Brightness

    //Alternatively, leave these values at 1 and provide a custom frame image.
    public override Texture2D? CustomFrame(CustomCardModel card)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Invalid comparison between Unknown and I4
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
        // //This will attempt to load DemonHunterTest/images/cards/frame.png
        // return PreloadManager.Cache.GetTexture2D("cards/frame.png".ImagePath());
		CardType type = ((CardModel)card).Type;
		if (1 == 0)
		{
		}
		Texture2D result =
            ((int)type == 1) ? PreloadManager.Cache.GetAsset<Texture2D>("card_frame_attack_s.png".ImagePath())
            : (((int)type != 3) ? PreloadManager.Cache.GetAsset<Texture2D>("card_frame_skill_s.png".ImagePath())
            : PreloadManager.Cache.GetAsset<Texture2D>("card_frame_power_s.png".ImagePath()))
        ;
		if (1 == 0)
		{
		}
		return result;
	}

    //Color of small card icons
    public override Color DeckEntryCardColor => new("142e0b");
    
    public override bool IsColorless => false;
}
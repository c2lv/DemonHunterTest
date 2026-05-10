using System.ComponentModel;
using Godot;
using Godot.Bridge;
using MegaCrit.Sts2.Core.Nodes.Screens.Shops;

[GlobalClass]
public partial class DemonHunterTestMerchant : NMerchantCharacter
{
	public override void _Ready()
	{
		// Intentionally do nothing: this custom merchant scene uses a Sprite2D visual
		// instead of the standard SpineSprite merchant animation.
	}
}

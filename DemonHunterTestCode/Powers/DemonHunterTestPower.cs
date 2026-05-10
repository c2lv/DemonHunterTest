using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Powers;
using BaseLib.Extensions;
using DemonHunterTest.DemonHunterTestCode.Extensions;
using Godot;

namespace DemonHunterTest.DemonHunterTestCode.Powers;

public abstract class DemonHunterTestPower : CustomPowerModel
{
    //Loads from DemonHunterTest/images/powers/your_power.png
    public override string CustomPackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
    public override string CustomBigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
}
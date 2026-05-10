using BaseLib.Abstracts;
using DemonHunterTest.DemonHunterTestCode.Extensions;
using Godot;

namespace DemonHunterTest.DemonHunterTestCode.Character;

public class DemonHunterTestRelicPool : CustomRelicPoolModel
{
    public override Color LabOutlineColor => DemonHunterTest.Color;

    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}
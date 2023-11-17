using Content.Server.SurveillanceCamera;
using Content.Shared.Clothing.Systems;

namespace Content.Server.SurveillanceCamera;

[RegisterComponent]
[Access(typeof(SecurityBodyCameraSystem), typeof(BodyCameraSlotSystem))]
public sealed partial class SecurityBodyCameraComponent : Component
{
    [DataField("wattage"), ViewVariables(VVAccess.ReadWrite)]
    public float Wattage = 0.3f;

    public bool lastState = false;
}


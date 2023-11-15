using Content.Server.SurveillanceCamera;

namespace Content.Server.SurveillanceCamera;

[RegisterComponent]
[Access(typeof(SecurityBodyCameraSystem))]
public sealed partial class SecurityBodyCameraComponent : Component
{
    [DataField("wattage"), ViewVariables(VVAccess.ReadWrite)]
    public float Wattage = 1f;

    public bool lastState = false;
}


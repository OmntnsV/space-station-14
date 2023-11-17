namespace Content.Client.SurveillanceCamera;

[RegisterComponent]
public sealed partial class SecurityBodyCameraVisualsComponent : Component
{
    public bool IsPowered;

    public bool isActive;
}

public enum BodyCameraVisuals : byte
{
    Unpowered,
    Inactive,
    Active
}

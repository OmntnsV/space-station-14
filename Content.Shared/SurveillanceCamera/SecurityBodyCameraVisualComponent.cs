using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.SurveillanceCamera;
[RegisterComponent, NetworkedComponent]
public sealed partial class SecurityBodyCameraVisualComponent : Component
{

}
[Serializable, NetSerializable]
public enum SharedBodyCameraVisuals : byte
{
    Active
}

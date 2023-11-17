using Content.Shared.Clothing.Systems;
using Content.Shared.Containers.ItemSlots;

namespace Content.Shared.Clothing.Components;

[RegisterComponent]
[Access(typeof(BodyCameraSlotSystem))]
public sealed partial class BodyCameraSlotComponent : Component
{
    public static string BodyCameraSlot = "jumpsuit-camera";

    [DataField]
    public ItemSlot CameraSlot = new();
}

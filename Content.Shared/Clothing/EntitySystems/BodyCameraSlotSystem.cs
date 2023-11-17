using Content.Shared.Clothing.Components;
using Content.Shared.Containers.ItemSlots;
using Content.Shared.Whitelist;

namespace Content.Shared.Clothing.Systems;

public sealed class BodyCameraSlotSystem : EntitySystem
{
    [Dependency] private readonly ItemSlotsSystem _itemSlotsSystem = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<BodyCameraSlotComponent, ComponentInit>(OnInit);
        SubscribeLocalEvent<BodyCameraSlotComponent, ComponentRemove>(OnRemove);
    }

    public void OnInit(EntityUid uid, BodyCameraSlotComponent component, ComponentInit args)
    {
        EntityWhitelist entityWhitelist = new EntityWhitelist();
        entityWhitelist.Components = new string[] {"SecurityBodyCamera"};

        component.CameraSlot.Whitelist = entityWhitelist;
        _itemSlotsSystem.AddItemSlot(uid, BodyCameraSlotComponent.BodyCameraSlot, component.CameraSlot);
    }

    public void OnRemove(EntityUid uid, BodyCameraSlotComponent component, ComponentRemove args)
    {
        _itemSlotsSystem.RemoveItemSlot(uid, component.CameraSlot);
    }
}

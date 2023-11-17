using Content.Shared.Rounding;
using Content.Shared.Weapons.Ranged.Systems;
using Robust.Client.GameObjects;

namespace Content.Client.SurveillanceCamera;

public sealed partial class SecurityBodyCameraVisualsSystem : EntitySystem
{
    private void InitializeMagazineVisuals()
    {
        SubscribeLocalEvent<SecurityBodyCameraVisualsComponent, ComponentInit>(OnBodycamVisualsInit);
        SubscribeLocalEvent<SecurityBodyCameraVisualsComponent, AppearanceChangeEvent>(OnBodyCameraVisualsChange);
    }

    private void OnBodycamVisualsInit(EntityUid uid, SecurityBodyCameraVisualsComponent component, ComponentInit args)
    {
        if (!TryComp<SpriteComponent>(uid, out var sprite)) return;

        if (sprite.LayerMapTryGet(BodyCameraVisuals.Active, out _))
        {
            sprite.LayerSetState(BodyCameraVisuals.Active, "active");
            sprite.LayerSetVisible(BodyCameraVisuals.Active, false);
        }

        if (sprite.LayerMapTryGet(BodyCameraVisuals.Inactive, out _))
        {
            sprite.LayerSetState(BodyCameraVisuals.Inactive, "inactive");
            sprite.LayerSetVisible(BodyCameraVisuals.Inactive, false);
        }
    }

    private void OnBodyCameraVisualsChange(EntityUid uid, SecurityBodyCameraVisualsComponent component, ref AppearanceChangeEvent args)
    {
        // tl;dr
        // 1.If no mag then hide it OR
        // 2. If step 0 isn't visible then hide it (mag or unshaded)
        // 3. Otherwise just do mag / unshaded as is
        var sprite = args.Sprite;

        if (sprite == null) return;


        if (!TryComp<SurveillanceCameraComponent>(uid, out var surComp))
            return;
        if (surComp.Active)
        {

        }
        // {
        //     if (sprite.LayerMapTryGet(BodyCameraVisuals.Active, out _))
        //         return;
        //     {
        //         sprite.LayerSetVisible(BodyCameraVisuals.Active, true);
        //     }
        // }

        // if (!args.AppearanceData.TryGetValue(BodyCameraVisuals.Inactive, out var inactive) ||
        //     inactive is true)
        // {
        //     if (!args.AppearanceData.TryGetValue(AmmoVisuals.AmmoMax, out var capacity))
        //     {
        //         capacity = component.MagSteps;
        //     }
        //
        //     if (!args.AppearanceData.TryGetValue(AmmoVisuals.AmmoCount, out var current))
        //     {
        //         current = component.MagSteps;
        //     }
        //
        //     var step = ContentHelpers.RoundToLevels((int) current, (int) capacity, component.MagSteps);
        //
        //     if (step == 0 && !component.ZeroVisible)
        //     {
        //         if (sprite.LayerMapTryGet(GunVisualLayers.Mag, out _))
        //         {
        //             sprite.LayerSetVisible(GunVisualLayers.Mag, false);
        //         }
        //
        //         if (sprite.LayerMapTryGet(GunVisualLayers.MagUnshaded, out _))
        //         {
        //             sprite.LayerSetVisible(GunVisualLayers.MagUnshaded, false);
        //         }
        //
        //         return;
        //     }
        //
        //     if (sprite.LayerMapTryGet(GunVisualLayers.Mag, out _))
        //     {
        //         sprite.LayerSetVisible(GunVisualLayers.Mag, true);
        //         sprite.LayerSetState(GunVisualLayers.Mag, $"{component.MagState}-{step}");
        //     }
        //
        //     if (sprite.LayerMapTryGet(GunVisualLayers.MagUnshaded, out _))
        //     {
        //         sprite.LayerSetVisible(GunVisualLayers.MagUnshaded, true);
        //         sprite.LayerSetState(GunVisualLayers.MagUnshaded, $"{component.MagState}-unshaded-{step}");
        //     }
        // }
        // else
        // {
        //     if (sprite.LayerMapTryGet(GunVisualLayers.Mag, out _))
        //     {
        //         sprite.LayerSetVisible(GunVisualLayers.Mag, false);
        //     }
        //
        //     if (sprite.LayerMapTryGet(GunVisualLayers.MagUnshaded, out _))
        //     {
        //         sprite.LayerSetVisible(GunVisualLayers.MagUnshaded, false);
        //     }
        // }
    }
}

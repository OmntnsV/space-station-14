using Content.Server.Popups;
using Content.Server.PowerCell;
using Content.Shared.Examine;
using Content.Shared.Interaction;
using Content.Shared.Item;
using Content.Shared.PowerCell.Components;
using Content.Shared.Toggleable;

namespace Content.Server.SurveillanceCamera;

public sealed class SecurityBodyCameraSystem : EntitySystem
{
    [Dependency] private readonly PowerCellSystem _powerCell = default!;
    [Dependency] private readonly PopupSystem _popup = default!;
    [Dependency] private readonly SurveillanceCameraSystem _surveillanceCameras = default!;
    [Dependency] private readonly SharedAppearanceSystem _appearance = default!;
    [Dependency] private readonly SharedItemSystem _item = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<SecurityBodyCameraComponent, ActivateInWorldEvent>(OnActivate);
        SubscribeLocalEvent<SecurityBodyCameraComponent, PowerCellChangedEvent>(OnPowerCellChanged);
        SubscribeLocalEvent<SecurityBodyCameraComponent, ExaminedEvent>(OnExamine);
        SubscribeLocalEvent<SecurityBodyCameraComponent, ComponentInit>(OnInit);
    }

    public void OnInit(EntityUid uid, SecurityBodyCameraComponent comp, ComponentInit args)
    {

        if (!TryComp<SurveillanceCameraComponent>(uid, out var surComp))
            return;

        _surveillanceCameras.SetActive(uid, false, surComp);
        AppearanceChange(uid, surComp.Active);
    }
    public override void Update(float frameTime)
    {
        var query = EntityQueryEnumerator<SecurityBodyCameraComponent>();
        while (query.MoveNext(out var uid, out var cam))
        {
            if (!_powerCell.TryGetBatteryFromSlot(uid, out var battery))
                continue;

            if (!TryComp<SurveillanceCameraComponent>(uid, out var surComp))
                continue;

            if (!surComp.Active)
                continue;

            if (!battery.TryUseCharge(cam.Wattage * frameTime))
            {
                _surveillanceCameras.SetActive(uid, false, surComp);
                AppearanceChange(uid, surComp.Active);
            }
        }
    }
    public void OnActivate(EntityUid uid, SecurityBodyCameraComponent comp, ActivateInWorldEvent args)
    {
        if (!TryComp<SurveillanceCameraComponent>(uid, out var surComp))
            return;

        if (!_powerCell.TryGetBatteryFromSlot(uid, out var battery))
            return;

        _surveillanceCameras.SetActive(uid, battery.CurrentCharge > comp.Wattage && !surComp.Active, surComp);
        AppearanceChange(uid, surComp.Active);

        var message = "Body camera is " + (surComp.Active ? "ON": "OFF");
        _popup.PopupEntity(message, args.User, args.User);
        args.Handled = true;
    }

    public void OnPowerCellChanged(EntityUid uid, SecurityBodyCameraComponent comp, PowerCellChangedEvent args)
    {
        if (!TryComp<SurveillanceCameraComponent>(uid, out var surComp))
            return;

        if (args.Ejected)
        {
            _surveillanceCameras.SetActive(uid, false, surComp);
            AppearanceChange(uid, surComp.Active);
        }
    }

    public void OnExamine(EntityUid uid, SecurityBodyCameraComponent comp, ExaminedEvent args)
    {
        if (!TryComp<SurveillanceCameraComponent>(uid, out var surComp))
            return;

        if (args.IsInDetailsRange)
        {
            var message = "Body camera is " +  (surComp.Active ? "ON!" : "OFF!");
            args.PushMarkup(message);
        }
    }

    public void AppearanceChange(EntityUid uid, Boolean isActive)
    {
        if (TryComp<AppearanceComponent>(uid, out var appearance) &&
            TryComp<ItemComponent>(uid, out var item))
    {
        _item.SetHeldPrefix(uid, isActive ? "on" : "off", item);
        _appearance.SetData(uid, ToggleVisuals.Toggled, isActive, appearance);
    }
    }
}

// TODO: LOCALIZATION

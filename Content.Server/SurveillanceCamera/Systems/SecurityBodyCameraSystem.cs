using System.Diagnostics.CodeAnalysis;
using System.Security.AccessControl;
using Content.Server.DeviceNetwork.Components;
using Content.Server.Popups;
using Content.Server.PowerCell;
using Content.Shared.Examine;
using Content.Shared.Interaction;
using Content.Shared.PowerCell.Components;

namespace Content.Server.SurveillanceCamera;

public sealed class SecurityBodyCameraSystem : EntitySystem
{
    [Dependency] private readonly PowerCellSystem _powerCell = default!;
    [Dependency] private readonly PopupSystem _popup = default!;
    [Dependency] private readonly SurveillanceCameraSystem _surveillanceCameras = default!;

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
                _surveillanceCameras.SetActive(uid, false, surComp);
        }
    }
    public void OnActivate(EntityUid uid, SecurityBodyCameraComponent comp, ActivateInWorldEvent args)
    {
        if (!TryComp<SurveillanceCameraComponent>(uid, out var surveillanceCameraComponent))
            return;

        if (!_powerCell.TryGetBatteryFromSlot(uid, out var battery))
            return;

        _surveillanceCameras.SetActive(uid, battery.CurrentCharge > comp.Wattage && !surveillanceCameraComponent.Active, surveillanceCameraComponent);

        var message = "Body camera is " + (surveillanceCameraComponent.Active ? "ON": "OFF");
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
        }
        // else if (_powerCell.TryGetBatteryFromSlot(uid, out var battery))
        // {
        //     _surveillanceCameras.SetActive(uid, battery.CurrentCharge > comp.Wattage, surComp);
        // }
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
}

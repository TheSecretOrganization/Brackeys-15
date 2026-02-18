using Godot;
using System;

public partial class Lights : Node2D
{

    [Export] public PointLight2D LightAura;
    [Export] public PointLight2D Flashlight;
    [Export] public Timer FlashlightTimer;

    private const float FLASHLIGHT_FLASHBANG_SCALE = 6f;
    private static readonly Vector2 FLASHLIGHT_FLASHBANG_OFFSET = new Vector2(90, 0);


    private Vector2 _defaultFlashlightOffset;
    private float _defaultFlashlightScale;

    public override void _Ready()
    {
        base._Ready();
        Flashlight.ZIndex = 1;
        LightAura.ZIndex = 0;

        FlashlightTimer.OneShot = true;
        FlashlightTimer.Timeout -= OnTimerTimeout;
        FlashlightTimer.Timeout += OnTimerTimeout;

        _defaultFlashlightOffset = Flashlight.Offset;
        _defaultFlashlightScale = Flashlight.TextureScale;
    }

    void OnTimerTimeout()
    {
        Flashlight.SetTextureScale(_defaultFlashlightScale);
        Flashlight.Offset = _defaultFlashlightOffset;
    }

    public override void _Process(double delta)
    {
        UpdateFlashlightDirection();
    }

    void UpdateFlashlightDirection()
    {
        Vector2 mousePosition = GetGlobalMousePosition();
        LightAura.LookAt(mousePosition);
        Flashlight.LookAt(mousePosition);

        if (Input.IsActionJustPressed("flashlight") && FlashlightTimer.IsStopped())
        {
            Flashlight.SetTextureScale(FLASHLIGHT_FLASHBANG_SCALE);
            Flashlight.Offset = FLASHLIGHT_FLASHBANG_OFFSET;
            FlashlightTimer.Start();
        }
    }
}

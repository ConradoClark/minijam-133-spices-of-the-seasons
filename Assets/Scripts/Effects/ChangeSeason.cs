using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Orchestration;
using Licht.Unity.Builders;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ChangeSeason : BaseGameRunner
{
    [field:SerializeField]
    public Volume Volume{ get; private set; }

    [field: Header("Default Seasons Material")]

    [field: SerializeField]
    public Material Seasons { get; private set; }

    [field:Header("Default Season Colors")]
    [field: SerializeField]
    public Material Summer { get; private set; }
    [field: SerializeField]
    public Material Winter { get; private set; }
    [field: SerializeField]
    public Material Spring { get; private set; }
    [field: SerializeField]
    public Material Fall { get; private set; }

    [field: Header("Default Camera Colors")]
    [field: SerializeField]
    public Color SummerCamera { get; private set; }
    [field: SerializeField]
    public Color WinterCamera { get; private set; }
    [field: SerializeField]
    public Color SpringCamera { get; private set; }
    [field: SerializeField]
    public Color FallCamera { get; private set; }

    [field: SerializeField]
    public Seasons CurrentSeason { get; set; }

    [field: SerializeField]
    public SpriteRenderer PillarL { get; set; }

    [field: SerializeField]
    public SpriteRenderer PillarR { get; set; }

    public event Action<Seasons> OnSeasonChanged;

    public struct SeasonColors
    {
        public Color DarkestColor;
        public Color DarkColor;
        public Color NeutralColor;
        public Color LightColor;
        public Color LightestColor;
    }

    private ColorAdjustments _colorAdjustments;
    private SeasonColors _summerColors;
    private SeasonColors _winterColors;
    private SeasonColors _springColors;
    private SeasonColors _fallColors;

    private Player _player;
    private Camera _gameCamera;

    private SeasonColors GetColorsFromMaterial(Material mat)
    {
        return new SeasonColors
        {
            DarkestColor = mat.GetColor("_ReplaceDarkest"),
            DarkColor = mat.GetColor("_ReplaceDark"),
            NeutralColor = mat.GetColor("_ReplaceNeutral"),
            LightColor = mat.GetColor("_ReplaceLight"),
            LightestColor = mat.GetColor("_ReplaceLightest"),
        };
    }

    private SeasonColors GetColorsFromSeason(Seasons season)
    {
        return season switch
        {
            global::Seasons.Summer => _summerColors,
            global::Seasons.Winter => _winterColors,
            global::Seasons.Spring => _springColors,
            global::Seasons.Fall => _fallColors,
            _ => _summerColors
        };
    }

    private Color GetBackgroundColorFromSeason(Seasons season)
    {
        return season switch
        {
            global::Seasons.Summer => SummerCamera,
            global::Seasons.Winter => WinterCamera,
            global::Seasons.Spring => SpringCamera,
            global::Seasons.Fall => FallCamera,
            _ => SummerCamera
        };
    }

    private void InitializeSeason()
    {
        var colors = GetColorsFromSeason(CurrentSeason);

        Seasons.SetColor("_InitialDarkest", colors.DarkestColor);
        Seasons.SetColor("_InitialDark", colors.DarkColor);
        Seasons.SetColor("_InitialNeutral", colors.NeutralColor);
        Seasons.SetColor("_InitialLight", colors.LightColor);
        Seasons.SetColor("_InitialLightest", colors.LightestColor);

        Seasons.SetColor("_ReplaceDarkest", colors.DarkestColor);
        Seasons.SetColor("_ReplaceDark", colors.DarkColor);
        Seasons.SetColor("_ReplaceNeutral", colors.NeutralColor);
        Seasons.SetColor("_ReplaceLight", colors.LightColor);
        Seasons.SetColor("_ReplaceLightest", colors.LightestColor);

        Seasons.SetFloat("_EffectIntensity", 1f);

        var currentColor = GetBackgroundColorFromSeason(CurrentSeason);

        _gameCamera.backgroundColor = currentColor;
    }

    protected override void OnAwake()
    {
        base.OnAwake();
        Volume.profile.TryGet(out _colorAdjustments);
        _summerColors = GetColorsFromMaterial(Summer);
        _winterColors = GetColorsFromMaterial(Winter);
        _springColors = GetColorsFromMaterial(Spring);
        _fallColors = GetColorsFromMaterial(Fall);
        _player = _player.FromScene();
        _gameCamera = Camera.main;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        InitializeSeason();
    }

    protected override IEnumerable<IEnumerable<Action>> Handle()
    {
        var targetSeason = CurrentSeason == global::Seasons.Summer ? global::Seasons.Winter : global::Seasons.Summer;
        yield return Flash().AsCoroutine().Combine(
            Change(targetSeason).AsCoroutine())
            .Combine(ChangeBackgroundColor(targetSeason).AsCoroutine());
        yield return TimeYields.WaitOneFrameX;
    }

    public IEnumerable<IEnumerable<Action>> ChangeTo(Seasons season)
    {
        yield return Flash().AsCoroutine().Combine(
                Change(season).AsCoroutine())
            .Combine(ChangeBackgroundColor(season).AsCoroutine());
    }

    private IEnumerable<IEnumerable<Action>> Change(Seasons season)
    {
        var currentColors = GetColorsFromSeason(CurrentSeason);
        Seasons.SetColor("_InitialDarkest", currentColors.DarkestColor);
        Seasons.SetColor("_InitialDark", currentColors.DarkColor);
        Seasons.SetColor("_InitialNeutral", currentColors.NeutralColor);
        Seasons.SetColor("_InitialLight", currentColors.LightColor);
        Seasons.SetColor("_InitialLightest", currentColors.LightestColor);

        Seasons.SetFloat("_EffectIntensity", 0f);

        var replaceColors = GetColorsFromSeason(season);
        Seasons.SetColor("_ReplaceDarkest", replaceColors.DarkestColor);
        Seasons.SetColor("_ReplaceDark", replaceColors.DarkColor);
        Seasons.SetColor("_ReplaceNeutral", replaceColors.NeutralColor);
        Seasons.SetColor("_ReplaceLight", replaceColors.LightColor);
        Seasons.SetColor("_ReplaceLightest", replaceColors.LightestColor);

        PillarL.enabled = PillarR.enabled = true;

        var initialX = _player.transform.position.x;
        PillarL.transform.position = PillarR.transform.position = 
            new Vector3(initialX, PillarL.transform.position.y);

        var halfScreenWidthUnits = _gameCamera.orthographicSize * _gameCamera.aspect;
        PillarL.color = PillarR.color = Color.white;

        yield return new LerpBuilder()
            .SetTarget(1)
            .OnEachStep(val =>
            {
                Seasons.SetVector("_EffectCenter", (Vector2) _player.transform.position);
                Seasons.SetFloat("_EffectIntensity", val);

                PillarL.transform.position = 
                    new Vector3(initialX - val * halfScreenWidthUnits, PillarL.transform.position.y);

                PillarR.transform.position =
                    new Vector3(initialX + val * halfScreenWidthUnits, PillarL.transform.position.y);

                PillarL.color = PillarR.color = new Color(1, 1, 1, Mathf.Lerp(1, 0, val));
            })
            .Over(1)
            .Easing(EasingYields.EasingFunction.QuadraticEaseOut)
            .Build();

        Seasons.SetColor("_InitialDarkest", replaceColors.DarkestColor);
        Seasons.SetColor("_InitialDark", replaceColors.DarkColor);
        Seasons.SetColor("_InitialNeutral", replaceColors.NeutralColor);
        Seasons.SetColor("_InitialLight", replaceColors.LightColor);
        Seasons.SetColor("_InitialLightest", replaceColors.LightestColor);

        Seasons.SetFloat("_EffectIntensity", 1f);
        CurrentSeason = season;
        PillarL.enabled = PillarR.enabled = false;
        OnSeasonChanged?.Invoke(season);
    }

    private IEnumerable<IEnumerable<Action>> ChangeBackgroundColor(Seasons targetSeason)
    {
        var current = CurrentSeason;
        var currentColor = GetBackgroundColorFromSeason(current);
        var targetColor = GetBackgroundColorFromSeason(targetSeason);

        yield return new LerpBuilder()
            .SetTarget(1)
            .OnEachStep(val => _gameCamera.backgroundColor = Color.Lerp(currentColor, targetColor, val))
            .Over(1)
            .Easing(EasingYields.EasingFunction.QuadraticEaseIn)
            .Build();
    }

    private IEnumerable<IEnumerable<Action>> Flash()
    {
        yield return new LerpBuilder()
            .SetTarget(1)
            .OnEachStep(val => _colorAdjustments.postExposure.Override(3f * val))
            .Over(0.15f)
            .Easing(EasingYields.EasingFunction.QuadraticEaseIn)
            .Build();

        yield return new LerpBuilder(1)
            .SetTarget(0)
            .OnEachStep(val => _colorAdjustments.postExposure.Override(3f * val))
            .Over(0.25f)
            .Easing(EasingYields.EasingFunction.QuadraticEaseIn)
            .Build();
    }
}

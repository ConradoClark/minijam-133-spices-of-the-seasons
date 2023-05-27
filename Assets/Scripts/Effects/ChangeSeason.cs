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

    [field: SerializeField]
    public Seasons CurrentSeason { get; set; }

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
            _ => _summerColors
        };
    }

    protected override void OnAwake()
    {
        base.OnAwake();
        Volume.profile.TryGet(out _colorAdjustments);
        _summerColors = GetColorsFromMaterial(Summer);
        _winterColors = GetColorsFromMaterial(Winter);
        _player = _player.FromScene();
    }

    protected override IEnumerable<IEnumerable<Action>> Handle()
    {
        var targetSeason = CurrentSeason == global::Seasons.Summer ? global::Seasons.Winter : global::Seasons.Summer;
        yield return Flash().AsCoroutine().Combine(
            Change(targetSeason).AsCoroutine());
        yield return TimeYields.WaitOneFrameX;
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

        yield return new LerpBuilder()
            .SetTarget(1)
            .OnEachStep(val =>
            {
                Seasons.SetVector("_EffectCenter", (Vector2) _player.transform.position);
                Seasons.SetFloat("_EffectIntensity", val);
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
    }

    private IEnumerable<IEnumerable<Action>> Flash()
    {
        yield return new LerpBuilder()
            .SetTarget(1)
            .OnEachStep(val => _colorAdjustments.postExposure.Override(4f * val))
            .Over(0.25f)
            .Easing(EasingYields.EasingFunction.QuadraticEaseIn)
            .Build();

        yield return new LerpBuilder(1)
            .SetTarget(0)
            .OnEachStep(val => _colorAdjustments.postExposure.Override(4f * val))
            .Over(0.25f)
            .Easing(EasingYields.EasingFunction.QuadraticEaseIn)
            .Build();
    }
}

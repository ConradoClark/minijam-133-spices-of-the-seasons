using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using TMPro;
using UnityEngine;

public class SeasonText : BaseGameObject
{
    [field:SerializeField]
    public TMP_Text TextComponent { get; private set; }

    private ChangeSeason _seasonManager;
    private SeasonUIDefaults _uiDefaults;

    protected override void OnAwake()
    {
        base.OnAwake();
        _seasonManager = _seasonManager.FromScene();
        _uiDefaults = _uiDefaults.FromScene();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _seasonManager.OnSeasonChanged += OnSeasonChanged;
        OnSeasonChanged(_seasonManager.CurrentSeason);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _seasonManager.OnSeasonChanged -= OnSeasonChanged;
    }

    private void OnSeasonChanged(Seasons obj)
    {
        TextComponent.text = obj.ToString();

        TextComponent.color = obj switch
        {
            Seasons.Summer => _uiDefaults.SummerTextColor,
            Seasons.Winter => _uiDefaults.WinterTextColor,
            Seasons.Spring => _uiDefaults.SpringTextColor,
            Seasons.Fall => _uiDefaults.FallTextColor,
            _ => _uiDefaults.SummerTextColor
        };

    }

}

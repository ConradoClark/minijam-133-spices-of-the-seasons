using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using UnityEngine;

public class SeasonalObject : BaseGameObject
{
    [field:SerializeField]
    public Seasons Season { get; private set; }
    private ChangeSeason _seasonManager;
    private bool _justChanged;

    protected override void OnAwake()
    {
        base.OnAwake();
        _seasonManager = _seasonManager.FromScene();
        _seasonManager.OnSeasonStartedChanging += OnSeasonChanged;
        gameObject.SetActive(_seasonManager.CurrentSeason == Season);
    }

    private void OnDestroy()
    {
        _seasonManager.OnSeasonStartedChanging -= OnSeasonChanged;
    }

    private void OnSeasonChanged(Seasons obj)
    {
        _justChanged = true;
        gameObject.SetActive(obj == Season);
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        if (_justChanged)
        {
            _justChanged = false;
            return;
        }
        gameObject.SetActive(_seasonManager.CurrentSeason == Season);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;

public class PlaySoundOnSeasonChange : BaseGameObject
{
    private ChangeSeason _seasonManager;
    private SoundManager _soundManager;

    protected override void OnAwake()
    {
        base.OnAwake();
        _seasonManager = _seasonManager.FromScene();
        _soundManager = _soundManager.FromScene();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _seasonManager.OnSeasonStartedChanging += OnChange;
    }

    private void OnChange(Seasons obj)
    {
        _soundManager.PlaySeasonChangeSound();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Orchestration;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChangeSeasonOnButtonPress : BaseGameRunner
{
    [field:SerializeField]
    public InputActionReference SeasonAction { get; private set; }

    private ChangeSeason _seasonManager;
    private SeasonConfiguration _seasonConfig;

    protected override void OnAwake()
    {
        base.OnAwake();
        _seasonManager = _seasonManager.FromScene();
        _seasonConfig = _seasonConfig.FromScene();
    }

    protected override IEnumerable<IEnumerable<Action>> Handle()
    {
        while (ComponentEnabled)
        {
            if (SeasonAction.action.WasPerformedThisFrame())
            {
                var currentSeason = _seasonManager.CurrentSeason;
                var currentIndex = Array.IndexOf(_seasonConfig.Seasons, currentSeason);
                var nextSeason = _seasonConfig.Seasons[(currentIndex + 1)
                    % _seasonConfig.Seasons.Length];

                yield return _seasonManager.ChangeTo(nextSeason).AsCoroutine();
                continue;
            }

            yield return TimeYields.WaitOneFrameX;
        }
    }
}

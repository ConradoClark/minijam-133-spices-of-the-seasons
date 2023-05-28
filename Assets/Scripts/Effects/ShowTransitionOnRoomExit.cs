using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Events;
using Licht.Impl.Orchestration;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using UnityEngine;

public class ShowTransitionOnRoomExit : BaseGameObject
{
    [field:SerializeField]
    public SpriteRenderer TransitionScreen { get; private set; }
    protected override void OnEnable()
    {
        base.OnEnable();
        this.ObserveEvent<GameEvents, Room>(GameEvents.OnRoomExit, OnRoomExit);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        this.StopObservingEvent<GameEvents, Room>(GameEvents.OnRoomExit, OnRoomExit);
    }

    private void OnRoomExit(Room obj)
    {
        TransitionScreen.enabled = true;
        DefaultMachinery.AddUniqueMachine("room_transition", UniqueMachine.UniqueMachineBehaviour.Replace, FadeOut());
    }

    private IEnumerable<IEnumerable<Action>> FadeOut()
    {
        TransitionScreen.color = new Color(TransitionScreen.color.r,
            TransitionScreen.color.g, TransitionScreen.color.b, 1);

        yield return TimeYields.WaitSeconds(GameTimer, 0.2f);

        yield return TransitionScreen.GetAccessor()
            .Color
            .A
            .SetTarget(0)
            .Over(0.6f)
            .Easing(EasingYields.EasingFunction.CircularEaseIn)
            .Build();

        TransitionScreen.enabled = false;
    }
}

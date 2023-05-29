using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Orchestration;
using Licht.Unity.Objects;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class AnyKeyNextScene : BaseGameRunner
{
    [field:SerializeField]
    public InputActionReference[] Buttons { get; private set; }

    [field: SerializeField]
    public string SceneName { get; private set; }

    protected override IEnumerable<IEnumerable<Action>> Handle()
    {
        if (Buttons.Any(b => b.action.WasPerformedThisFrame()))
        {
            DefaultMachinery.FinalizeWith(() =>
            {
                SceneManager.LoadScene(SceneName);
            });
        }
        yield return TimeYields.WaitOneFrameX;

    }
}

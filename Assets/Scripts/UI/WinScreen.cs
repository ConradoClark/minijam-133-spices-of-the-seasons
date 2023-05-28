using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using UnityEngine;

public class WinScreen : BaseGameObject
{
    public SpriteRenderer SpriteRenderer { get; private set; }
    private TextCamera _textCamera;

    protected override void OnAwake()
    {
        base.OnAwake();
        _textCamera = _textCamera.FromScene();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _textCamera.Camera.enabled = false;
    }
}

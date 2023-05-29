using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using UnityEngine;

public class CharacterAnimator : BaseGameObject
{
    [field:SerializeField]
    public SpriteRenderer CharacterSpriteRenderer { get; private set; }

    private Player _player;

    protected override void OnAwake()
    {
        base.OnAwake();
        _player = _player.FromScene();
    }

    private void Update()
    {
        if (_player.MoveController.LatestDirection == 0) return;
        CharacterSpriteRenderer.flipX = _player.MoveController.LatestDirection < 0;
    }
}

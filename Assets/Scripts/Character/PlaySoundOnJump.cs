using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;

public class PlaySoundOnJump : BaseGameObject
{
    private Player _player;
    private SoundManager _soundManager;

    protected override void OnAwake()
    {
        base.OnAwake();
        _soundManager = _soundManager.FromScene();
        _player = _player.FromScene();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _player.JumpController.OnJumpStart += OnJump;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _player.JumpController.OnJumpStart -= OnJump;
    }

    private void OnJump(Licht.Unity.CharacterControllers.LichtPlatformerJumpController.LichtPlatformerJumpEventArgs obj)
    {
        _soundManager.PlayJumpSound(null);
    }
}

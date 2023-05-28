using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.CharacterControllers;
using Licht.Unity.Objects;
using UnityEngine;

public class Player: BaseGameObject
{
    [field:SerializeField] 
    public LichtPlatformerMoveController MoveController { get; private set; }

    [field: SerializeField]
    public LichtPlatformerJumpController JumpController { get; private set; }

    [field:SerializeField]
    public RoomSpawn LatestSpawn { get; set; }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Objects;
using UnityEngine;

public class RoomExit : BaseGameObject
{
    [field:SerializeField]
    public RoomSpawn RoomSpawn { get; private set; }
}

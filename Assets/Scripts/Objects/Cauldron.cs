using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Objects;
using UnityEngine;

public class Cauldron : BaseGameObject
{
    [field:SerializeField]
    public Vector2 SplashOffset { get; private set; }
}

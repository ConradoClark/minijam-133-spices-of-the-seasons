using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Objects;
using UnityEngine;

public class TextCamera : BaseGameObject
{
    [field:SerializeField]
    public Camera Camera { get; private set; }
}

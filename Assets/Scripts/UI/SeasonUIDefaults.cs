using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Objects;
using UnityEngine;

public class SeasonUIDefaults : BaseGameObject
{
    [field: SerializeField]
    public float ActiveIconOffset { get; private set; }

    [field:SerializeField]
    public Material ActiveIconMaterial { get; private set; }

    [field: SerializeField]
    public Material InactiveIconMaterial { get; private set; }

    [field: SerializeField]
    public Color SummerTextColor { get; private set; }
    [field: SerializeField]
    public Color WinterTextColor { get; private set; }
    [field: SerializeField]
    public Color SpringTextColor { get; private set; }
    [field: SerializeField]
    public Color FallTextColor { get; private set; }
}

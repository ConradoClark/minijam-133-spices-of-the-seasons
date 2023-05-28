using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Objects;
using UnityEngine;

public class SeasonConfiguration : BaseGameObject
{
    [field:SerializeField]
    public Seasons[] Seasons { get; private set; }
}

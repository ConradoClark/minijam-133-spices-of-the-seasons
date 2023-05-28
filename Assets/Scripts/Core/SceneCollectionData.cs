using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Objects;
using UnityEngine;

public class SceneCollectionData : BaseGameObject
{
    [field:SerializeField]
    public CollectablesData CollectablesData { get; private set; }
}

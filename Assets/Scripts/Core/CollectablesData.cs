using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Events;
using Licht.Unity.Objects;
using UnityEngine;

[CreateAssetMenu(fileName = "Prefab", menuName = "Spices/CollectablesData", order = 1)]
public class CollectablesData : ScriptableObject
{
    [Serializable]
    public class CollectableInfo
    {
        public ScriptIdentifier Identifier;
        public bool Collected;
    }

    [field:SerializeField]
    public List<CollectableInfo> Collection { get; private set; }

    public event Action<ScriptIdentifier> OnCollected;

    public void MarkAsCollected(ScriptIdentifier collectable)
    {
        foreach (var collectableInfo in Collection
                     .Where(collectableInfo => collectable == collectableInfo.Identifier))
        {
            collectableInfo.Collected = true;
            OnCollected?.Invoke(collectable);
            break;
        }
    }

    public bool IsCollected(ScriptIdentifier collectable)
    {
        return Collection.Any(c => c.Identifier == collectable && c.Collected);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using TMPro;
using UnityEngine;

public class PercentageUI : BaseGameObject
{
    [field:SerializeField]
    public TMP_Text TextComponent { get; private set; }

    [field: SerializeField]
    public CollectablesData OverrideCollectablesData { get; private set; }

    private SceneCollectionData _sceneCollectionData;
    protected override void OnAwake()
    {
        base.OnAwake();
        _sceneCollectionData = _sceneCollectionData.FromScene();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        UpdatePercentageString();
    }

    private void UpdatePercentageString()
    {
        var collectables = OverrideCollectablesData != null
            ? OverrideCollectablesData
            : _sceneCollectionData.CollectablesData;

        var total = collectables.Collection.Count;
        var collected = collectables.Collection.Count(c => c.Collected);
        var percentage = Mathf.RoundToInt(100 * collected / (float) total);
        TextComponent.text = $"<color=#c6f4ff>{percentage}%</color> complete!";
    }
}

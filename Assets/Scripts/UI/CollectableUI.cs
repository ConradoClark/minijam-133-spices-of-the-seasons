using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Objects;
using UnityEngine;

public class CollectableUI : BaseGameObject
{
    [field: SerializeField]
    public CollectablesData CollectablesReference { get; private set; }

    [field: SerializeField]
    public ScriptIdentifier Collectable { get; private set; }

    [field: SerializeField]
    public SpriteRenderer SpriteRenderer { get; private set; }

    [field: SerializeField]
    public Sprite CollectedSprite { get; private set; }
    [field: SerializeField]
    public Sprite MissingSprite { get; private set; }

    protected override void OnEnable()
    {
        base.OnEnable();
        CollectablesReference.OnCollected += CollectablesReference_OnCollected;
        UpdateSprite();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        CollectablesReference.OnCollected -= CollectablesReference_OnCollected;
    }

    private void CollectablesReference_OnCollected(ScriptIdentifier obj)
    {
        if (Collectable != obj) return;

        UpdateSprite();
    }

    private void UpdateSprite()
    {
        SpriteRenderer.sprite = CollectablesReference.IsCollected(Collectable) 
            ? CollectedSprite : MissingSprite;
    }
}

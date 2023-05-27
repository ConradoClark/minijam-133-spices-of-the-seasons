using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Objects;
using UnityEngine;

public class ChangeToOutlineWhenCollected : BaseGameObject
{
    [field:SerializeField]
    public SpriteRenderer[] SpritesToShow { get; private set; }

    [field: SerializeField]
    public SpriteRenderer SpriteToHide { get; private set; }

    private Collectable _collectable;

    protected override void OnEnable()
    {
        base.OnEnable();
        Actor.TryGetCustomObject(out _collectable);
        _collectable.OnCollect += OnCollect;
        
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _collectable.OnCollect -= OnCollect;
    }

    private void OnCollect(Collectable.OnCollectEventArgs obj)
    {
        SpriteToHide.enabled = false;
        foreach (var sprite in SpritesToShow)
        {
            sprite.enabled = true;
        }
    }
}

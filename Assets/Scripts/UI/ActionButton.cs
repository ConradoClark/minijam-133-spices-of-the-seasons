using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Objects;
using Licht.Unity.UI;
using UnityEngine;

public class ActionButton : BaseGameObject
{
    [field:SerializeField]
    public UIAction Action { get; private set; }
    [field: SerializeField]
    public SpriteRenderer SpriteRenderer { get; private set; }
    [field: SerializeField]
    public Sprite SelectedSprite { get; private set; }
    [field: SerializeField]
    public Sprite UnselectedSprite { get; private set; }


    protected override void OnEnable()
    {
        base.OnEnable();
        Action.OnSelected += Action_OnSelected;
        Action.OnDeselected += Action_OnDeselected;
        SpriteRenderer.sprite = Action.Selected ? SelectedSprite : UnselectedSprite;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Action.OnSelected -= Action_OnSelected;
        Action.OnDeselected -= Action_OnDeselected;
    }

    private void Action_OnDeselected()
    {
        SpriteRenderer.sprite = UnselectedSprite;
    }

    private void Action_OnSelected(bool obj)
    {
        SpriteRenderer.sprite = SelectedSprite;
    }
}

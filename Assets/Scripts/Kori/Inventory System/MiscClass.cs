using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Misc Class", menuName = "Item/Misc")]
public class MiscClass : ItemClass
{
    public override void Use(PlayerController caller)
    {
        base.Use(caller);
    }

    public override MiscClass GetMisc() { return this; }
}

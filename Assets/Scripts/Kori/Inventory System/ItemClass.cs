using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemClass : ScriptableObject
{
    [Header("Item Properties")]
    public string itemName;
    [TextAreaAttribute]
    public string description;
    public Sprite itemIcon;
    public bool isStackable = true;
    public GameObject objectPrefab;
    public GameObject objectPrefabPickable;

    public virtual void Use(PlayerController caller)
    {
        Debug.Log("Used " + itemName);
    }

    public virtual ItemClass GetItem() { return this; }
    public virtual MiscClass GetMisc() { return null; }
    public virtual ToolClass GetTool() { return null; }
    public virtual ConsumableClass GetConsumable() { return null; }
}

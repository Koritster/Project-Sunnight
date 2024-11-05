using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tool Class", menuName = "Item/Tool")]
public class ToolClass : ItemClass
{
    [Header("Tool Properties")]
    public ToolType toolType;

    public int damagePoints;

    public Sprite toolSprite;

    public enum ToolType
    {
        meleeWeapon,
        pickaxe,
        axe,
        fireWeapon
    }

    public override void Use(PlayerController caller)
    {
        base.Use(caller);
    }

    public override ToolClass GetTool() { return this; }
}

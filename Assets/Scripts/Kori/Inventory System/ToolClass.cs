using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tool Class", menuName = "Item/Tool")]
public class ToolClass : ItemClass
{
    [Header("Tool Properties")]
    public ToolType toolType;

    public int damagePoints;

    public enum ToolType
    {
        weapon,
        pickaxe,
        axe
    }

    public override void Use(PlayerController caller)
    {
        base.Use(caller);
    }

    public override ToolClass GetTool() { return this; }
}

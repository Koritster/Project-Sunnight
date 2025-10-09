using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Consumable Class", menuName = "Item/Consumable")]
public class ConsumableClass : ItemClass
{
    [Header("Consumable Stats")]
    [Space(5)]
    public int lifePoints;
    public int hungryPoints;
    public int thristPoints;

    public override void Use(PlayerController caller)
    {
        base.Use(caller);
        Scripter.scripter?.ChangeStats(lifePoints, hungryPoints, thristPoints);
        caller.inventory.UseSelected();
    }

    public override ConsumableClass GetConsumable() { return this; }
}

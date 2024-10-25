using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Consumable Class", menuName = "Item/Consumable")]
public class ConsumableClass : ItemClass
{
    [Header("Consumable Events")]
    [Space(5)]
    public UnityEvent customEvent;

    public override void Use(PlayerController caller)
    {
        base.Use(caller);
        customEvent?.Invoke();
        caller.inventory.UseSelected();
    }

    public override ConsumableClass GetConsumable() { return this; }
}

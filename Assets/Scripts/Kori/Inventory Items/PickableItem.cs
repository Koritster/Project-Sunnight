using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItem : MonoBehaviour, IPickable
{
    [SerializeField] private ItemClass item;

    public void PickItem()
    {
        Debug.Log("You picked " + item.name);
        Scripter.scripter.inventory.Add(item, 1);
        Destroy(gameObject);
    }
}

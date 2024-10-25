using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scripter : MonoBehaviour
{
    public static Scripter scripter;

    [Header("Chest Attributes")]
    public GameObject chestSlotsHolder;
    public Scrollbar scrollBar;
    public GameObject chestSlotPrefab;

    [Header("Inventory Attributes")]
    public InventoryManager inventory;

    void Awake()
    {
        if (scripter == null)
        {
            scripter = this;
        }
    }

    public void DestroyAllChestSlots()
    {
        foreach(Transform child in chestSlotsHolder.transform)
        {
            Destroy(child.gameObject);
        }
    }
}

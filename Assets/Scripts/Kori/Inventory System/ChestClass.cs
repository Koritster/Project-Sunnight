using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Chest Class", menuName = "Chest/Create Chest")]
public class ChestClass : ScriptableObject
{
    [System.Serializable]
    public class Objects
    {
        public ItemClass item;
        public Vector2 probabilities;
        public int quantity;

        public void ClearItem()
        {
            item = null;
            quantity = 0;
        }
    }

    [Header("Chest attributes")]
    [Space(5)]
    public Objects[] itemsToSpawn;
}

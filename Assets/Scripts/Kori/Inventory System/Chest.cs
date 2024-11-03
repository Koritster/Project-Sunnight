using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chest : MonoBehaviour
{
    public ChestClass chestType;
    [SerializeField]
    private ChestClass.Objects[] objects;

    void Start()
    {
        objects = new ChestClass.Objects[chestType.itemsToSpawn.Length];
        
        //Generar objetos
        for (int i = 0; i < chestType.itemsToSpawn.Length; i++)
        {
            int tempQuantity;

            tempQuantity = Random.Range((int)chestType.itemsToSpawn[i].probabilities.x, (int)chestType.itemsToSpawn[i].probabilities.y + 1);

            //No se genero nada
            if(tempQuantity <= 0)
            {
                continue;
            }

            objects[i] = new ChestClass.Objects();
            objects[i].item = chestType.itemsToSpawn[i].item;
            if (objects[i].item.isStackable)
            {
                objects[i].quantity = tempQuantity;
            }
            else
            {
                objects[i].quantity = 1;
            }
        }

        InstanceObjects();
    }

    public void InstanceObjects()
    {
        Scripter.scripter.DestroyAllChestSlots();

        foreach(ChestClass.Objects obj in objects)
        {
            if(obj != null)
            {
                if(obj.item == null)
                {
                    continue;
                }

                GameObject instance = Instantiate(Scripter.scripter.chestSlotPrefab, Scripter.scripter.chestSlotsHolder.transform);
                instance.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = obj.item.itemIcon;
                instance.transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = "x" + obj.quantity;
                instance.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => Scripter.scripter.inventory.Add(obj.item, obj.quantity));
                instance.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => obj.ClearItem());
                instance.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => Destroy(instance));
            }
        }
    }
}

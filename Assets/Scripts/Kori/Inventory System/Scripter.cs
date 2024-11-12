using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Scripter : MonoBehaviour
{
    public static Scripter scripter;

    [Header("Chest Attributes")]
    public GameObject chestSlotsHolder;
    public Scrollbar chestScrollBar;
    public GameObject chestSlotPrefab;

    [Header("Crafting Attributes")]
    public GameObject craftingSlotsHolder;
    public Scrollbar craftingScrollBar;
    public GameObject craftingSlotPrefab;

    [Header("Inventory Attributes")]
    public InventoryManager inventory;

    [Header("Player Attributes")]
    public GameObject player;
    public hambre_vida_Agua statsSystem;
    public bool inAction;

    void Awake()
    {
        if (scripter == null)
        {
            scripter = this;
        }

        player = GameObject.FindWithTag("Player");
        statsSystem = player.GetComponent<hambre_vida_Agua>();
    }

    #region Chest Functions

    public void DestroyAllChestSlots()
    {
        foreach(Transform child in chestSlotsHolder.transform)
        {
            Destroy(child.gameObject);
        }
    }

    #endregion

    #region Player Functions

    public void ChangeStats(int lifePoints, int hungryPoints, int thristPoints)
    {
        statsSystem.Curar(lifePoints);
        statsSystem.ChangeHungry(hungryPoints);
        statsSystem.ChangeThirst(thristPoints);
    }

    public void Attack(IAttackable obj, ToolClass tool, int damagePoints)
    {
        if (!inAction)
        {
            player.GetComponent<Animator>().SetTrigger("UseTool");
            StartCoroutine(AttackCoroutine(obj, tool, damagePoints));
        }
    }

    IEnumerator AttackCoroutine(IAttackable obj, ToolClass tool, int damagePoints)
    {
        inAction = true;
        yield return new WaitForSeconds(0.5f);
        obj.Hurt(tool, damagePoints);
        yield return new WaitForSeconds(0.5f);
        inAction = false;
    }

    #endregion

    #region Global Functions

    public void Pause()
    {
        //Freeze enemies
        //Freeze stats system
    }

    #endregion
}

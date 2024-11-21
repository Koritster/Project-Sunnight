using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class Scripter : MonoBehaviour
{
    public static Scripter scripter;

    [Header("Chest Attributes")]
    public GameObject chestSlotsHolder;
    public Scrollbar chestScrollBar;
    public GameObject chestSlotPrefab;

    [System.Serializable]
    public class CraftingSlots
    {
        public string craftingType;
        public GameObject craftingSlotsHolder;
    }

    [Header("Crafting Attributes")]
    public CraftingSlots[] craftingSlots;
    public GameObject craftingSlotPrefab;
    public Scrollbar craftingScrollBar;
    public CraftingRecipeClass[] craftingRecipes;

    [Header("Inventory Attributes")]
    public InventoryManager inventory;

    [Header("Player Attributes")]
    public GameObject player;
    public hambre_vida_Agua statsSystem;
    private bool inAction;

    void Awake()
    {
        if (scripter == null)
        {
            scripter = this;
        }

        player = GameObject.FindWithTag("Player");
        statsSystem = player.GetComponent<hambre_vida_Agua>();

        GetCrafteos();
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
            if(tool.toolType == ToolClass.ToolType.fireWeapon)
            {
                Debug.Log("Disparaste");
                //Animaciones para armas de fuego
            }
            else
            {
                Debug.Log("Golpeaste");
                //Animaciones armas melee
                player.GetComponent<Animator>().SetTrigger("UseTool");
            }

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

    #region Crafting Functions

    private void GetCrafteos()
    {
        Debug.Log("Buscando...");
        string[] guids = AssetDatabase.FindAssets("t:MyCustomScriptableObject", new[] { "Assets" });

        craftingRecipes = new CraftingRecipeClass[guids.Length];

        for (int i = 0; i < guids.Length; i++)
        {
            // Obtener la ruta completa del asset
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);

            // Cargar el ScriptableObject específico
            craftingRecipes[i] = AssetDatabase.LoadAssetAtPath<CraftingRecipeClass>(assetPath);

            // Mostrar el nombre del objeto cargado
            Debug.Log("Cargado: " + craftingRecipes[i].name);
        }
    }

    #endregion
}

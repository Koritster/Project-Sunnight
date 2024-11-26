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

    [Header("Feedback System Attributes")]
    public FeedbackSystem feedbackSystem;
    public GameObject constructionPreview;
    public GameObject constructionFeedbackUI;

    [Header("Quests System Attributes")]
    public QuestsSystem questsSystem;

    void Awake()
    {
        if (scripter == null)
        {
            scripter = this;
        }

        player = GameObject.FindWithTag("Player");
        statsSystem = player.GetComponent<hambre_vida_Agua>();
        feedbackSystem = player.GetComponent<FeedbackSystem>();

        craftingRecipes = LoadCraftingRecipes();
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

    string folderPath = "Assets/Scripts/Kori/Inventory Items/Crafting Recipes";

    public CraftingRecipeClass[] LoadCraftingRecipes()
    {
        // Obtiene las rutas de todos los archivos .asset en la carpeta especificada
        string[] assetPaths = AssetDatabase.FindAssets("t:CraftingRecipeClass", new[] { folderPath });

        CraftingRecipeClass[] recipes = new CraftingRecipeClass[assetPaths.Length];

        // Convierte las rutas en objetos del tipo CraftingRecipeClass
        for (int i = 0; i < assetPaths.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(assetPaths[i]);
            recipes[i] = AssetDatabase.LoadAssetAtPath<CraftingRecipeClass>(path);
        }

        return recipes;
    }

    #endregion
}

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
    public PlayerController playerController;

    [SerializeField] private AnimationClip useToolClip;
    [SerializeField] private AnimationClip useFireWeaponClip;

    private bool inAction;

    [Header("Feedback System Attributes")]
    public FeedbackSystem feedbackSystem;
    public GameObject constructionPreview;
    public GameObject constructionFeedbackUI;
    public GameObject chestFeedbackUI;
    public GameObject grabItemsFeedbackUI;
    public GameObject useRadioFeedbackUI;
    public GameObject useCampfireFeedbackUI;

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
        playerController = player.GetComponent<PlayerController>();
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
            float animDuration;

            if(tool.toolType == ToolClass.ToolType.fireWeapon)
            {
                //Animaciones para armas de fuego
                Debug.Log("Disparaste");
                animDuration = useFireWeaponClip.length;
                player.GetComponent<Animator>().SetTrigger("UseFireWeapon");
            }
            else
            {
                //Animaciones armas melee
                Debug.Log("Golpeaste");
                animDuration = useToolClip.length;
                player.GetComponent<Animator>().SetTrigger("UseTool");
            }

            StartCoroutine(AttackCoroutine(obj, tool, damagePoints, animDuration));
        }
    }

    public void Attack(ToolClass tool)
    {
        if (!inAction)
        {
            float animDuration;

            if (tool.toolType == ToolClass.ToolType.fireWeapon)
            {
                //Animaciones para armas de fuego
                Debug.Log("Disparaste");
                animDuration = useFireWeaponClip.length;
                player.GetComponent<Animator>().SetTrigger("UseFireWeapon");
            }
            else
            {
                //Animaciones armas melee
                Debug.Log("Golpeaste");
                animDuration = useToolClip.length;
                player.GetComponent<Animator>().SetTrigger("UseTool");
            }

            StartCoroutine(AttackCoroutine(animDuration));
        }
    }

    IEnumerator AttackCoroutine(IAttackable obj, ToolClass tool, int damagePoints, float animDuration)
    {
        inAction = true;
        yield return new WaitForSeconds(animDuration/2);
        obj.Hurt(tool, damagePoints);
        yield return new WaitForSeconds(animDuration / 2);
        inAction = false;
    }

    IEnumerator AttackCoroutine(float animDuration)
    {
        inAction = true;
        yield return new WaitForSeconds(animDuration);
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

    public CraftingRecipeClass[] LoadCraftingRecipes()
    {
        CraftingRecipeClass[] recipes = Resources.LoadAll<CraftingRecipeClass>("");

        return recipes;
    }

    public void ChangeBarValue()
    {
        StartCoroutine(ChangeBarValue(0.0001f));
    }

    IEnumerator ChangeBarValue(float time)
    {
        yield return new WaitForSeconds(time);
        craftingScrollBar.value = 1;
    }

    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Crafting Recipe", menuName = "Item/Crafting/New Recipe")]
public class CraftingRecipeClass : ScriptableObject 
{
    #region Crafting Functions

    [Header("Recipe attributes")]
    public SlotClass[] ingredients;
    public SlotClass outputItem;

    public bool CanCraft(InventoryManager inv)
    {
        foreach(SlotClass c in ingredients) 
        {
            if (!inv.Contains(c.GetItem(), c.GetQuantity()))
            {
                return false;
            }
        }

        return true;
    }

    public void Craft(InventoryManager inv)
    {
        foreach (SlotClass c in ingredients)
        {
            inv.Remove(c.GetItem(), c.GetQuantity());
        }

        inv.Add(outputItem.GetItem(), outputItem.GetQuantity());
    }

    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tool Class", menuName = "Item/Tool")]
public class ToolClass : ItemClass
{
    [Header("Tool Properties")]
    public ToolType toolType;

    public int damagePoints;

    public Sprite toolSprite;

    public enum ToolType
    {
        meleeWeapon,
        pickaxe,
        axe,
        fireWeapon
    }

    public override void Use(PlayerController caller)
    {
        base.Use(caller);

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 7))
        {
            if(hit.collider.TryGetComponent<IAttackable>(out IAttackable obj))
            {
                Scripter.scripter.Attack(obj, this, damagePoints);
            }
        }
    }

    public override ToolClass GetTool() { return this; }
}

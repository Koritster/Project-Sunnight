using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tool Class", menuName = "Item/Tool")]
public class ToolClass : ItemClass
{
    [Header("Tool Properties")]
    public ToolType toolType;

    public int damagePoints;
    public float rayLenght = 4;
    public ParticleSystem particles;

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
        float tempRayLenght;
        base.Use(caller);

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        RaycastHit hit;
        
        //Al usar un arma de fuego
        if(toolType == ToolType.fireWeapon)
        {
            tempRayLenght = rayLenght * 3;
            particles.Play();
        }
        //Al usar otro tipo de arma
        else
        {
            tempRayLenght = rayLenght;
        }
        
        if (Physics.Raycast(ray, out hit, tempRayLenght))
        {
            if(hit.collider.TryGetComponent<IAttackable>(out IAttackable obj))
            {
                Scripter.scripter.Attack(obj, this, damagePoints);
            }
        }
    }

    public override ToolClass GetTool() { return this; }
}

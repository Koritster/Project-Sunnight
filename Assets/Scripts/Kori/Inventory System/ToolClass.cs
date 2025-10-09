using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "New Tool Class", menuName = "Item/Tool")]
public class ToolClass : ItemClass
{
    [Header("Tool Properties")]
    public ToolType toolType;

    public int damagePoints;
    public float rayLenght = 4;

    public Sprite toolSprite;
    public ItemClass ammo;

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
            try
            {
                if (Scripter.scripter.inventory.Contains(ammo, 1))
                {
                    Scripter.scripter.inventory.Remove(ammo);
                }
                else
                    return;
                
                objectPrefab.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
            }
            catch
            {
                Debug.Log("Falta ponerle particulas a esta madre w");
            }
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
        else
        {
            Debug.Log("No hay nada enfrente");
            Scripter.scripter.Attack(this);
        }
    }

    public override ToolClass GetTool() { return this; }

    #if UNITY_EDITOR
    // Asegúrate de que el CustomEditor hace referencia a ToolClass
    [CustomEditor(typeof(ToolClass))]
    public class ToolClassEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // Obtén una referencia al objeto que estamos editando
            ToolClass toolClass = (ToolClass)target;

            DrawDefaultInspector();

            // Mostrar el enum ToolType
            toolClass.toolType = (ToolClass.ToolType)EditorGUILayout.EnumPopup("Tool Type", toolClass.toolType);

            // Si toolType es 'fireWeapon', mostrar el campo ammo
            if (toolClass.toolType == ToolClass.ToolType.fireWeapon)
            {
                toolClass.ammo = (ItemClass)EditorGUILayout.ObjectField("Ammo", toolClass.ammo, typeof(ItemClass), true);
            }

            // Guarda los cambios
            if (GUI.changed)
            {
                EditorUtility.SetDirty(toolClass);
            }
        }
    }
    #endif
}

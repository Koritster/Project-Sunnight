using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New AttackableObject", menuName = "AttackableObject/New")]
public class AttackableObject : ScriptableObject
{
    public string objectName;
    public ToolToDamage toolType;
    public AudioClip soundEffect;
    public int lifePoints;

    public enum ToolToDamage
    {
        none,
        any,
        meleeWeapon,
        fireWeapon,
        axe,
        pickaxe
    }
}

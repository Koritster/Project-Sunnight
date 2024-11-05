using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New AttackableObject", menuName = "AttackableObject/New")]
public class AttackableObject : ScriptableObject
{
    public string objectName;

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

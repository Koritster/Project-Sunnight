using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Construction Class", menuName = "Item/Construction")]
public class ConstructionClass : ItemClass
{
    [Header("Construction Properties")]
    public GameObject constructionPrefab;

    public override void Use(PlayerController caller)
    {
        Debug.Log("Construiste algo");
    }

    public override ConstructionClass GetConstruction() { return this; }
}

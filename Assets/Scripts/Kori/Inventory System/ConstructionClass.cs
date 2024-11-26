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
        base.Use(caller);
        Debug.Log("Construiste algo");

        Scripter.scripter.feedbackSystem.constructionPreview.GetComponent<BoxCollider>().isTrigger = false;
        Scripter.scripter.feedbackSystem.constructionPreview.layer = LayerMask.NameToLayer("Default");
        Scripter.scripter.feedbackSystem.constructionPreview.transform.GetChild(0).gameObject.SetActive(true);
        Scripter.scripter.feedbackSystem.constructionPreview.transform.GetChild(1).gameObject.SetActive(false);

        caller.inventory.UseSelected();
    }

    public override ConstructionClass GetConstruction() { return this; }
}

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class FeedbackSystem : MonoBehaviour
{
    Scripter scripter;
    InventoryManager inv;

    public GameObject constructionPreview;
    BoxCollider constructionCollider;
    ConstructionClass construction;

    int layerMask;

    float rayLenght;

    private void Start()
    {
        layerMask = ~LayerMask.GetMask("Player", "Construction");
        scripter = Scripter.scripter;
        inv = scripter.inventory;
        rayLenght = scripter.playerController.raycastLength;
    }

    void Update()
    {
        ConstructionSystem();

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, rayLenght, layerMask))
        {
            if (hit.collider.GetComponent<Chest>())
            {
                scripter.chestFeedbackUI.SetActive(true);
            }
            else if (hit.collider.GetComponent<PickableItem>())
            {
                scripter.grabItemsFeedbackUI.SetActive(true);
            }
            else if (hit.collider.GetComponent<Radio>())
            {
                scripter.useRadioFeedbackUI.SetActive(true);
            }
            /*else if(hit.collider.GetComponent<Campfire>())
            {
                scripter.useCampfireFeedbackUI.SetActive(true);
            }
             */
            else
            {
                scripter.chestFeedbackUI.SetActive(false);
                scripter.grabItemsFeedbackUI.SetActive(false);
                scripter.useRadioFeedbackUI.SetActive(false);
                scripter.useCampfireFeedbackUI.SetActive(false);
            }

        }
    }

    private void ConstructionSystem()
    {
        //Sistema de construcción
        if (inv.selectedItem != null)
        {
            if (inv.selectedItem.GetConstruction())
            {
                if (construction != inv.selectedItem.GetConstruction())
                {
                    construction = inv.selectedItem.GetConstruction();

                    Destroy(constructionPreview);
                    constructionPreview = null;
                    constructionCollider = null;
                    scripter.constructionPreview = null;

                    GameObject prefab = inv.selectedItem.GetConstruction().constructionPrefab;
                    constructionPreview = Instantiate(prefab);
                    scripter.constructionPreview = constructionPreview;
                    constructionCollider = constructionPreview.GetComponent<BoxCollider>();
                }
                else if (constructionPreview == null)
                {
                    GameObject prefab = inv.selectedItem.GetConstruction().constructionPrefab;
                    constructionPreview = Instantiate(prefab);
                    scripter.constructionPreview = constructionPreview;
                    constructionCollider = constructionPreview.GetComponent<BoxCollider>();
                }

                Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 10f, layerMask))
                {
                    if (!constructionPreview.activeSelf)
                        constructionPreview.SetActive(true);

                    constructionPreview.transform.position = new Vector3(hit.point.x, hit.point.y + (constructionCollider.bounds.size.y / 2), hit.point.z);

                    scripter.constructionFeedbackUI.SetActive(true);
                }
                else
                {
                    constructionPreview.SetActive(false);

                    scripter.constructionFeedbackUI.SetActive(false);
                }
            }
        }
        //Destruir el preview
        else if (constructionPreview != null && constructionCollider.isTrigger)
        {
            Destroy(constructionPreview);
            constructionPreview = null;
            constructionCollider = null;
            scripter.constructionPreview = null;

            scripter.constructionFeedbackUI.SetActive(false);
        }

        if (constructionPreview != null)
        {
            if (!constructionCollider.isTrigger)
            {
                constructionPreview = null;
                constructionCollider = null;
                scripter.constructionPreview = null;

                scripter.constructionFeedbackUI.SetActive(false);
            }
        }
    }
}

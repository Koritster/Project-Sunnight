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
            else if(hit.collider.TryGetComponent<Campfire>(out Campfire c))
            {
                scripter.useCampfireFeedbackUI.SetActive(true);
                if (!c.withLogs)
                {
                    scripter.useCampfireFeedbackUI.transform.GetChild(0).gameObject.SetActive(true);
                }
                else if (!c.onFire)
                {
                    scripter.useCampfireFeedbackUI.transform.GetChild(1).gameObject.SetActive(true);
                }
                else if (Scripter.scripter.inventory.selectedItem.GetItem().name == "Raw meat")
                {
                    scripter.useCampfireFeedbackUI.transform.GetChild(2).gameObject.SetActive(true);
                }
                else
                {
                    scripter.useCampfireFeedbackUI.transform.GetChild(0).gameObject.SetActive(false);
                    scripter.useCampfireFeedbackUI.transform.GetChild(1).gameObject.SetActive(false);
                    scripter.useCampfireFeedbackUI.transform.GetChild(2).gameObject.SetActive(false);
                }
            }
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

                    Debug.Log("Se reemplazo la construccion");
                }
                else if (constructionPreview == null)
                {
                    GameObject prefab = inv.selectedItem.GetConstruction().constructionPrefab;
                    constructionPreview = Instantiate(prefab);
                    scripter.constructionPreview = constructionPreview;
                    constructionCollider = constructionPreview.GetComponent<BoxCollider>();
                    Debug.Log("Se instancio una construccion");
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
            if (construction.GetConstruction().placed)
                return;

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerController : MonoBehaviour
{
    public InventoryManager inventory;

    [SerializeField] private GameObject cs_Inventory;
    [SerializeField] private GameObject cs_InformationPanel;
    [SerializeField] private GameObject cs_Crafting;
    [SerializeField] private GameObject cs_Chest;

    private bool isInventoryOpen;
    private FirstPersonController fpsController;

    [SerializeField] private float raycastLength;

    void Awake()
    {
        fpsController = GetComponent<FirstPersonController>();
    }

    void Update()
    {
        //Abrir inventario
        if (Input.GetKeyDown(KeyCode.I))
        {
            OpenMenu();
            cs_Inventory.SetActive(true);
            //cs_InformationPanel.SetActive(true)
        }
        //Abrir crafteo
        else if (Input.GetKeyDown(KeyCode.C))
        {
            OpenMenu();
            cs_Inventory.SetActive(true);
            //cs_Crafting.SetActive(true);
        }
        //Abrir cofre
        else if (Input.GetKeyDown(KeyCode.F))
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, raycastLength))
            {
                Chest chest = hit.collider.gameObject.GetComponent<Chest>();
                if(chest != null)
                {
                    Debug.Log("Se abrió un cofre");
                    chest.InstanceObjects();
                    OpenMenu(); 
                    cs_Inventory.SetActive(true);
                    cs_Chest.SetActive(true);
                }
            }
            else
            {
                if (isInventoryOpen)
                {
                    CloseAllCanvas();
                }
            }
            //
        }
        //Usar objetos
        else if (Input.GetMouseButtonDown(0) && !isInventoryOpen)
        {
            if (inventory.selectedItem != null)
            {
                inventory.selectedItem.Use(this);
            }
        }
        //Cerrar todo
        if (!isInventoryOpen)
        {
            CloseAllCanvas();
        }
    }

    private void OpenMenu()
    {
        isInventoryOpen = inventory.OpenAndCloseInventory();
        fpsController.OpenAndClose();
    }

    private void CloseAllCanvas()
    {
        cs_Inventory.SetActive(false);
        //cs_InformationPanel.SetActive(false);
        //cs_Crafting.SetActive(false);
        //cs_Chest.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        // Dibuja la línea del raycast siempre
        Gizmos.color = Color.red; // Color de la línea
        Vector3 start = Camera.main.transform.position; // Posición de la cámara
        Vector3 end = start + Camera.main.transform.forward * raycastLength; // Punto final basado en la dirección de la cámara y la longitud
        Gizmos.DrawLine(start, end);
    }
}

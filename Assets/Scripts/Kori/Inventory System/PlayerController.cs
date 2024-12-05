using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerController : MonoBehaviour
{
    public InventoryManager inventory;
    public float raycastLength;

    [SerializeField] private GameObject cs_InGame;
    [SerializeField] private GameObject cs_Inventory;
    [SerializeField] private GameObject cs_InformationPanel;
    [SerializeField] private GameObject cs_Crafting;
    [SerializeField] private GameObject cs_Chest;
    [SerializeField] private GameObject cs_Pause;

    private bool isInventoryOpen;
    private bool isPause;

    [SerializeField]
    private FirstPersonController fpsController;

    [SerializeField] private float speedRotation;

    void Awake()
    {
        fpsController = GetComponent<FirstPersonController>();
    }

    void Update()
    {
        //Pausar
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isInventoryOpen) 
            {
                Debug.Log("Cerrar inv");
                Pause();
            }
            else if (!isPause)
            {
                Debug.Log("Pausar");
                Pause();
                cs_Pause.SetActive(true);
                cs_InGame.SetActive(false);
                isPause = true;
                Scripter.scripter.Pause();
            }
            else
            {
                Debug.Log("Despausar");
                Continue();
            }

        }

        if (isPause)
        {
            return;
        }

        //Abrir inventario
        if (Input.GetKeyDown(KeyCode.I))
        {
            OpenMenu();
            cs_Inventory.SetActive(true);
            cs_InformationPanel.SetActive(true);
            cs_InGame.SetActive(false);

            //Mision de abrir inventario
            if (Scripter.scripter.questsSystem.questIndex == 1)
            {
                Scripter.scripter.questsSystem.CompleteQuestAnim();
            }
        }
        //Abrir crafteo
        else if (Input.GetKeyDown(KeyCode.C))
        {
            OpenMenu();
            cs_Crafting.SetActive(true);
            cs_InGame.SetActive(false);

            Scripter.scripter.ChangeBarValue();

            //Mision de abrir crafteo
            if (Scripter.scripter.questsSystem.questIndex == 2)
            {
                Scripter.scripter.questsSystem.CompleteQuestAnim();
            }
        }
        //Abrir cofre o agarrar objetos
        else if (Input.GetKeyDown(KeyCode.F))
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, raycastLength))
            {
                //Abrir cofre
                Chest chest = hit.collider.gameObject.GetComponent<Chest>();
                if (chest != null)
                {
                    Debug.Log("Se abrió un cofre");
                    chest.InstanceObjects();
                    OpenMenu();
                    cs_Inventory.SetActive(true);
                    cs_Chest.SetActive(true);
                    cs_InGame.SetActive(false);

                    //Mision de abrir cofre
                    if (Scripter.scripter.questsSystem.questIndex == 0)
                    {
                        Scripter.scripter.questsSystem.CompleteQuestAnim();
                    }
                }
                else if(hit.collider.gameObject.TryGetComponent<IPickable>(out IPickable obj))
                {
                    obj.PickItem();
                }
                else if (hit.collider.gameObject.TryGetComponent<Radio>(out Radio r))
                {
                    if (!r.CheckWin())
                        return;

                    if(Scripter.scripter.questsSystem.questIndex == (Scripter.scripter.questsSystem.quests.Length - 1))
                    {
                        Scripter.scripter.questsSystem.CompleteQuestAnim();
                    }

                    Scripter.scripter.gameObject.GetComponent<SceneManager>().ChangeSceneByName("WinAnimation");
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
        //Rotar construcciones
        else if(Scripter.scripter.inventory.selectedItem != null)
        {
            if(Scripter.scripter.inventory.selectedItem.GetConstruction() != null)
            {
                Debug.Log("Rotando...");
                float y = Input.GetAxis("Rotate");
                Scripter.scripter.constructionPreview.transform.Rotate(0, speedRotation * y * Time.deltaTime, 0);
            }
        }

        //Cerrar todo
        if (!isInventoryOpen)
        {
            CloseAllCanvas();
        }
    }

    public void Continue()
    {
        OpenMenu();
        isPause = false;
    }

    private void OpenMenu()
    {
        isInventoryOpen = inventory.OpenAndCloseInventory();
        fpsController.OpenAndClose();
    }

    private void Pause()
    {
        isPause = inventory.OpenAndCloseInventory();
        fpsController.OpenAndClose();
    }

    private void CloseAllCanvas()
    {
        cs_InGame.SetActive(true);
        cs_Inventory.SetActive(false);
        cs_InformationPanel.SetActive(false);
        cs_Pause.SetActive(false);
        cs_Crafting.SetActive(false);
        cs_Chest.SetActive(false);
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

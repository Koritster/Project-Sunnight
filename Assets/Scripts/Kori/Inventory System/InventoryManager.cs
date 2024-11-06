using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    #region Inventory Variables

    [Header("Inventory section")]
    [Space(5)]
    [Tooltip("Imagen que se mostrará al agarrar un objeto")]
    [SerializeField] private GameObject itemCursor;
    
    [Tooltip("Objeto que contiene todos los slots")]
    [SerializeField] private GameObject slotHolder;
    [Tooltip("Objeto que contiene los slots de la hotbar")]
    [SerializeField] private GameObject hotbarSlotHolder;
    [SerializeField] private ItemClass itemToAdd;
    [SerializeField] private ItemClass itemToRemove;

    [Tooltip("Objetos con los que iniciará el personaje")]
    [SerializeField] private SlotClass[] startingItems;

    private SlotClass[] items;

    private GameObject[] slots;
    private GameObject[] hotbarSlots;

    private SlotClass movingSlot;
    private SlotClass tempSlot;
    private SlotClass originalSlot;

    bool isMovingItem;

    #endregion

    #region Hotbar Variables
    
    [Space(5)]
    [Header("Hotbar section")]
    [Space(5)]
    [Tooltip("Imagen que indica el objeto seleccionado en la hotbar")]
    [SerializeField] private GameObject hotbarSelector;
    [Tooltip("Index del objeto seleccionado en la hotbar")]
    [SerializeField] private int selectedSlotIndex = 0;
    [Tooltip("Objeto seleccionado en la hotbar")]
    public ItemClass selectedItem;

    [Tooltip("Objeto donde se spawnearan los objetos seleccionados")]
    [SerializeField] private GameObject handAttachment;
    [SerializeField] private GameObject[] itemPrefabs;

    #endregion

    #region Information Panel Variables

    [Space(5)]
    [Header("Information Panel Variables")]
    [Space(5)]
    [SerializeField] private Image itemIconHolder;
    [SerializeField] private Text txt_ItemName;
    [SerializeField] private Text txt_Description;

    //0 - 2 Life, Hungry and Thrist, 3 Tools
    [SerializeField] private Image[] img_Stats;
    [SerializeField] private Text[] txt_Stats;

    private ItemClass hoverItem;

    #endregion

    private void Start()
    {
        slots = new GameObject[slotHolder.transform.childCount];
        items = new SlotClass[slots.Length];

        hotbarSlots = new GameObject[hotbarSlotHolder.transform.childCount];

        itemPrefabs = new GameObject[hotbarSlots.Length];

        for (int i = 0; i < hotbarSlots.Length; i++)
        {
            hotbarSlots[i] = hotbarSlotHolder.transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < items.Length; i++)
        {
            items[i] = new SlotClass();
        }

        for (int i = 0; i < startingItems.Length; i++)
        {
            items[i] = startingItems[i];
        }

        for (int i = 0; i < slotHolder.transform.childCount; i++)
        {
            slots[i] = slotHolder.transform.GetChild(i).gameObject;
        }

        Add(itemToAdd, 1);
        Remove(itemToRemove);

        RefreshUI();
    }

    private void Update()
    {
        //Codigo para cuando el inv este abierto
        //Activar la imagen que se muestra al agarrar un objeto
        if (isMovingItem && movingSlot.GetItem() != null)
        {
            itemCursor.SetActive(true);
            itemCursor.transform.position = Input.mousePosition;
            itemCursor.GetComponent<Image>().sprite = movingSlot.GetItem().itemIcon;
        }

        //Arrastrar y soltar objetos dentro del inventario
        if (Input.GetMouseButtonDown(0))
        {
            BeginItemMove();
            EndItemMove();
        }

        //Agarrar la mitad de objetos / Soltar un solo objeto
        else if (Input.GetMouseButtonDown(1))
        {
            if (isMovingItem)
            {
                EndItemMove_Single();
                itemCursor.SetActive(false);
            }
            else
            {
                BeginItemMove_Half();
            }
        }

        //General
        //Movimiento dentro de la hotbar
        if(Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if(selectedSlotIndex == hotbarSlots.Length - 1)
            {
                selectedSlotIndex = 0;
            }
            else
            {
                selectedSlotIndex = Mathf.Clamp(selectedSlotIndex + 1, 0, hotbarSlots.Length - 1);
            }

            DesactivateHotbarPrefabs();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (selectedSlotIndex == 0)
            {
                selectedSlotIndex = hotbarSlots.Length - 1;
            }
            else
            {
                selectedSlotIndex = Mathf.Clamp(selectedSlotIndex - 1, 0, hotbarSlots.Length - 1);
            }

            DesactivateHotbarPrefabs();
        }

        //Ajustar el selector del hotbar
        hotbarSelector.transform.position = hotbarSlots[selectedSlotIndex].transform.position;
        selectedItem = items[selectedSlotIndex + (slots.Length - hotbarSlots.Length)].GetItem();

        if(selectedItem != null && itemPrefabs[selectedSlotIndex] != null && !itemPrefabs[selectedSlotIndex].activeSelf)
        {
            itemPrefabs[selectedSlotIndex].SetActive(true);
        }


        //Seccion - Information Panel

        SlotClass hoverSlot = GetClossestSlot();
        if(hoverSlot != null)
        {
            //Si son lo mismo, no hacer nada
            if (hoverSlot.GetItem() == hoverItem)
                return;

            hoverItem = hoverSlot.GetItem();

            //Si está vacio hoverItem, no hacer nada
            if(hoverItem == null)
                return;

            itemIconHolder.gameObject.SetActive(true);
            txt_ItemName.gameObject.SetActive(true);
            txt_Description.gameObject.SetActive(true);

            itemIconHolder.sprite = hoverItem.itemIcon;
            txt_ItemName.text = hoverItem.itemName;
            txt_Description .text = hoverItem.description;

            //Si es un item consumible
            if (hoverItem.GetConsumable() != null)
            {
                ConsumableClass tempCons = hoverItem.GetConsumable();

                for (int i = 0; i <= 2; i++)
                {
                    img_Stats[i].gameObject.SetActive(true);
                    txt_Stats[i].gameObject.SetActive(true);
                }

                txt_Stats[0].text = tempCons.lifePoints + "";
                txt_Stats[1].text = tempCons.hungryPoints + "";
                txt_Stats[2].text = tempCons.thristPoints + "";
            }

            //Si es una herramienta
            else if (hoverItem.GetTool())
            {
                ToolClass tempTool = hoverItem.GetTool();

                img_Stats[3].gameObject.SetActive(true);
                txt_Stats[3].gameObject.SetActive(true);

                img_Stats[3].sprite = tempTool.toolSprite;
                txt_Stats[3].text = tempTool.damagePoints + "";
            }
        }
        //Si no se esta agarrando nada, convertir hoverItem a null
        else if(hoverItem != null)
        {
            hoverItem = null;

            itemIconHolder.gameObject.SetActive(false);
            txt_ItemName.gameObject.SetActive(false);
            txt_Description.gameObject.SetActive(false);

            for (int i = 0; i < txt_Stats.Length; i++)
            {
                img_Stats[i].gameObject.SetActive(false);
                txt_Stats[i].gameObject.SetActive(false);
            }
        }
    }

    #region Inventory Utils

    //Actualiza la UI
    public void RefreshUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            try
            {
                slots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = items[i].GetItem().itemIcon;
                slots[i].transform.GetChild(1).gameObject.SetActive(items[i].GetItem().isStackable);
                slots[i].transform.GetChild(1).GetComponent<Text>().text = items[i].GetQuantity() + "";
            }
            catch
            {
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                slots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                slots[i].transform.GetChild(1).GetComponent<Text>().text = "";
            }
        }

        RefreshHotbarUI();
    }

    public void RefreshHotbarUI()
    {
        for (int i = (slots.Length - hotbarSlots.Length); i < slots.Length; i++)
        {
            try
            {
                hotbarSlots[i - (slots.Length - hotbarSlots.Length)].transform.GetChild(0).GetComponent<Image>().enabled = true;
                hotbarSlots[i - (slots.Length - hotbarSlots.Length)].transform.GetChild(0).GetComponent<Image>().sprite = items[i].GetItem().itemIcon;
                hotbarSlots[i - (slots.Length - hotbarSlots.Length)].transform.GetChild(1).gameObject.SetActive(items[i].GetItem().isStackable);
                hotbarSlots[i - (slots.Length - hotbarSlots.Length)].transform.GetChild(1).GetComponent<Text>().text = items[i].GetQuantity() + "";

                //Instanciar prefabs en caso de no tener
                //verificar que no esté vació el espacio en la hotbar
                Debug.Log("Verificando hotbar...");
                ItemClass tempObj = items[((slots.Length - hotbarSlots.Length) + (i - (slots.Length - hotbarSlots.Length)))].GetItem();
                if (tempObj != null)
                {
                    if(itemPrefabs[i - (slots.Length - hotbarSlots.Length)] == null)
                    {
                        Transform trsm_HandAttachment = handAttachment.transform;
                        Transform trsm_tempObj = tempObj.objectPrefab.transform;
                        Debug.Log("Agregar objeto a la lista");
                        Debug.Log(new Vector3(trsm_tempObj.localPosition.x, trsm_tempObj.localPosition.y, trsm_tempObj.localPosition.z));
                        itemPrefabs[i - (slots.Length - hotbarSlots.Length)] = Instantiate(tempObj.objectPrefab, new Vector3(0f, 0f, 0f), Quaternion.Euler(trsm_tempObj.eulerAngles), handAttachment.transform);
                        itemPrefabs[i - (slots.Length - hotbarSlots.Length)].transform.localPosition = new Vector3(trsm_tempObj.localPosition.x, trsm_tempObj.localPosition.y, trsm_tempObj.localPosition.z);
                        itemPrefabs[i - (slots.Length - hotbarSlots.Length)].transform.localRotation = Quaternion.Euler(trsm_tempObj.eulerAngles);
                    }
                }
            }
            catch
            {
                hotbarSlots[i - (slots.Length - hotbarSlots.Length)].transform.GetChild(0).GetComponent<Image>().sprite = null;
                hotbarSlots[i - (slots.Length - hotbarSlots.Length)].transform.GetChild(0).GetComponent<Image>().enabled = false;
                hotbarSlots[i - (slots.Length - hotbarSlots.Length)].transform.GetChild(1).GetComponent<Text>().text = "";

                //Destruir prefabs en caso de tenerlos instanciados
                if (items[((slots.Length - hotbarSlots.Length) + (i - (slots.Length - hotbarSlots.Length)))].GetItem() == null && itemPrefabs[i - (slots.Length - hotbarSlots.Length)] != null)
                {
                    Debug.Log("Quitar objeto de la lista");
                    Destroy(itemPrefabs[i - (slots.Length - hotbarSlots.Length)]);
                    itemPrefabs[i - (slots.Length - hotbarSlots.Length)] = null;
                }
            }
        }

        DesactivateHotbarPrefabs();
    }

    private void DesactivateHotbarPrefabs()
    {
        for(int i = 0; i < hotbarSlots.Length; i++)
        {
            if (itemPrefabs[i] != null)
            {
                itemPrefabs[i].SetActive(false);
            }
        }
    }

    //Añadir el objeto, retorna falso si no hay espacio en el inventario, retorna verdadero si sí hay espacio
    //En caso de ya contar con un objeto del mismo tipo y sea stackeable, se suma al stack
    //En caso contrario, se añade nuevo objeto a la lista
    public bool Add(ItemClass item, int quantity)
    {
        if (item == null)
            return false;

        SlotClass slot = Contains(item);
        if (slot != null && slot.GetItem().isStackable)
            slot.AddQuantity(quantity);
        else
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].GetItem() == null)
                {
                    items[i].AddItem(item, quantity);
                    break;
                }
            }
        }

        RefreshUI();
        return true;
    }

    //Remover objeto, retorna falso si el objeto no está en el inventario, retorna verdadero si sí
    public bool Remove(ItemClass item)
    {
        if (item == null)
            return false;

        SlotClass temp = Contains(item);
        if (temp != null)
        {
            if (temp.GetQuantity() > 1)
            {
                temp.SubQuantity(1);
            }
            else
            {
                int indexSlotToRemove = 0;

                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i].GetItem() == item)
                    {
                        indexSlotToRemove = i;
                        break;
                    }
                }

                items[indexSlotToRemove].Clear();
            }
        }
        else
        {
            return false;
        }

        RefreshUI();
        return true;
    }

    public void UseSelected()
    {
        items[selectedSlotIndex + (slots.Length - hotbarSlots.Length)].SubQuantity(1);
        if(items[selectedSlotIndex + (slots.Length - hotbarSlots.Length)].GetQuantity() <= 0)
        {
            items[selectedSlotIndex + (slots.Length - hotbarSlots.Length)].Clear();
        }
        RefreshUI();
    }

    //Clase que verifica si el inventario contiene un objeto en particular
    public SlotClass Contains(ItemClass item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].GetItem() == item)
            {
                return items[i];
            }
        }

        return null;
    }

    #endregion

    #region Moving Stuff

    private bool BeginItemMove()
    {
        if(isMovingItem)
        {
            return false;
        }

        originalSlot = new SlotClass(GetClossestSlot());
        if (originalSlot == null || originalSlot.GetItem() == null)
        {
            return false;
        }


        movingSlot = originalSlot;
        originalSlot.Clear();
        isMovingItem = true;
        RefreshUI();
        return true;
    }

    private bool BeginItemMove_Half()
    {
        originalSlot = GetClossestSlot();

        if (originalSlot == null || originalSlot.GetItem() == null)
        {
            return false;
        }

        if (Mathf.FloorToInt(originalSlot.GetQuantity() / 2f) < 1)
            return false;

        movingSlot = new SlotClass(originalSlot.GetItem(), Mathf.FloorToInt(originalSlot.GetQuantity() / 2f));
        originalSlot.SubQuantity(Mathf.FloorToInt(originalSlot.GetQuantity() / 2f)); 
        if (originalSlot.GetQuantity() < 1)
        {
            originalSlot.Clear();
        }

        isMovingItem = true;
        RefreshUI();
        return true;
    }

    private bool EndItemMove()
    {
        if (!isMovingItem)
        {
            return false;
        }

        originalSlot = GetClossestSlot();
        
        if(originalSlot == null) 
        {
            Add(movingSlot.GetItem(), movingSlot.GetQuantity());
            movingSlot.Clear();
        }
        else
        {
            if(originalSlot.GetItem() != null)
            {
                if(originalSlot.GetItem() == movingSlot.GetItem()) //Stackear objetos al ser del mismmo tipo
                {
                    originalSlot.AddQuantity(movingSlot.GetQuantity());
                    movingSlot.Clear();
                }
                else
                {
                    tempSlot = new SlotClass(originalSlot);
                    originalSlot.AddItem(movingSlot.GetItem(), movingSlot.GetQuantity());
                    movingSlot.AddItem(tempSlot.GetItem(), tempSlot.GetQuantity());
                    RefreshUI();
                    //isMovingItem = false;
                    return true;
                }
            }
            else //Regresar el objeto a su estado original
            {
                originalSlot.AddItem(movingSlot.GetItem(), movingSlot.GetQuantity());
                movingSlot.Clear();
            }
        }

        isMovingItem = false;
        itemCursor.SetActive(false);

        RefreshUI();
        return true;
    }

    private bool EndItemMove_Single()
    {
        originalSlot = GetClossestSlot();

        if (originalSlot == null)
        {
            return false;
        }

        movingSlot.SubQuantity(1);
        if(originalSlot.GetItem() != null && originalSlot.GetItem() == movingSlot.GetItem())
        {
            originalSlot.AddQuantity(1);
        }
        else
        {
            originalSlot.AddItem(movingSlot.GetItem(), 1);
        }
        
        if(movingSlot.GetQuantity() < 1)
        {
            isMovingItem = false;
            movingSlot.Clear();
        }
        else
        {
            isMovingItem = true;
        }

        isMovingItem = true;
        RefreshUI();
        return true;
    }

    private SlotClass GetClossestSlot()
    {
        for(int i = 0; i < slots.Length;i++ )
        {
            if (Vector2.Distance(slots[i].transform.position, Input.mousePosition) <= 64)
            {
                return items[i];
            }
        }

        return null;
    }

    #endregion

    private bool inventoryOpen;

    //Abrir inventario
    public bool OpenAndCloseInventory()
    {
        inventoryOpen = !inventoryOpen;
        return inventoryOpen;
    }
}
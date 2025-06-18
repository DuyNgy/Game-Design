using System;
using System.Collections.Generic;
using Project.Helper;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Project.Inventory;
using TMPro;
using UnityEditor;

public class InventorySlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image iconImage;
    private CanvasGroup canvasGroup;
    public ItemData itemData;
    public static Image dragImage;
    public static RectTransform dragRectTransform;
    public GameObject tooltip;
    //[SerializeField] private GameObject tooltipObject;
    private bool isPointerOver = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (tooltip == null)
        {
            tooltip = GameObject.Find("Tooltip");

            if (tooltip == null)
            {
                Debug.LogError("TooltipUI not found in the scene. Please make sure a TooltipUI object is present.");
            }
        }
    }

    void Update()
    {
        if (isPointerOver)
        {
            ShowTooltip();
        }
    }

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        // Find the shared drag image
        if (dragImage == null)
        {
            dragImage = GameObject.Find("DragImage").GetComponent<Image>();
            dragRectTransform = dragImage.GetComponent<RectTransform>();
        }
    }

    /// <summary>
    /// Sets the image in the inventory UI.
    /// </summary>
    /// <param name="icon">Sprite to be shown in the inventory.</param>
    public void SetIcon(Sprite icon)
    {
        iconImage.sprite = icon;
        iconImage.enabled = icon != null;
    }

    public void SetItemData(ItemData itemData)
    {
        this.itemData = itemData;
        SetIcon(itemData.icon);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Debug.Log("Pointer entered: " + itemData.itemName);
        tooltip.GetComponent<TooltipUI>().ChangeTooltip(itemData.itemName, itemData.description);
        isPointerOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Debug.Log("Pointer exited: " + itemData.itemName);
        isPointerOver = false;
        HideTooltip();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;

        // Show drag image
        dragImage.sprite = itemData.icon;
        dragImage.color = Color.white;
        dragImage.enabled = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        dragRectTransform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        // Hide drag image
        dragImage.enabled = false;

        // Raycast to check if dropped on a world object
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D[] hits = Physics2D.RaycastAll(worldPos, Vector2.zero);

        foreach (var hit in hits)
        {
            var receiver = hit.collider?.GetComponent<ItemReceiver>();
            if (receiver != null)
            {
                if (receiver.TryUseItem(itemData))
                {
                    // Do not remove Staff and Scissors
                    if (itemData.itemID is 56 or 59) return;
                    InventoryManager.Instance.RemoveItem(itemData);
                    InventoryUI.Instance.UpdateInventoryUI();
                    return;
                }
            }
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        InventorySlotUI droppedSlot = eventData.pointerDrag?.GetComponent<InventorySlotUI>();
        if (droppedSlot == null || droppedSlot.itemData == null || itemData == null) return;

        // Prevent combining the same item with itself
        /*if (droppedSlot.itemData == itemData)
        {
            Debug.LogWarning("Cannot combine an item with itself.");
            dragImage.enabled = false; // Ensure the drag image is hidden
            return;
        }*/

        // Check if combination is possible
        if (droppedSlot.itemData.CanCombine(itemData.itemID))
        {
            ItemData result = droppedSlot.itemData.GetCombinationResult(itemData.itemID);
            if (result != null)
            {
                Debug.Log($"Combined {droppedSlot.itemData.itemName} + {itemData.itemName} => {result.itemName}");

                // Remove both original items
                InventoryManager.Instance.RemoveItem(droppedSlot.itemData);
                InventoryManager.Instance.RemoveItem(itemData);

                // Add result item
                InventoryManager.Instance.AddItem(result);

                // Refresh UI
                FindFirstObjectByType<InventoryUI>().UpdateInventoryUI();

                isPointerOver = false;
                HideTooltip();
            }
            else
            {
                Debug.LogWarning("Combination result is null.");
            }
        }
        else
        {
            Debug.LogWarning($"Cannot combine {droppedSlot.itemData.itemName} with {itemData.itemName}.");
        }

        // Ensure drag image is hidden after combination attempt
        dragImage.enabled = false;
    }

    private void ShowTooltip()
    {
        if (itemData != null)
        {
            tooltip.GetComponent<TooltipUI>().SetShow(true);
        }
    }

    private void HideTooltip()
    {
        tooltip.GetComponent<TooltipUI>().SetShow(false);
        tooltip.GetComponent<Image>().enabled = false;  // Disable the background image
        foreach (Transform child in tooltip.transform)
        {
            child.gameObject.SetActive(false);  // Hide all tooltip child objects
        }
    }
}

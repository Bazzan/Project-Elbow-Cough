using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCanvas : MonoBehaviour
{
    [SerializeField] private bool isOpen = false;
    [SerializeField] private Image[] armorImages = null;

    private void Awake()
    {
        DisableCanvas(isOpen);
    }

    public void OpenInventory()
    {
        isOpen = !isOpen;
        DisableCanvas(isOpen);

    }

    private void DisableCanvas(bool isOpen)
    {
        gameObject.SetActive(isOpen);
    }

    public void AddItemToInventoryCanvas(int placement, Sprite sprite)
    {
        armorImages[placement].sprite = sprite;
    }

}

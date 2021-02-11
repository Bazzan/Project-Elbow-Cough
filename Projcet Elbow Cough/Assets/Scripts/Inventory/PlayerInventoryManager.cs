using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour
{
    [SerializeField] private Item[] currentArmor;
    [SerializeField] private InventoryCanvas inventoryCanvas = null;
    [SerializeField] private RaycastMiddleOfScreen raycastScreen = null;


    public void AddArmor(Item newArmor)
    {
        currentArmor[newArmor.placement] = newArmor;
        inventoryCanvas.AddItemToInventoryCanvas(newArmor.placement, newArmor.icon);
    }

    public void PickUpArmor()
    {
        if (raycastScreen.GetCurrentObject() == null)
        {
            Debug.Log("no current object");
            return;
        }
        // AddArmor(raycastScreen.GetCurrentObject().GetComponent<Item>());
    }
}

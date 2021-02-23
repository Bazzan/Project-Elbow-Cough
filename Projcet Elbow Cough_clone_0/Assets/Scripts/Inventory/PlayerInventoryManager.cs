using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour
{
    [SerializeField] private Item[] currentArmor;
    [SerializeField] private InventoryCanvas inventoryCanvas = null;
    [SerializeField] private RaycastMiddleOfScreen raycastScreen = null;
    [SerializeField] private ItemGenerator itemGenerator = null;


    public void AddArmor(Item newArmor)
    {
        if (currentArmor[newArmor.placement] != null)
        {
            ThrowAwayItem(currentArmor[newArmor.placement]);
        }
        currentArmor[newArmor.placement] = newArmor;
        inventoryCanvas.AddItemToInventoryCanvas(newArmor.placement, newArmor.icon);
    }

    public void PickUpArmor()
    {
        if (raycastScreen.GetCurrentItemPickup() == null)
        {
            Debug.Log("no current object");
            return;
        }
        AddArmor(raycastScreen.GetCurrentItemPickup());
        raycastScreen.KillCurrentItemPickup();

    }

    public void ThrowAwayItem(Item item)
    {
        itemGenerator.SpawnWorldItem(item, new Vector3(0, 0, 0));
    }

}

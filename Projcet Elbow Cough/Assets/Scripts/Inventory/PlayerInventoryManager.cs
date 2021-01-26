using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour
{
    [SerializeField] private Item[] currentArmor;
    [SerializeField] private InventoryCanvas inventoryCanvas = null;


    public void AddArmor(Item newArmor)
    {
        currentArmor[newArmor.placement] = newArmor;
        inventoryCanvas.AddItemToInventoryCanvas(newArmor.placement, newArmor.icon);
    }

}

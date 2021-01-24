using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour
{
    [SerializeField] private Item[] armor;
    [SerializeField] private InventoryCanvas inventoryCanvas = null;


    public void AddArmor(Item newArmor)
    {

    }

    public void GenerateItem()
    {
        Item generatedItem = armor[Random.Range(0, armor.Length)];
        inventoryCanvas.AddItemToInventoryCanvas(generatedItem.placement, generatedItem.icon);
    }
}

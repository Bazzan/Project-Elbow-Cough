using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private Item item;
    [SerializeField] private PlayerInventoryManager inventoryManager = null;

    private void Awake()
    {
        inventoryManager = FindObjectOfType<PlayerInventoryManager>();
    }

    public void SetWorldItem(Item item)
    {
        this.item = item;
        GetComponent<MeshFilter>().mesh = item.mesh;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<FPSCharacterController>())
        {
            inventoryManager.AddArmor(item);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("ItemPickup collided with something else than player");
        }
    }
}

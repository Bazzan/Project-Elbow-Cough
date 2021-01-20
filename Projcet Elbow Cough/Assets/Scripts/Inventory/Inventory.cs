using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private bool isOpen = false;

    private void Awake()
    {
        DisableCanvas(isOpen);
    }

    public void OpenInventory()
    {
        DisableCanvas(isOpen);
        isOpen = !isOpen;

    }

    private void DisableCanvas(bool isOpen)
    {
        gameObject.SetActive(isOpen);
    }

    public void GenerateItem()
    {
        
    }

}

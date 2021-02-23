using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastMiddleOfScreen : MonoBehaviour
{
    [SerializeField] private float range = 100f;
    [SerializeField] private Camera cam = null;
    [SerializeField] private ItemTooltip itemTooltip = null;

    private ItemPickup currentItemPickup = null;

    void Update()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, range) && hit.transform.gameObject.tag == "Item Pickup")
        {
            currentItemPickup = hit.transform.gameObject.GetComponent<ItemPickup>();
            Debug.Log("hit");
            itemTooltip.ShowTooltip(GetCurrentItemPickup(), true);
        }
        else
        {
            currentItemPickup = null;
            itemTooltip.ShowTooltip(GetCurrentItemPickup(), false);
        }
    }

    public Item GetCurrentItemPickup()
    {
        return currentItemPickup.GetItem();
    }

    public void KillCurrentItemPickup()
    {
        currentItemPickup.KillItemPickup();
    }
}

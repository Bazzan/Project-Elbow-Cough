using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastMiddleOfScreen : MonoBehaviour
{
    [SerializeField] private float range = 100f;
    [SerializeField] private Camera cam = null;

    private GameObject currentObject = null;

    void Update()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, range) && hit.transform.gameObject.tag == "Item Pickup")
        {
            currentObject = hit.transform.gameObject;
            Debug.Log("hit");
        }
        else
        {
            currentObject = null;
        }
    }

    public Item GetCurrentObject()
    {
        return currentObject.GetComponent<ItemPickup>().GetItem();
    }
}

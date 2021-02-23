using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTooltip : MonoBehaviour
{
    [SerializeField] GameObject mainBorder = null;
    [SerializeField] Sprite itemSprite = null;

    private void Awake()
    {
        mainBorder.SetActive(false);
    }

    public void ShowTooltip(Item item, bool showing)
    {
        mainBorder.SetActive(showing);
        itemSprite = item.icon;
    }
}

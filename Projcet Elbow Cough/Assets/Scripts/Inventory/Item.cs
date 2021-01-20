using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Test Item", menuName = "Test Item", order = 2)]
public class Item : ScriptableObject
{
    new public string name = "New item";
    public Sprite icon = null;
    public bool isDefaultItem = false;

}

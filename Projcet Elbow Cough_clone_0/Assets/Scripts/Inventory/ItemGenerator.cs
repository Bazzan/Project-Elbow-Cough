using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    [SerializeField] private Item[] armor;
    [SerializeField] private GameObject itemWorldObject = null;
    [SerializeField] private Transform spawnPoint = null;

    public void GenerateWorldItem()
    {
        Item generatedItem = armor[Random.Range(0, armor.Length)];
        SpawnWorldItem(generatedItem, spawnPoint.position);

    }

    public void SpawnWorldItem(Item item, Vector3 spawnPoint)
    {
        GameObject worldItem = Instantiate(itemWorldObject, spawnPoint, Quaternion.identity);
        worldItem.GetComponent<ItemPickup>().SetWorldItem(item);
    }
}

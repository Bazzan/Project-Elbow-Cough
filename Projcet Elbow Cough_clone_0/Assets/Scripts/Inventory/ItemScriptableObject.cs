using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Armor Item", menuName = "Armor Item", order = 1)]
public class ItemScriptableObject : ScriptableObject
{
    //how many stats are pulled from each list, for example 3 different stats from A and 2 different stats from B
    [SerializeField] private int statsFromA;
    [SerializeField] private int statsFromB;

    //guarantee that a certain stat ID will always be generated on this item
    [SerializeField] private int guaranteedStatIDA;
    [SerializeField] private int guaranteedStatIDB;

    //stats that are ignored in item generation, for example prevent helm from generating with crit on it
    [SerializeField] private int[] ignoredStatIDfromA;
    [SerializeField] private int[] ignoredStatIDfromB;

    [SerializeField] private StatIntervals[] array;

    [System.Serializable]
    struct StatIntervals
    {
        public float a; public float b;
    }



}

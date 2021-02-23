using UnityEngine;

public class GamePlayTesting : MonoBehaviour
{


    [SerializeField] private GameObject EnemieToSpawn;
    [SerializeField] private Transform spawnPoint;

    public static GamePlayTesting instance;

    private void Awake()
    {
        if(instance != null) instance = this;
    }


    public void SpawnEnemie()
    {
       Instantiate(EnemieToSpawn, spawnPoint.position, spawnPoint.rotation);
        
    }
    
    
}

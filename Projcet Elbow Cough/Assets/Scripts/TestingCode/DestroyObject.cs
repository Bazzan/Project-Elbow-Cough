using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    [SerializeField] private float timeToDestroy = default;

    private void Start()
    {
        timeToDestroy = Time.time + timeToDestroy;
    }

    private void Update()
    {
        if (Time.time < timeToDestroy) return;
        Destroy(gameObject);


    }
}

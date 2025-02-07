using UnityEngine;

public class OutOfBoundsDetection : MonoBehaviour
{
    [SerializeField] private int OutOfBoundsLayer;
    [SerializeField] private bool Respawn;
    [SerializeField] private GameObject objPrefab;
    private Vector3 position;
    private Quaternion rotation;
    private bool collided;

    private void Start()
    {
        position = transform.position;
        rotation = transform.rotation;
        collided = false;
    }

    private void OnTriggerEnter(Collider other)
    {;
        if (OutOfBoundsLayer != other.gameObject.layer || collided) return;
        if (Respawn)
        {
            collided = true;
            Instantiate(objPrefab, position, rotation);
        } 
        Destroy(gameObject);
    }
}

using UnityEngine;

public class OutOfBoundsRespawn : MonoBehaviour
{
    [SerializeField] private int OutOfBoundsLayer;
    [SerializeField] private GameObject objPrefab;
    private Vector3 position;
    private Quaternion rotation;
    private bool collided = false;

    private void Start()
    {
        position = transform.position;
        rotation = transform.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (OutOfBoundsLayer != other.gameObject.layer || collided) return;
        
        collided = true;
        Instantiate(objPrefab, position, rotation);
        Destroy(gameObject);
    }
}

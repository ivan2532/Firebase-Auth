using UnityEngine;

public class LoadingCircleRotator : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 5.0f;

    private Transform myTransform;

    private void Start()
    {
        myTransform = GetComponent<Transform>();
    }

    private void Update()
    {
        myTransform.Rotate(new Vector3(0.0f, 0.0f, rotationSpeed * Time.deltaTime));
    }
}

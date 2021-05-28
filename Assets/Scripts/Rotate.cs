using UnityEngine;

public class Rotate : MonoBehaviour
{
    private float rotateSpeed = 15f;
    private Transform _rotator;
    //в void Update() transform.Rotate(0, rotateSpeed, 0);

    void Start()
    {
        _rotator = GetComponent<Transform>();
    }

    void Update()
    {
        _rotator.Rotate(0, rotateSpeed*Time.deltaTime, 0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyRotator : MonoBehaviour
{
    [SerializeField] private Vector3 _rotateVelocity;
    [SerializeField] private Space _rotateSpace;
    [SerializeField] private float speed = 4f;

    private Vector3 minHeight;
    private Vector3 maxHeight;

    private void Start()
    {
        minHeight = transform.position;
        maxHeight = minHeight + new Vector3(0, 0.5f, 0);
    }

    void Update()
    {
        transform.Rotate(_rotateVelocity * Time.deltaTime, _rotateSpace);

        float lerpValue = (Mathf.Sin(speed * Time.time) + 1f) / 2f; // <0, 1> 
        transform.position = Vector3.Lerp(minHeight, maxHeight, lerpValue);
    }
}


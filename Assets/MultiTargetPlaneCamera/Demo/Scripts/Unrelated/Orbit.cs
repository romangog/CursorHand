using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    [SerializeField] private Transform center;
    [SerializeField] private bool IsClockwise;
    [SerializeField] private float speed;

    private void Update()
    {
        if (IsClockwise)
        {
            transform.RotateAround(center.position, Vector3.forward, Time.deltaTime * speed);
        }
        else
        {
            transform.RotateAround(center.position, Vector3.forward, -Time.deltaTime * speed);
        }
        this.transform.rotation = Quaternion.identity;
    }
}

using UnityEngine;

public class RotatingAxis : MonoBehaviour
{
    [SerializeField] Vector3 _relativeRotation;

    void Update()
    {
        this.transform.Rotate(_relativeRotation * Time.deltaTime, Space.Self);
    }
}

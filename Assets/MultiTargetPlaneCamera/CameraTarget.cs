using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    [Tooltip("Радиус цели (диаметр)")]
    [SerializeField] private float _size;
    public float Size => _size/2;
}

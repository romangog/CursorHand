using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    [Tooltip("������ ���� (�������)")]
    [SerializeField] private float _size;
    public float Size => _size/2;
}

using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [SerializeField] bool _IsSmoothFollowing;
    [SerializeField] Transform _player;
    [SerializeField] float _detectionRadius;
    [SerializeField] Collider[] _objectsInRadius;
    [SerializeField] List<CameraTarget> _targets;

    MultiTargetOptimizer _multiTarget;

    Vector3 _currentVelocity;
    Vector3 _targetPosition;
    Rigidbody _rigidbody;
    private void Awake()
    {
        _multiTarget = this.GetComponent<MultiTargetOptimizer>();
        _rigidbody = this.GetComponent<Rigidbody>();


        // ����� ��������� ��� ���� � ������
        _multiTarget.AddTargetsRange(FindObjectsOfType<CameraTarget>().ToList());
    }


    private void FixedUpdate()
    {
        // ������������ ������ ������� ������
        _targetPosition = _multiTarget.CalculateCameraPosition();

        // �������������� �������� ToIncludeBorders, ������� �����������,
        // ������ ��� ������ ��������� ������������� �� ������
        // lBorder, rBorder, bBorder � uBorder - �����. �����, ������, ������ � �������
        // ������ �������������� ����. ���������� ��� ������� ����� ��������������� ����� ���� ��� �� ����������


        // ���������� ���� ������
        if (_IsSmoothFollowing)
        {
            // ��� �������� ����������� ���������� ��������� ����������� �� Z � XY. Z - �������, XY - �����.
            // � ������ ������������ ��������� ��� Z
            _rigidbody.MovePosition(Vector3.SmoothDamp(this.transform.position, _targetPosition, ref _currentVelocity, Time.fixedDeltaTime * 10f));
        }
        else
        {
            _rigidbody.MovePosition(_targetPosition);
        }
    }
}

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


        // —разу добавл€ем все цели в реестр
        _multiTarget.AddTargetsRange(FindObjectsOfType<CameraTarget>().ToList());
    }


    private void FixedUpdate()
    {
        // –ассчитываем нужную позицию камеры
        _targetPosition = _multiTarget.CalculateCameraPosition();

        // Ќеоб€зательный параметр ToIncludeBorders, который опередел€ет,
        // должна или камера старатьс€ отталкиватьс€ от границ
        // lBorder, rBorder, bBorder и uBorder - соотв. левой, правой, нижней и верхней
        // границ пр€моугольного пол€. ”становить эти гарницы можно непосредственно через пол€ или из инспектора


        // ѕеремещаем туда камеру
        if (_IsSmoothFollowing)
        {
            // ƒл€ плавного перемещени€ рекомендую разделить перемещение по Z и XY. Z - плавнее, XY - резче.
            // ¬ данной конфигурации настроено под Z
            _rigidbody.MovePosition(Vector3.SmoothDamp(this.transform.position, _targetPosition, ref _currentVelocity, Time.fixedDeltaTime * 10f));
        }
        else
        {
            _rigidbody.MovePosition(_targetPosition);
        }
    }
}

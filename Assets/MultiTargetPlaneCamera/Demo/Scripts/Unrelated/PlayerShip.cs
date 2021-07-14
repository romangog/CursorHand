using UnityEngine;

public class PlayerShip : MonoBehaviour
{
    [SerializeField] Joystick _joystick;
    [SerializeField] Transform _engineTrails;

    Rigidbody _rigidbody;

    bool _AreEnginesOn;

    private void Awake()
    {
        _rigidbody = this.GetComponent<Rigidbody>();
    }
    void Update()
    {
        float angleToTurn = Vector2.SignedAngle(transform.forward, _joystick.Direction);

        _rigidbody.AddRelativeTorque(new Vector3(0, ((angleToTurn < 0) ? (angleToTurn * angleToTurn) : -(angleToTurn * angleToTurn)) * 0.0001f, 0));

        float joystickImpactSqr = (_joystick.Vertical * _joystick.Vertical + _joystick.Horizontal * _joystick.Horizontal);
        if (angleToTurn < 15f && joystickImpactSqr > 0.1f)
        {
            if(!_AreEnginesOn)
            {
                _engineTrails.gameObject.SetActive(true);
                _AreEnginesOn = true;
            }

        }
        else
        {
            if (_AreEnginesOn)
            {
                _engineTrails.gameObject.SetActive(false);
                _AreEnginesOn = false;
            }

        }
        if (_AreEnginesOn)
        {
            _rigidbody.AddRelativeForce(Vector3.forward * joystickImpactSqr, ForceMode.Force);
            _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, 5f);
        }
    }
}

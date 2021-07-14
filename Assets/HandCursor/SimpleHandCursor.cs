
using UnityEngine;
using UnityEngine.UI;

public class SimpleHandCursor : HandCursor
{
    [SerializeField] Transform handReal;
    [SerializeField] Transform handShadow;

    public override void OnPointerDown()
    {
        handReal.Rotate(Vector3.up * 50, Space.Self);
        handShadow.localScale = new Vector3(0.8f, 0.9f, 1);
        handShadow.localPosition = Vector3.zero;

        handDownEvent.Invoke();
    }

    public override void OnPointerUp()
    {
        handReal.Rotate(-Vector3.up * 50, Space.Self);
        handShadow.localScale = new Vector3(1, 1, 1);
        handShadow.localPosition = new Vector3(27.7f, -18.2f, 0);
    }
}

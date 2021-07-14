using UnityEngine;

public class InputModuleExample : MonoBehaviour
{
    HandCursor _cursor;
    private void Awake()
    {
        _cursor = this.GetComponent<HandCursor>();
    }

    public void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            _cursor.OnPointerDown();
        }
        if (Input.GetMouseButtonUp(0))
        {
            _cursor.OnPointerUp();
        }
    }
}

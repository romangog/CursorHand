using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class HandCursor : MonoBehaviour
{
    [SerializeField] protected KeyCode activationKey = KeyCode.H;
    [SerializeField] protected Transform handMain;
    
    // Событие виртуального клика. Нужно для анимированной руки,
    // чтобы клик засчитывался не по нажатию ЛКМ, а по "прикосновению" анимации
    public UnityEvent handDownEvent { get; private set; } = new UnityEvent();

    protected bool IsOn = false;

    protected virtual void Update()
    {
        if (Input.GetKeyDown(activationKey))
        {
            SwitchCursorMode();
        }

        if (IsOn)
        {
            handMain.GetComponent<RectTransform>().anchoredPosition= Input.mousePosition;
        }
    }
    public abstract void OnPointerDown();

    public abstract void OnPointerUp();

    protected virtual void SwitchCursorMode()
    {
        IsOn = !IsOn;

        if (IsOn)
            handMain.gameObject.SetActive(true);
        else
            handMain.gameObject.SetActive(false);
    }
}

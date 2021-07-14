using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedHandCursor : HandCursor
{
    [SerializeField] private ParticlesPool _particlesPool;
    Animator _animator;

    private void Awake()
    {
        _animator = this.GetComponent<Animator>();
    }

    public void OnPointerDownAnimationFinished()
    {
        Debug.Log("Click");
        handDownEvent.Invoke();
        _particlesPool.ActivateNext();
    }

    public override void OnPointerDown()
    {
        _animator.SetBool("IsDown", true);
    }

    public override void OnPointerUp()
    {
        _animator.SetBool("IsDown", false);
    }
    protected override void SwitchCursorMode()
    {
        base.SwitchCursorMode();
        _particlesPool.SetInactive();

    }


}

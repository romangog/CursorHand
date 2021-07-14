using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    Animator _animator;
    bool IsRed = false;
    private void Awake()
    {
        _animator = this.GetComponent<Animator>();
    }

    internal void SwitchState()
    {
        IsRed = !IsRed;
         _animator.SetBool("IsRed", IsRed);
    }
}

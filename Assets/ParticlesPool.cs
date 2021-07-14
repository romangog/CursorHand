using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesPool : MonoBehaviour
{
    [SerializeField] GameObject[] _particles;

    int currentindex = 0;


    // Чтобы партиклы работали нужно поместить руку на канвас ScreenSpace - Camera
    public void ActivateNext()
    {
        _particles[currentindex].SetActive(false);
        if(++currentindex == _particles.Length)
        {
            currentindex = 0;
        }
        _particles[currentindex].SetActive(true);
    }

    internal void SetInactive()
    {
        foreach (var item in _particles)
        {
            item.SetActive(false);
        }
    }
}

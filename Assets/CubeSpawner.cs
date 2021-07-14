using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] Animator _animator;

    [SerializeField] HandCursor _hand;


    Transform[] _allCubes;
    Plane cubesPlane;
    private void Awake()
    {
        cubesPlane = new Plane(Vector3.back, new Vector3(0, 0, -0.5f));
        _allCubes = new Transform[this.transform.childCount];
        for (int i = 0; i < this.transform.childCount; i++)
        {
            _allCubes[i] = this.transform.GetChild(i);
        }
        _hand.handDownEvent.AddListener(OnHandDown);
    }


    private void OnHandDown()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        cubesPlane.Raycast(ray, out float distance);

        StartCoroutine(SpawnCubesRoutine(ray.GetPoint(distance)));
    }

    private IEnumerator SpawnCubesRoutine(Vector3 start)
    {
        float time = 0;
        List<Cube> affectedCubes = new List<Cube>();
        while (time < 3f)
        {
            time += Time.deltaTime;
            foreach (var item in Physics.OverlapSphere(start, time * 10f))
            {
                Cube cube = item.transform.parent.GetComponent<Cube>();
                if (!affectedCubes.Contains(cube))
                {
                    cube.SwitchState();
                    affectedCubes.Add(cube);
                }
            }
            yield return null;
        }
    }

}

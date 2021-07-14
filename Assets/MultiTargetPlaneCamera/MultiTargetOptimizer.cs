using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MultiTargetOptimizer : MonoBehaviour
{
[Tooltip("Отступ крайнего захватываемого объекта от границы экрана")]
    /// <summary>
    /// Отступ крайнего захватываемого объекта от границы экрана
    /// </summary>
    public float offsetBorders = 1f;
[Tooltip("Минимальное расстояние от камеры до наблюдаемого объекта")]
    /// <summary>
    /// Минимальное расстояние от камеры до наблюдаемого объекта
    /// </summary>
    public float minCameraDistance = 4f;

    [Header("Настройка промежуточной зоны")]
    // Внутри промежуточной зоны объекты сохраняют свой статус отслеживаемости
    // т.е. если они пришли в эту зону издалека, то они остаются неотслеживаемыми, пока не пересекут ближний порог,
    // а если они пришли в нее через ближний порог, они остаются в кадре, пока не пересекут дальний
    [Tooltip("Расстояние, до которого объекты будут гаранитрованно отслеживаться")]
    /// <summary>
    /// Расстояние, до которого объекты будут гаранитрованно отслеживаться
    /// </summary>
    public float closestThreshold = 13f;
    [Tooltip("Расстояние, дальше которого объекты гарантированно перестанут отслеживаться")]
    /// <summary>
    /// Расстояние, дальше которого объекты гарантированно перестанут отслеживаться
    /// </summary>
    public float fartherThreshold = 15f;


    // Пределы захватываемой области
    [Header("Пределы захватываемой прямоугольной области")]
    public float lBorder;
    public float rBorder;
    public float uBorder;
    public float bBorder;

    public CameraTarget MainTarget;

    [Space]
    [Header("Захватываемые объекты")]
    [SerializeField] private List<CameraTarget> _objects = new List<CameraTarget>();

    private List<bool> _wasCaptured = new List<bool>();

    private Camera _camera;
    private float h;

    public int ObjectsCount { get => _objects.Count; }

    void Awake()
    {
        _camera = this.GetComponent<Camera>();
        if(MainTarget!= null) AddTarget(MainTarget);
    }

    public void SetBorders(float left, float right, float bottom, float upper)
    {
        lBorder = left;
        rBorder = right;
        bBorder = bottom;
        uBorder = upper;
    }

    private Vector3 CalculatePositionForTargets(List<CameraTarget> targets, bool AreTargetsForcedToUse,  bool ToIncludeBorders = false)
    {
        float minY = float.MaxValue, minX = float.MaxValue;
        float maxY = float.MinValue, maxX = float.MinValue;
        float fieldPosZ = targets[0].transform.position.z;

        if (MainTarget == null) MainTarget = targets[0];

        for (int i = 0; i < targets.Count; i++)
        {
            if (!AreTargetsForcedToUse)
            {
                float distance = Vector3.Distance(targets[i].transform.position, MainTarget.transform.position) + targets[i].Size;

                if (targets[i] != MainTarget)
                {
                    // Если объект в межзонье
                    if (distance < fartherThreshold && distance > closestThreshold)
                    {
                        if (!_wasCaptured[i])
                        {
                            _wasCaptured[i] = false;
                            continue;
                        }
                    }
                    if (distance > fartherThreshold)
                    {
                        _wasCaptured[i] = false;
                        continue;
                    }
                }
                _wasCaptured[i] = true;

            }

            maxX = Mathf.Max(targets[i].transform.position.x, maxX);
            minX = Mathf.Min(targets[i].transform.position.x, minX);
            maxY = Mathf.Max(targets[i].transform.position.y, maxY);
            minY = Mathf.Min(targets[i].transform.position.y, minY);
        }

        if (ToIncludeBorders)
        {
            maxX = Mathf.Min(rBorder, maxX);
            minX = Mathf.Max(lBorder, minX);
            maxY = Mathf.Min(uBorder, maxY);
            minY = Mathf.Max(bBorder, minY);
        }

        float xDif = maxX - minX;
        float yDif = maxY - minY;
        if (yDif == 0) yDif = 0.0001f;
        if (xDif == 0) xDif = 0.0001f;
        float x, y;

        if (yDif == 0 || xDif / yDif > _camera.aspect)
        {
            x = xDif;
            y = x / _camera.aspect + offsetBorders * 2;
            x += offsetBorders * 2;
        }
        else
        {
            y = yDif;
            x = y * _camera.aspect + offsetBorders * 2;
            y += offsetBorders * 2;
        }


        float aSqr = y * y / (2f * (1f - Mathf.Cos(_camera.fieldOfView * Mathf.Deg2Rad)));
        float bfSqr = (y * y + x * x) / 4f;

        float newH = Mathf.Sqrt(aSqr - bfSqr);
        if (!float.IsInfinity(newH) && !float.IsNaN(newH) ) h = newH;
        h = Mathf.Clamp(h, minCameraDistance, float.MaxValue);

        Vector3 finalOffset = Vector3.zero;
        Vector3 center = new Vector3((maxX + minX) / 2f, (maxY + minY) / 2f, 0);

        if (ToIncludeBorders)
        {
            if (center.x + x * 0.5f > rBorder)
            {
                finalOffset.x -= (center.x + x * 0.5f) - rBorder;
            }
            if (center.x - x * 0.5f < lBorder)
            {
                finalOffset.x += lBorder - (center.x - x * 0.5f);
            }
            if (center.y + y * 0.5f > uBorder)
            {
                finalOffset.y -= (center.y + y * 0.5f) - uBorder;
            }
            if (center.y - y * 0.5f < bBorder)
            {
                finalOffset.y += bBorder - (center.y - y * 0.5f);
            }
        }

        // MAGIC VALUE DETECTED
        return new Vector3(center.x, center.y, fieldPosZ - h * 1.13768f) + finalOffset / 2f;
    }
    
    /// <summary>
    /// Возвращает позицию камеры с учетом захвата целей, добавленных через AddTarget()
    /// </summary>
    /// <param name="ToIncludeBorders">Нужно ли учитывать установленные границы</param>
    /// <returns></returns>
    public Vector3 CalculateCameraPosition(bool ToIncludeBorders = true)
    {
        return CalculatePositionForTargets(_objects, false,  ToIncludeBorders);
    }
    /// <summary>
    /// Возвращает позицию камеры для захвата целей targets
    /// </summary>
    /// <param name="targets">Цели для захвата</param>
    /// <param name="ToIncludeBorders">Нужно ли учитывать установленные границы</param>
    /// <returns></returns>
    //public Vector3 CalculateCameraPosition(List<CameraTarget> targets, bool ToIncludeBorders = true)
    //{
    //    return CalculatePositionForTargets(targets,true, ToIncludeBorders);
    //}



    /// <summary>
    /// Добавляет объект в список наблюдаемых
    /// </summary>
    /// <param name="target">Объект наблюдения</param>
    public void AddTarget(CameraTarget target)
    {
        _objects.Add(target);
        _wasCaptured.Add(true);
    }

    public void AddTargetsRange(List<CameraTarget> targets)
    {
        foreach (var item in targets)
            AddTarget(item);
    }
    /// <summary>
    /// Исключает объект из списка наблюдаемых
    /// </summary>
    /// <param name="target">Наблюдаемый объект</param>
    public void RemoveTarget(CameraTarget target)
    {
        int i = _objects.IndexOf(target);
        _objects.Remove(target);
        _wasCaptured.RemoveAt(i);
    }

    /// <summary>
    /// Очистить список наблюдаемых объектов
    /// </summary>
    public void Clear()
    {
        _objects.Clear();
        _wasCaptured.Clear();
    }

}

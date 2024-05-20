using UnityEngine;

[CreateAssetMenu(fileName = "UICanvasSettings", menuName = "ScriptableObjects/UICanvasSettings", order = 1)]
public class UICanvasSettings : ScriptableObject
{
    public float positionTolerance;
    public float spawnDistance;
    public float positionLerpSpeed;
    public float rotationLerpSpeed;
    public float upOffset;
    public float rightOffset;
    public bool limitYRange;
    public float minY;
    public float maxY;
}

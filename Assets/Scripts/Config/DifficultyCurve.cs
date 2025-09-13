using UnityEngine;

[CreateAssetMenu(menuName = "Configs/DifficultyCurve")]
public class DifficultyCurve : ScriptableObject
{
    public float a = 0.1f;
    public float b = 1f;
    public float c = 0.1f;
    public float d = 0.05f;
    public float e = 0.1f;
    public float SR0 = 1f;
}


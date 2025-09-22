using UnityEngine;

public class CalculationUtils : MonoBehaviour
{
    public string ConvertFloatToTime(float time)
    {
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);

        return string.Format("{0}:{1}", minutes, seconds);
    }
}

using System.Globalization;
using UnityEngine;

public class PlayerDataServer : MonoBehaviour
{
    public bool isEnded;
    public int gameModeScore;
    public float gameModeTime;

    void Start()
    {
        isEnded = false;
    }

    public void UpdateOrdinalMultiplicationExamplesDyTableGameModeState(bool state)
    {
        isEnded = state;
    }

    public void UpdateScore(int value)
    {
        gameModeScore = value;
    }

    public void UpdateTime(float millis)
    {
        gameModeTime = millis;
    }
}

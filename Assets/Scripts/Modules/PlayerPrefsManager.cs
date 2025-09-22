/*
========================
| Player prefs manager |
========================
*/

using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
    //writing value to game mode pref
    public void WriteGameModePref(string value)
    {
        PlayerPrefs.SetString("GameMode", value);
    }

    //writing value to int pref
    public void WriteToIntPref(string prefName, int value)
    {
        PlayerPrefs.SetInt(prefName, value);
    }

    //extracting value from int pref
    public int ExtractValueFromIntPref(string prefName)
    {
        return PlayerPrefs.GetInt(prefName, 0);
    }

    //writing value to float pref
    public void WriteToFloatPref(string prefName, float value)
    {
        PlayerPrefs.SetFloat(prefName, value);
    }

    //extracting value from float pref
    public float ExtractValueFromFloatPref(string prefName)
    {
        return PlayerPrefs.GetFloat(prefName, 0f);
    }

    //writing value to string pref
    public void WriteToStringPref(string prefName, string value)
    {
        PlayerPrefs.SetString(prefName, value);
    }

    //extracting value from string pref
    public string ExtractValueFromStringPref(string prefName)
    {
        return PlayerPrefs.GetString(prefName, "");
    }

    //extracting value from game mode pref
    public string ExtractValueFromGameModePref()
    {
        return PlayerPrefs.GetString("GameMode");
    }
}

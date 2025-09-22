using System;
using UnityEngine;

public class InitUpperScoreBarType : MonoBehaviour
{
    [Serializable]
    public class ScoreBars
    {
        public GameObject scoreBar;
        public GameObject timeBar;
    }

    public ScoreBars scoreBars;

    void Start()
    {
        PlayerPrefsManager prefsManager = gameObject.AddComponent<PlayerPrefsManager>();
        

        bool gameMode = bool.Parse(prefsManager.ExtractValueFromGameModePref());

        switch (gameMode)
        {
            case true:
                scoreBars.scoreBar.SetActive(true);
                break;
            case false:
                scoreBars.timeBar.SetActive(true);
                break;
        }
    }
}

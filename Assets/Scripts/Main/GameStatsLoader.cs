using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameStatsLoader : MonoBehaviour
{

    [SerializeField] private TMP_Text statsView;
    [SerializeField] private GameObject statsSlider, timeDescription, scoreDescription, scoreDescription2;

    void Start()
    {
        PlayerPrefsManager prefsManager = gameObject.AddComponent<PlayerPrefsManager>();
        GlobalValues globalValues = gameObject.AddComponent<GlobalValues>();
        CalculationUtils calculationUtils = gameObject.AddComponent<CalculationUtils>();

        Slider slider = statsSlider.GetComponent<Slider>();

        bool gameMode = bool.Parse(prefsManager.ExtractValueFromGameModePref());

        switch (gameMode)
        {
            case true:
                int oldScore = prefsManager.ExtractValueFromIntPref("SolvedPercent");
                int score = prefsManager.ExtractValueFromIntPref("SolvedExamplesPercent");

                slider.value = score;
                statsView.text = string.Format("{0}/{1}", score, globalValues.all_ordinal_examples_count);

                if (score > oldScore)
                {
                    prefsManager.WriteToIntPref("SolvedPercent", score);
                }

                prefsManager.WriteToIntPref("SolvedExamplesPercent", 0);
                break;
            case false:
                float oldTime = prefsManager.ExtractValueFromFloatPref("Time");
                float time = prefsManager.ExtractValueFromFloatPref("GameTime");

                double formattedTime = Math.Round((double) time);

                scoreDescription.SetActive(false);
                scoreDescription2.SetActive(false);
                timeDescription.SetActive(true);

                statsView.text = string.Format("{0}/{1} m", formattedTime, calculationUtils.ConvertFloatToTime(globalValues.time_remainig));

                statsSlider.SetActive(false);

                if (time > oldTime)
                {
                    prefsManager.WriteToFloatPref("Time", time);
                }

                prefsManager.WriteToFloatPref("GameTime", 0f);
                break;
        }
    }
}

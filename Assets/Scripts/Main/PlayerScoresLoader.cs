using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScoresLoader : MonoBehaviour
{
    [SerializeField] private TMP_Text percentScoreView, timeView;
    [SerializeField] private Slider percentSlider;

    void Start()
    {
        PlayerPrefsManager prefsManager = gameObject.AddComponent<PlayerPrefsManager>();
        CalculationUtils calculationUtils = gameObject.AddComponent<CalculationUtils>();
        GlobalValues globalValues = gameObject.AddComponent<GlobalValues>();

        int score = prefsManager.ExtractValueFromIntPref("SolvedPercent");
        float time = prefsManager.ExtractValueFromFloatPref("Time");

        string playerCurrentTimeScore = calculationUtils.ConvertFloatToTime(time);
        string timeRemaining = calculationUtils.ConvertFloatToTime(globalValues.time_remainig);

        percentScoreView.text = string.Format("{0}/{1}", score, globalValues.all_ordinal_examples_count);
        percentSlider.value = score;
        timeView.text = string.Format("{0}/{1} m", playerCurrentTimeScore, timeRemaining);
    }
}

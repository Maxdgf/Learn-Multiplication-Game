using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CountDownTimer : MonoBehaviour
{
    [SerializeField] private TMP_Text timerView;
    [SerializeField] private Slider slider;
    [SerializeField] private PlayerDataServer dataServer;

    private float timeNow;
    private CalculationUtils calculationUtils;
    private GlobalValues globalValues;

    private void Start()
    {
        timeNow = 0f;

        calculationUtils = gameObject.AddComponent<CalculationUtils>();
        globalValues = gameObject.AddComponent<GlobalValues>();

        slider.maxValue = globalValues.time_remainig;
    }

    void Update()
    {
        if (Time.timeScale > 0f)
        {
            if (timeNow < globalValues.time_remainig)
            {
                timeNow += Time.deltaTime;
                slider.value = timeNow;
                dataServer.UpdateTime(timeNow);

                string formattedTime = calculationUtils.ConvertFloatToTime(timeNow);
                timerView.text = formattedTime;
            }
        }
    }
}

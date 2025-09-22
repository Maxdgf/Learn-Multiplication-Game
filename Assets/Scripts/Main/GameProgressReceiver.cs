using UnityEngine;

public class GameProgressReceiver : MonoBehaviour
{
    [SerializeField] private PlayerDataServer dataServer;

    private bool gameMode;
    private SceneChanger sceneChanger;
    private PlayerPrefsManager prefsManager;
    private CalculationUtils calculationUtils;
    private GlobalValues globalValues;

    void Start()
    {
        prefsManager = gameObject.AddComponent<PlayerPrefsManager>();
        sceneChanger = gameObject.AddComponent<SceneChanger>();
        calculationUtils = gameObject.AddComponent<CalculationUtils>();
        globalValues = gameObject.AddComponent<GlobalValues>();

        gameMode = bool.Parse(prefsManager.ExtractValueFromGameModePref());
    }

    void Update()
    {
        switch (gameMode)
        {
            case true:
                if (dataServer.isEnded)
                {
                    if (dataServer.gameModeScore == globalValues.all_ordinal_examples_count)
                    {
                        prefsManager.WriteToIntPref("SolvedPercent", dataServer.gameModeScore);
                        sceneChanger.ChangeScene("WinScene");
                    } 
                    else
                    {
                        prefsManager.WriteToIntPref("SolvedExamplesPercent", dataServer.gameModeScore);
                        sceneChanger.ChangeScene("GameOver");
                    }
                }

                break;
            case false:
                if (dataServer.gameModeTime > globalValues.time_remainig)
                {
                    prefsManager.WriteToStringPref("Time", calculationUtils.ConvertFloatToTime(dataServer.gameModeTime));
                    sceneChanger.ChangeScene("WinScene");
                }

                break;
        }
    }
}

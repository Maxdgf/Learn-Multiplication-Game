using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//sound effects
[Serializable]
public class Sounds
{
    public AudioClip exampleGeneration;
    public AudioClip wrongAnswer;
    public AudioClip correctAnswer;
}

//animations
[Serializable]
public class Animations
{
    public string answerButtonsCaptionAnimation;
    public string exampleViewAnimation;
}

//example accuracy icons
[Serializable]
public class ExampleIcons
{
    public Sprite correctIcon;
    public Sprite incorrectIcon;
}

public class MathExamplesGenerator : MonoBehaviour
{
    //const values
    private const float ANIMATION_PLAY_DIFFERENCE_FACTOR = 0.1f; //answer button text animation play difference factor
    private const float BLOCK_PANEL_ACTIVE_TIME = 0.5f; //fast click bloca panel active duration
    private const int START_FIRST_NUMBER = 2; //first example num
    private const int START_SECOND_NUMBER = 1; //second example num

    [SerializeField] private TMP_Text example_view; //example view
    [SerializeField] private Button[] answer_buttons; //answer buttons group
    [SerializeField] private Slider points_slider; //player score progressbar
    [SerializeField] private TMP_Text points_view; //score text view
    [SerializeField] private AudioSource audioSource; //for sound effects
    [SerializeField] private GameObject playerCharacter, exampleViewPanel,exampleAnwserCorrectionIcon, blockPanel; //importnat gameobjects
    [SerializeField] private string answerButtonsCaptionAnimationName; //answer button text animation name
    [SerializeField] private PlayerDataServer dataServer; //game data server
    [SerializeField] private float buttonAlphaLevel;

    //private variables
    private bool game_mode;
    private int first_number;
    private int second_number;
    private int player_score;
    private int current_math_example_result;

    //private classes
    private AudioPlayer audioPlayer;
    public Sounds sounds;
    private PlayerController playerController;
    public Animations animations;
    private AnimationManager animationManager;
    private Animator exampleViewAnimator;
    public ExampleIcons exampleIcons;
    private PlayerPrefsManager playerPrefsManager;

    void Start()
    {
        System.Random rnd = new System.Random();

        audioPlayer = gameObject.AddComponent<AudioPlayer>();
        audioPlayer.audioSource = audioSource;

        playerController = playerCharacter.GetComponent<PlayerController>();

        animationManager = gameObject.AddComponent<AnimationManager>();
        exampleViewAnimator = example_view.GetComponent<Animator>();

        playerPrefsManager = gameObject.AddComponent<PlayerPrefsManager>();

        game_mode = bool.Parse(playerPrefsManager.ExtractValueFromGameModePref());

        switch (game_mode)
        {
            case true:
                first_number = START_FIRST_NUMBER;
                second_number = START_SECOND_NUMBER;
                break;

            case false:
                first_number = rnd.Next(1, 9);
                second_number = rnd.Next(1, 9);
                break;
        }

        foreach (Button button in answer_buttons)
        {
            string buttonName = button.name;
            button.onClick.AddListener(delegate { OnClickAnswerButtonFunction(buttonName, button); });
        }

        GenerateMathExample();
        SetTextToAnswerButtons(answer_buttons);
    }

    private TMP_Text GetTextFromButton(string button_name)
    {
        GameObject text_object = GameObject.Find($"{button_name}/caption");
        return text_object.GetComponent<TMP_Text>();
    }

    private void SetTextToAnswerButtons(Button[] buttons)
    {
        System.Random rnd = new System.Random();

        int counter = 0;
        float animationDelay = 0f;
        int random_answer_position = rnd.Next(1, buttons.Length);

        StartCoroutine(ManageBlockFastClickPanel(BLOCK_PANEL_ACTIVE_TIME));

        foreach (Button button in buttons)
        {
            button.enabled = true;

            Image buttonImage = button.GetComponent<Image>();
            buttonImage.color = new Color(1f, 1f, 1f, 1f);

            counter++;
            animationDelay += ANIMATION_PLAY_DIFFERENCE_FACTOR;

            TMP_Text text_view = GetTextFromButton(button.name);
            Animator text_animator = text_view.GetComponent<Animator>();

            StartCoroutine(animationManager.PlayAnimationAfterDelay(text_animator, animations.answerButtonsCaptionAnimation, animationDelay, false));

            if (counter == random_answer_position)
            {
                text_view.text = current_math_example_result.ToString();
            } 
            else
            {
                int random_num = rnd.Next(1, 81);

                if (random_num == current_math_example_result)
                {
                    int edited_num = random_num + rnd.Next(1, 5);
                    text_view.text = edited_num.ToString();
                } 
                else
                {
                    text_view.text = random_num.ToString();
                }
            }
        }
    }

    private void OnClickAnswerButtonFunction(string button_name, Button button)
    {
        DisableButtons();

        StartCoroutine(playerController.MoveWings());

        TMP_Text text_view = GetTextFromButton(button_name);

        string value = text_view.text;
        int answer = int.Parse(value);

        example_view.text = string.Format("{0}{1}", example_view.text, value);

        if (answer == current_math_example_result)
        {
            player_score++;
            dataServer.UpdateScore(player_score);

            points_slider.value = player_score;
            points_view.text = player_score.ToString();

            audioPlayer.PlayAudio(sounds.correctAnswer);
            playerController.AddYForceToPlayer(true);
             
            StartCoroutine(SignalizeExampleCorrecthness(0.3f, exampleIcons.correctIcon, Color.green));
        } 
        else
        {
            audioPlayer.PlayAudio(sounds.wrongAnswer);
            Handheld.Vibrate();

            StartCoroutine(SignalizeExampleCorrecthness(0.3f, exampleIcons.incorrectIcon, Color.red));
        }
    }

    private void GenerateMathExample()
    {
        StartCoroutine(audioPlayer.PlayAudioAfterDelay(sounds.exampleGeneration, 0.6f));

        string result_example = string.Format("{0} x {1} = ", first_number, second_number);

        ExpressionEvaluator.Evaluate($"{first_number} * {second_number}", out current_math_example_result);
        example_view.text = result_example;

        if (game_mode)
        {
            if (first_number <= 9)
            {
                second_number++;

                if (second_number > 10)
                {
                    first_number++;
                    second_number = 1;
                }
            } 
            else
            {
                dataServer.isEnded = true;
            }
        } 
        else
        {
            System.Random rnd = new System.Random();

            first_number = rnd.Next(1, 9);
            second_number = rnd.Next(1, 9);
        }

        animationManager.PlayAnimation(exampleViewAnimator, animations.exampleViewAnimation, false);
    }

    private void DisableButtons()
    {
        foreach (Button button in answer_buttons)
        {
            button.enabled = false;

            Image buttonImage = button.GetComponent<Image>();
            buttonImage.color = new Color(1f, 1f, 1f, buttonAlphaLevel);
        }
    }

    private IEnumerator SignalizeExampleCorrecthness(float duration, Sprite sprite, Color examplePanelColor)
    {
        exampleAnwserCorrectionIcon.SetActive(true);

        Image image = exampleAnwserCorrectionIcon.GetComponent<Image>();
        Image exampleImage = exampleViewPanel.GetComponent<Image>();
        Color startColor = exampleImage.color;

        image.sprite = sprite;
        exampleImage.color = examplePanelColor;

        yield return new WaitForSeconds(duration);

        exampleImage.color = startColor;
        exampleAnwserCorrectionIcon.SetActive(false);

        GenerateMathExample();
        SetTextToAnswerButtons(answer_buttons);
    }

    private IEnumerator ManageBlockFastClickPanel(float activeTime)
    {
        blockPanel.SetActive(true);

        yield return new WaitForSeconds(activeTime);

        blockPanel.SetActive(false);
    }
}
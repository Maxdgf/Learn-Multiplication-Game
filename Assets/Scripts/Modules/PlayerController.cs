/*
============================
| Player controller script.|
============================
*/

using System;
using UnityEngine;
using System.Collections;

//a collection of danger areas(objects) tags for player
[Serializable]
public class DangerTagsForPlayer
{
    public string upperZoneTag;
    public string lowerZoneTag;
}

//a collection of AudioClip's for player sound effects
[Serializable]
public class PlayerSoundEffects
{
    public AudioClip upperZoneCollision;
    public AudioClip fallDown;
    public AudioClip wingsSound;
}

//a collection of player state sprites (for player wings move animation)
[Serializable]
public class StateSprites
{
    public Sprite actionState;
    public Sprite normalState;
    public Sprite confusedState;
}

public class PlayerController : MonoBehaviour
{
    private const float WINGS_MOVE_DURATION = 0.3f; //birdy wings animation duration constant value (float)
    private const int FORCE_REDUCTION_FACTOR = 2; //force player reduction factor (int)

    [SerializeField] private float forcePower; //force applied to the player
    [SerializeField] private AudioSource soundSource; //player AudioSource for sound effects
    [SerializeField] private PlayerDataServer dataServer; //player data server object
    [SerializeField] private GameObject interactivePanel, gameStatusBar; //game status ui bars
    [SerializeField] private int upAngle, fallAngle; //player rotation angles

    private bool gameMode; //current gamemode
    private Rigidbody2D rigidBody2D; //player rigidbody2D
    private SpriteRenderer spriteRenderer; //player spriterenderer
    public DangerTagsForPlayer dangerTagsForPlayer; //danger tags collection
    private AudioPlayer audioPlayer; //audio player
    public PlayerSoundEffects playerSoundEffects; //player sound effects collection
    private SceneChanger sceneChanger; //scene changer
    public StateSprites stateSprites; //player state sprites collection
    private PlayerPrefsManager prefsManager; //player prefs manager

    void Start()
    {
        //getting important components
        rigidBody2D = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        //adding important modules
        audioPlayer = gameObject.AddComponent<AudioPlayer>();
        sceneChanger = gameObject.AddComponent<SceneChanger>();
        prefsManager = gameObject.AddComponent<PlayerPrefsManager>();

        gameMode = bool.Parse(prefsManager.ExtractValueFromGameModePref()); //getting current game mode

        audioPlayer.audioSource = soundSource; //setting player AudioSource to main AudioSource object in AudioPlayer module
    }

    void Update()
    {
        //player rotation when he falling or moving up.
        if (rigidBody2D.linearVelocityY < 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, fallAngle); //falling
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, upAngle); //moving up
        }
    }

    //player collision detection
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(dangerTagsForPlayer.upperZoneTag))
        {
            Handheld.Vibrate(); //vibration

            audioPlayer.PlayAudio(playerSoundEffects.upperZoneCollision);
            StartCoroutine(CollisionReaction(1f));

            AddYForceToPlayer(false);
        }
    }

    //player trigger enter detection
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(dangerTagsForPlayer.lowerZoneTag))
        {
            Handheld.Vibrate(); //vibration

            audioPlayer.PlayAudio(playerSoundEffects.fallDown);

            spriteRenderer.sprite = null; //making player is invisible
            rigidBody2D.gravityScale = 0f; //stop player falling down

            interactivePanel.SetActive(false); //--|
            gameStatusBar.SetActive(false); //-----| making ui panels is invisible

            //saving player stats by gamemode.
            switch (gameMode) {
                case true:
                    prefsManager.WriteToIntPref("SolvedExamplesPercent", dataServer.gameModeScore);
                    break;
                case false:
                    prefsManager.WriteToFloatPref("GameTime", dataServer.gameModeTime);
                    break;
            }

            StartCoroutine(sceneChanger.ChangeSceneAfterDelay("GameOver", 1f));
        }
    }

    //applying force to the player along the y-axis (positive or negative)
    public void AddYForceToPlayer(bool isPositive)
    {
        //matching state
        switch (isPositive) {
            case true:
                rigidBody2D.AddForce(Vector2.up * forcePower); //positive y-axis force
                break;
            case false:
                rigidBody2D.AddForce(Vector2.down * forcePower / FORCE_REDUCTION_FACTOR); //negative y-axis force (with reduction factor)
                break;
        }
    }

    //move wings animation (coroutine)
    public IEnumerator MoveWings()
    {
        audioPlayer.PlayAudio(playerSoundEffects.wingsSound); //playing sound effect
        spriteRenderer.sprite = stateSprites.actionState; //setting state sprite

        yield return new WaitForSeconds(WINGS_MOVE_DURATION); //delay

        spriteRenderer.sprite = stateSprites.normalState; //setting normal state sprite
    }

    //player collision reaction (coroutine)
    private IEnumerator CollisionReaction(float duration)
    {
        spriteRenderer.sprite = stateSprites.confusedState; //setting state sprite

        yield return new WaitForSeconds(duration); //delay

        spriteRenderer.sprite = stateSprites.normalState; //setting normal state sprite
    }
}
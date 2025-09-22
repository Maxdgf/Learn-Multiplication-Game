using UnityEngine;

public class RandomGameBackgroundPicker : MonoBehaviour
{
    [SerializeField] private Sprite[] backgrounds;
    [SerializeField] private float backgroundScaleFactor;

    private void Start()
    {
        System.Random rnd = new System.Random();
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        int random_index = rnd.Next(0, backgrounds.Length - 1);
        spriteRenderer.sprite = backgrounds[random_index];

        float spriteWidth = spriteRenderer.sprite.bounds.size.x;
        float spriteHeight = spriteRenderer.sprite.bounds.size.y;

        Camera camera = Camera.main;
        float screenHeight = backgroundScaleFactor * camera.orthographicSize;
        float screenWidth = screenHeight * camera.aspect;

        transform.localScale = new Vector2(screenWidth / spriteWidth, screenHeight / spriteHeight);
    }
}

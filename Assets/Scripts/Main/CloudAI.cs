using UnityEngine;

public class CloudAI : MonoBehaviour
{
    [SerializeField] private string killTag;
    [SerializeField] private float speed;

    private Renderer objectRenderer;

    void Start()
    {
        objectRenderer = gameObject.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime); //left

        if (!objectRenderer.isVisible)
        {
            Destroy(gameObject); //destroy if object invisible
        }
    }
}

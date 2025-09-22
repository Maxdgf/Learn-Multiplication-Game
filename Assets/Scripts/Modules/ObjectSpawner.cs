using System.Collections;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private float spawnDuration;
    [SerializeField] private int marginY;
    [SerializeField] private GameObject objectToSpawn;

    private System.Random random;
    private float screenHeight;
    private float screenWidth;

    void Start()
    {
        Camera camera = Camera.main;

        random = new System.Random();
        screenHeight = camera.orthographicSize;
        screenWidth = screenHeight * camera.aspect;

        StartCoroutine(SpawnObject(spawnDuration));
    }

    private IEnumerator SpawnObject(float spawnDuration)
    {
        while (true)
        {
            int width = (int) screenWidth;
            int height = (int) screenHeight;

            int rndY = random.Next(0 + marginY, height);

            Vector2 spawnPos = new Vector2(width + width, rndY);

            Instantiate(objectToSpawn, spawnPos, Quaternion.identity);

            yield return new WaitForSeconds(spawnDuration);
        }
    }
}

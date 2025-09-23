/*
==================
| Object spawner |
==================
*/

using System.Collections;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private float spawnDuration; //spawn delay
    [SerializeField] private int marginY; //y bottom margin (to prevent objects from appearing under the UI)
    [SerializeField] private GameObject objectToSpawn; //object to spawn

    private System.Random random;
    private float screenHeight;
    private float screenWidth;

    void Start()
    {
        Camera camera = Camera.main; //getting camera

        random = new System.Random(); //init random
        screenHeight = camera.orthographicSize; //getting screen height (for calculation spawn pos in device screen area)
        screenWidth = screenHeight * camera.aspect; //getting screen width (for calculation spawn pos in device screen area)

        StartCoroutine(SpawnObject(spawnDuration)); //start infinity object spawn coroutine
    }

    //object spawn infinity coroutine
    private IEnumerator SpawnObject(float spawnDuration)
    {
        while (true)
        {
            int width = (int) screenWidth; //screen width - integer
            int height = (int) screenHeight; //screen height - integer

            int rndY = random.Next(0 + marginY, height); //random y in range(y bottom margin - screen height)

            Vector2 spawnPos = new Vector2(width + width, rndY); //configuring spawn pos

            Instantiate(objectToSpawn, spawnPos, Quaternion.identity); //spawn object

            yield return new WaitForSeconds(spawnDuration); //delay
        }
    }
}

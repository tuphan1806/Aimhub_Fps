using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float xRangeMax = -10f, xRangeMin = 14f;
    public float yRangeMax = 8f, yRangeMin = 3f;
    public float zRangeMin = 8f, zRangeMax = 20f;

    GameManager gameManager;
    DifficultyButton difficultyButton;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        transform.position = RandomPosition();
    }

    Vector3 RandomPosition()
    {
        return new Vector3(Random.Range(xRangeMin, xRangeMax), Random.Range(yRangeMin, yRangeMax), Random.Range(zRangeMin, zRangeMax));
    }

}

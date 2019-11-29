using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObstacleSpawner : MonoBehaviour
{
    private const float IncreaseSpeedAmount = 0.5f;
    private const float LevelDuration = 5f;



    public GameObject ObstaclePrefab;
    public float Speed = GameManager.Speed;

    
    private int CurrentLevel = 1;

    private float Spacing = 0.0f;
    private List<GameObject> Obstacles = new List<GameObject>();
    private GameObject LastObstacle;


    private float CurrentTime = 0.0f;


    private void Start()
    {
        GameManager.Spawner = this;
    }

    public void Reset()
    {
        Speed = GameManager.Speed;
        foreach (GameObject obstacle in Obstacles)
            GameObject.Destroy(obstacle);
        Obstacles.Clear();
        LastObstacle = null;
        CurrentLevel = 1;
        Spacing = 0.0f;
        CurrentTime = 0.0f;
    }


    // Update is called once per frame
    void Update()
    {
        if (GameManager.CurrentState != GameManager.GameState.INGAME)
            return;

        CurrentTime += Time.deltaTime;
        List<GameObject> queuedToDestroy = new List<GameObject>();

        float updateDistance = Speed * Time.deltaTime;

        // Move all obstacles
        foreach (GameObject obstacle in Obstacles)
        {
            Vector3 newPosition = obstacle.transform.position - new Vector3(0, 0, updateDistance);

            if (newPosition.z < 0)
            {
                queuedToDestroy.Add(obstacle);
            }
            else
            {
                obstacle.transform.position = newPosition;
            }
        }

        GameManager.Distance += updateDistance;
        if (CurrentTime > (LevelDuration * CurrentLevel))
        {
            CurrentLevel++;
            Speed += IncreaseSpeedAmount;
        }

        
        foreach(GameObject obstacle in queuedToDestroy)
        {
            Obstacles.Remove(obstacle);
            GameObject.Destroy(obstacle);
        }


        // if there aren't any obstacles, or if the last obstacle is beyond the spacing requirement,
        // spawn a new obstacle.
        if (Obstacles.Count == 0 || LastObstacle.transform.position.z < transform.position.z - Spacing)
            SpawnObstacle();
    }


    /**
     * Assumes there are three rows, and one needs to be open
     * for the player to move through.
     */
    private void SpawnObstacle()
    {
        SetNewSpacing();

        // move the spawn point to the left edge of the ground
        float newX = (GameManager.PlatformSize * 0.5f) * -1;
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);

        List<float> positions = GetPositions();

        // Create the obstacles and positon them accordingly
        GameObject container = new GameObject();
        Childify(container, gameObject);
        foreach (float position in positions)
        {
            
            GameObject obstacle = Instantiate(ObstaclePrefab);
            Childify(obstacle, container);

            Vector3 obstaclePostion = obstacle.transform.position;
            obstaclePostion.x += position;
            obstacle.transform.position = obstaclePostion;
        }
         
        LastObstacle = container;
        Obstacles.Add(container);
    }


    private void Childify(GameObject obj, GameObject parent)
    {
        obj.transform.parent = parent.transform;
        obj.transform.position = parent.transform.position;
    }


    private List<float> GetPositions()
    {
        int numberOfSteps = Mathf.FloorToInt(GameManager.PlatformSize);

        // generate posible positions across the ground to place an obstacle
        List<float> positions = new List<float>();
        for (float step = 0.5f; step <= numberOfSteps; step++)
            positions.Add(step);

        int numberToRemove = Random.Range(1, numberOfSteps);
        List<int> indicies = new List<int>();
        for (int i = 0; i < numberToRemove; i++)
        {
            int index = (int)Mathf.Round(Random.value * (positions.Count - i - 1));
            indicies.Add(index);
        }
        for (int i = 0; i < indicies.Count; i++)
            positions.RemoveAt(indicies[i]);

        return new List<float>(positions);
    }

    private void SetNewSpacing()
    {
        Spacing = Random.Range(GameManager.SpawnMin, GameManager.SpawnMax);
    }


}

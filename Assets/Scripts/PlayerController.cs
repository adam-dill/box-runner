using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public GameObject BrokenPrefab;
    public GameObject BrokenObstalcePrefab;
    public float RollingSpeed = 0.2f;

    private GameObject container;
    private bool isRolling = false;

    private GameObject _ground;
    private int NumberOfHits = 0;

    private GameObject Broken;
    private GameObject BrokenObstacle;


    private Vector3 Origin;


    private void Start()
    {
        
        GameManager.Player = this;
        _ground = GameObject.FindGameObjectWithTag("Ground");
        float startingX = (_ground.transform.localScale.x % 2 == 0) ? 0.5f : 0f;
        Origin = new Vector3(startingX, transform.position.y, transform.position.z);
    }

    public void Reset()
    {
        if (Broken != null)
            GameObject.Destroy(Broken);

        if (BrokenObstacle != null)
            GameObject.Destroy(BrokenObstacle);

        isRolling = false;
        NumberOfHits = 0;
        transform.position = Origin;
        gameObject.SetActive(true);
    }


    // Update is called once per frame
    void Update()
    {
        if (GameManager.CurrentState != GameManager.GameState.INGAME)
            return;

        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        int direction = (moveHorizontal < 0) ? -1 :
                        (moveHorizontal > 0) ? 1 :
                        0;

        if (direction != 0 && isRolling == false)
            StartCoroutine(Roll(direction));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Obstacle")
        {
            NumberOfHits++;
            
            if (NumberOfHits >= 1)
            {
                Broken = GameObject.Instantiate(BrokenPrefab);
                Broken.transform.position = transform.position;
                Broken.transform.rotation = transform.rotation;
                Broken.transform.parent = null;

                gameObject.SetActive(false);
                gameObject.transform.rotation = Quaternion.identity;


                // This is what we hit
                BrokenObstacle = GameObject.Instantiate(BrokenObstalcePrefab);
                BrokenObstacle.transform.position = other.gameObject.transform.position;
                BrokenObstacle.transform.rotation = other.gameObject.transform.rotation;
                BrokenObstacle.transform.parent = null;

                other.gameObject.SetActive(false);
                CleanUpRoll();

                // DEAD!
                GameManager.EndGame();
            }
            
        }
    }


    private IEnumerator Roll(int direction)
    {
        // early exit if the player is on the edge.
        if (AbleToMove(direction) == false)
            yield break;

        isRolling = true;

        container = new GameObject("RollingContainer");
        container.transform.position = transform.position + new Vector3(0.5f * direction, -0.5f, 0f);
        transform.parent = container.transform;

        // animated rotation
        Quaternion startRotation = container.transform.rotation;
        Quaternion endRotation = Quaternion.Euler(new Vector3(0f, 0f, 90f*direction*-1)) * startRotation;
        for (float time = 0; time < RollingSpeed; time += Time.deltaTime)
        {
            container.transform.rotation = Quaternion.Lerp(startRotation, endRotation, time / RollingSpeed);
            yield return null;
        }
        container.transform.rotation = endRotation;
        CleanUpRoll();
        isRolling = false;
    }


    private void CleanUpRoll()
    {
        transform.parent = null;
        if (container)
            Destroy(container);
    }


    /**
     * check to see if the 
     */
    private bool AbleToMove(int direction)
    {
        Vector3 side = new Vector3(1f, 0f);
        return Physics.Raycast(transform.position + (side * direction), -Vector3.up);
    }


}

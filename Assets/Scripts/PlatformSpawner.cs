using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{

    public GameObject Prefab;

    /** The distance in units that the platforms move on the Z axis per second. */
    public float Speed = 1f;


    private List<GameObject> CurrentPlatforms = new List<GameObject>();
    private GameObject LastPlatform;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(GameObject platform in CurrentPlatforms)
        {
            Vector3 newPosition = platform.transform.position - new Vector3(0, 0, Speed * Time.deltaTime);
            if (newPosition.z < 0)
            {
                CurrentPlatforms.Remove(platform);
                Destroy(platform);
            }
            else
            {
                platform.transform.position = newPosition;
            }
        }
        
        // check for room to add another platform
        if (CurrentPlatforms.Count < 1 || LastPlatform.transform.position.z < gameObject.transform.position.z - LastPlatform.transform.localScale.z)
            AddPlatform();
    }


    private void AddPlatform()
    {
        GameObject platform = Instantiate(Prefab);
        platform.transform.parent = transform;
        platform.transform.position = transform.position;
        if (LastPlatform != null)
            platform.transform.position = LastPlatform.transform.position + new Vector3(0, 0, LastPlatform.transform.localScale.z * 0.5f);
        CurrentPlatforms.Add(platform);
        LastPlatform = platform;
    }
    
}

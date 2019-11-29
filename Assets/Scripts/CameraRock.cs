using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRock : MonoBehaviour
{
    public Transform Target;
    public float OscillationSpeed = 0.5f;

    private Vector3 OriginalPosition;
    private float OscillationDistance = 3f;

    private Coroutine OscillatingCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        OriginalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (OscillatingCoroutine != null && GameManager.CurrentState == GameManager.GameState.INGAME)
        {
            StopCoroutine(OscillatingCoroutine);
            OscillatingCoroutine = null;
            StartCoroutine(ReturnToOrigin());
        }
        else if (OscillatingCoroutine == null && GameManager.CurrentState != GameManager.GameState.INGAME)
        {
            OscillatingCoroutine = StartCoroutine(Oscillate());
        }
    }

    private IEnumerator Oscillate()
    {
        while (true)
        {
            transform.position = new Vector3(Mathf.Sin(Time.time * OscillationSpeed) * OscillationDistance, transform.position.y, transform.position.z);
            transform.LookAt(Target);
            yield return new WaitForEndOfFrame();
        }
    }


    private IEnumerator ReturnToOrigin()
    {
        float duration = 0.3f;
        float i = 0.0f;
        float rate = 1.0f / duration;
        Vector3 startPosition = transform.position;

        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            transform.position = Vector3.Lerp(startPosition, OriginalPosition, i);
            transform.LookAt(Target);
            yield return null;
        }
    }
}

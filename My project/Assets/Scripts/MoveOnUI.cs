using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnUI : MonoBehaviour
{
    private readonly float lerpDuration = 0.5f;

    IEnumerator coroutine = null;

    [SerializeField] GameObject player;

    bool isRunning;

    public enum RotationDirection
    {
        RIGHT,
        LEFT,
        NONE
    }
    public void RotatePlayerUI(RotationDirection rotation)
    {
        if (!isRunning)
        {
            isRunning = true;
            if (rotation == RotationDirection.NONE)
            {
                if (coroutine != null)
                {
                    StopCoroutine(coroutine);
                }
                coroutine = Lerp(player.transform.rotation.eulerAngles);
                StartCoroutine(coroutine);
            }
            //Debug.Log("I am rotating the cat");
            if (rotation == RotationDirection.RIGHT)
            {
                if (coroutine != null)
                {
                    StopCoroutine(coroutine);
                }
                coroutine = Lerp(player.transform.rotation.eulerAngles + new Vector3(0, 45, 0));
                StartCoroutine(coroutine);
            }
            else if (rotation == RotationDirection.LEFT)
            {
                if (coroutine != null)
                {
                    StopCoroutine(coroutine);
                }
                coroutine = Lerp(player.transform.rotation.eulerAngles + new Vector3(0, -45, 0));
                StartCoroutine(coroutine);
            }
        }  
    }
    IEnumerator Lerp(Vector3 endValue)
    {
        float timeElapsed = 0;
        Vector3 startValue = transform.rotation.eulerAngles;
        while (timeElapsed < lerpDuration)
        {
            transform.rotation = Quaternion.Euler(Vector3.Lerp(startValue, endValue, timeElapsed / lerpDuration));
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.rotation = Quaternion.Euler(endValue);

        coroutine = null;
        isRunning = false;
    }

}

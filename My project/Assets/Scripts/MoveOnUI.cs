using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnUI : MonoBehaviour
{
    private readonly float lerpDuration = 0.5f;

    IEnumerator coroutine = null;
    public void RotatePlayerUI(bool moveRight)
    {
        if(moveRight)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            coroutine = Lerp(new Vector3(0, 45, 0));
            StartCoroutine(coroutine);
        }
        else
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            coroutine = Lerp(new Vector3(0, -45, 0));
            StartCoroutine(coroutine);
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
    }

}

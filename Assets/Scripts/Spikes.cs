using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    bool moveVertical = false;
    bool moveHorizontal = false;

    int start = -240;
    int end = 240;

    float vertCurrent = 0;
    float horCurrent = 0;

    bool vertForward = false;
    bool horForward = false;

    private void Start()
    {
        int vert = Random.Range(0, 2);
        if (vert == 1) moveVertical = true;
        int hor = Random.Range(0, 2);
        if (hor == 1) moveHorizontal = true;
        int vertFwd = Random.Range(0, 2);
        if (vertFwd == 1) vertForward = true;
        int horFwd = Random.Range(0, 2);
        if (horFwd == 1) horForward = true;
    }

    void Update()
    {
        if (vertForward)
        {
            vertCurrent += Time.deltaTime * 300;
            if (moveVertical)
                transform.Translate(0, Time.deltaTime, 0);
            if (vertCurrent >= end) vertForward = false;
        }
        else
        {
            vertCurrent -= Time.deltaTime * 300;
            if (moveVertical)
                transform.Translate(0, -Time.deltaTime, 0);
            if (vertCurrent <= start) vertForward = true;
        }
        if (horForward)
        {
            horCurrent += Time.deltaTime * 300;
            if (moveHorizontal)
                transform.Translate(Time.deltaTime, 0, 0);
            if (horCurrent >= end) horForward = false;
        }
        else
        {
            horCurrent -= Time.deltaTime * 300;
            if (moveHorizontal)
                transform.Translate(-Time.deltaTime, 0, 0);
            if (horCurrent <= start) horForward = true;
        }
    }
}
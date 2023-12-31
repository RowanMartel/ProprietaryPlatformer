using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    bool moveVertical = false;
    bool moveHorizontal = true;

    int start = -480;
    int end = 480;

    float vertCurrent = 0;
    float horCurrent = 0;

    bool vertForward = false;
    bool horForward = false;

    private void Start()
    {
        int vert = Random.Range(0, 2);
        if (vert == 1) moveVertical = true;
        //int hor = Random.Range(0, 2);
        //if (hor == 1) moveHorizontal = true;
        int vertFwd = Random.Range(0, 2);
        if (vertFwd == 1) vertForward = true;
        int horFwd = Random.Range(0, 2);
        if (horFwd == 1) horForward = true;
    }

    void Update()
    {
        if (vertForward)
        {
            vertCurrent += Time.deltaTime * 500;
            if (moveVertical)
                transform.Translate(0, Time.deltaTime, 0);
            if (vertCurrent >= end) vertForward = false;
        }
        else
        {
            vertCurrent -= Time.deltaTime * 500;
            if (moveVertical)
                transform.Translate(0, -Time.deltaTime, 0);
            if (vertCurrent <= start) vertForward = true;
        }
        if (horForward)
        {
            horCurrent += Time.deltaTime * 500;
            if (moveHorizontal)
                transform.Translate(Time.deltaTime, 0, 0);
            if (horCurrent >= end) horForward = false;
        }
        else
        {
            horCurrent -= Time.deltaTime * 500;
            if (moveHorizontal)
                transform.Translate(-Time.deltaTime, 0, 0);
            if (horCurrent <= start) horForward = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            collision.transform.SetParent(transform, true);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            collision.transform.SetParent(null, true);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    TerrainGen terrainGen;

    private void Start()
    {
        terrainGen = FindObjectOfType<TerrainGen>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            terrainGen.Reset();
    }
}
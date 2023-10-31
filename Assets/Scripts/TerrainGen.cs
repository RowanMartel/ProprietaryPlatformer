using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGen : MonoBehaviour
{
    [SerializeField] GameObject groundPrefab;
    [SerializeField] GameObject goalPrefab;
    [SerializeField] GameObject spikesPrefab;

    List<GameObject> groundList = new();
    bool[,] ground = new bool[42, 74];

    int startPosX, startPosY;

    [SerializeField][Range(1, 40)] int groundLevel;
    [SerializeField][Range(0, 5)] int spikesCount;
    [SerializeField][Range(0, 5)] int bumpCount;
    [SerializeField][Range(0, 10)] int bumpVariance;
    [SerializeField][Range(0, 10)] int smoothings;

    List<int[]> bumpList = new();

    private void Start()
    {
        startPosX = 1;
        startPosY = groundLevel;

        Step1GroundGen();
        Step2Bumps();
        Step3SpikePlacement();
        Step4Smoothing();
        Step5GoalPlacement();
        PrefabPlacement();
    }

    void Step1GroundGen()
    {
        for (int i = 0; i < ground.GetLength(0); i++)
        {
            for (int j = 0; j < ground.GetLength(1); j++)
            {
                if (i < groundLevel) ground[i, j] = true;
            }
        }
    }
    void Step2Bumps()
    {
        for (int i = 0; i < bumpCount; i++)
            bumpList.Add(new int[2]);

        int prevHeight = startPosY;
        foreach (var bump in bumpList)
        {
            bump[0] = 74 / bumpCount * (bumpList.IndexOf(bump) + 1);
            bump[1] = Random.Range(Mathf.Clamp(prevHeight - bumpVariance, 1, 40), Mathf.Clamp(prevHeight + bumpVariance + 1, 1, 40));
            prevHeight = bump[1];
        }

        for (int bumpNum = 0; bumpNum < bumpList.Count; bumpNum++)
        {
            float startingX, startingY;
            float endingX, endingY;

            if (bumpNum == 0)
            {
                startingX = startPosX;
                startingY = startPosY;
            }
            else
            {
                startingX = bumpList[bumpNum - 1][0];
                startingY = bumpList[bumpNum - 1][1];
            }
            endingX = bumpList[bumpNum][0];
            endingY = bumpList[bumpNum][1];

            float deltaX = endingX - startingX;
            float deltaY = endingY - startingY;
            float step = deltaY / deltaX;
            float yMod = 0;

            for (int i = (int)startingX; i < startingX + deltaX; i++)
            {
                if (startingY < endingY)
                {
                    if (ground[startPosY + Mathf.RoundToInt(startingY - groundLevel + yMod), startPosX + i])
                        ground[startPosY + Mathf.RoundToInt(startingY - groundLevel + yMod), startPosX + i] = false;
                    else ground[startPosY + Mathf.RoundToInt(startingY - groundLevel + yMod), startPosX + i] = true;
                }
                else
                {
                    if (ground[startPosY + Mathf.RoundToInt(startingY - groundLevel + yMod), startPosX + i])
                            ground[startPosY + Mathf.RoundToInt(startingY - groundLevel + yMod), startPosX + i] = false;
                    else ground[startPosY + Mathf.RoundToInt(startingY - groundLevel + yMod), startPosX + i] = true;
                }
                for (int j = (int)yMod; j < 41 - startPosY; j++)
                {
                    if (ground[Mathf.Clamp(startPosY + Mathf.RoundToInt(startingY - groundLevel + j), 1, 40) + 1, startPosX + i] == false)
                    {
                        if (ground[Mathf.Clamp(startPosY + Mathf.RoundToInt(startingY - groundLevel + j), 1, 40) + 2, startPosX + i] == false)
                            break;
                    }
                    ground[Mathf.Clamp(startPosY + Mathf.RoundToInt(startingY - groundLevel + j), 1, 40), startPosX + i] = false;
                }
                if (startingY + yMod < groundLevel)
                {
                    ground[groundLevel - 1, startPosX + i] = false;
                }
                yMod += step;
            }
        }
    }
    void Step3SpikePlacement()
    {

    }
    void Step4Smoothing()
    {
        for (int smoothCounter = 0; smoothCounter < smoothings;  smoothCounter++)
        {
            for (int i = 0; i < ground.GetLength(0); i++)
            {
                for (int j = 0; j < ground.GetLength(1); j++)
                {
                    if (ground[i, j])
                    {
                        if (i > 0 && i < ground.GetLength(0) - 1 && j > 0 && j < ground.GetLength(1) - 1)
                        {
                            if (!ground[i + 1, j]) ground[i - 1, j] = true;

                            if (!ground[i, j - 1] && !ground[i, j + 1])
                                if (!ground[i + 1, j] || !ground[i - 1, j]) ground[i, j] = false;
                        }
                    }
                    else
                    {
                        if (i > 0 && i < ground.GetLength(0) - 1 && j > 0 && j < ground.GetLength(1) - 1)
                        {
                            if (ground[i, j - 1] && ground[i, j + 1]) ground[i, j] = true;

                            if (ground[i + 1, j] && ground[i - 1, j]) ground[i, j] = true;
                        }
                    }
                }
            }
        }
    }
    void Step5GoalPlacement()
    {
        
    }

    void PrefabPlacement()
    {
        for (float i = 0; i < ground.GetLength(0); i++)
        {
            for (float j = 0; j < ground.GetLength(1); j++)
            {
                if (ground[(int)i, (int)j])
                {
                    GameObject newTile = Instantiate(groundPrefab);
                    newTile.transform.position = new Vector2(-9 + j / 4, -5 + i / 4);
                    groundList.Add(newTile);
                }
            }
        }
    }
}
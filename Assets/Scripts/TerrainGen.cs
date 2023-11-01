using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGen : MonoBehaviour
{
    [SerializeField] GameObject groundPrefab;
    [SerializeField] GameObject goalPrefab;
    [SerializeField] GameObject spikesPrefab;

    private List<GameObject> groundList = new();
    private Tile[,] ground = new Tile[42, 74];

    private List<GameObject> spikesList = new();

    private int startPosX, startPosY;

    [SerializeField][Range(1, 40)] public int groundLevel;
    [SerializeField][Range(0, 15)] int spikesCount;
    [SerializeField][Range(0, 10)] int spikesPosVariance;
    [SerializeField][Range(0, 5)] int bumpCount;
    [SerializeField][Range(0, 10)] int bumpVariance;
    [SerializeField][Range(0, 5)] int pitCount;
    [SerializeField][Range(0, 10)] int pitRadius;
    [SerializeField][Range(0, 5)] int pitRadiusVariance;
    [SerializeField][Range(0, 10)] int smoothings;

    private List<int[]> bumpList = new();
    private List<int> pitXPositions = new();

    private void Start()
    {
        Reset();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            Reset();
    }

    private void Reset()
    {
        foreach(GameObject tile in groundList)
            Destroy(tile);
        foreach (GameObject spikes in spikesList)
            Destroy(spikes);
        groundList = new();
        spikesList = new();
        bumpList = new();
        pitXPositions = new();

        for(int i = 0; i < ground.GetLength(0); i++)
        {
            for (int j = 0; j < ground.GetLength(1); j++)
                ground[i, j] = new();
        }

        startPosX = 1;
        startPosY = groundLevel;

        Step1GroundGen();
        Step2Bumps();
        Step3Pits();
        Step4SpikePlacement();
        Step5Smoothing();
        Step6GoalPlacement();
        PrefabPlacement();

        FindObjectOfType<Player>().Reset();
    }

    void Step1GroundGen()
    {
        for (int i = 0; i < ground.GetLength(0); i++)
        {
            for (int j = 0; j < ground.GetLength(1); j++)
                if (i < groundLevel) ground[i, j].active = true;
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
                    if (ground[startPosY + Mathf.RoundToInt(startingY - groundLevel + yMod), startPosX + i].active)
                        ground[startPosY + Mathf.RoundToInt(startingY - groundLevel + yMod), startPosX + i].active = false;
                    else ground[startPosY + Mathf.RoundToInt(startingY - groundLevel + yMod), startPosX + i].active = true;
                }
                else
                {
                    if (ground[startPosY + Mathf.RoundToInt(startingY - groundLevel + yMod), startPosX + i].active)
                            ground[startPosY + Mathf.RoundToInt(startingY - groundLevel + yMod), startPosX + i].active = false;
                    else ground[startPosY + Mathf.RoundToInt(startingY - groundLevel + yMod), startPosX + i].active = true;
                }
                for (int j = (int)yMod; j < 41 - startPosY; j++)
                {
                    if (ground[Mathf.Clamp(startPosY + Mathf.RoundToInt(startingY - groundLevel + j), 1, 40) + 1, startPosX + i].active == false)
                    {
                        if (ground[Mathf.Clamp(startPosY + Mathf.RoundToInt(startingY - groundLevel + j), 1, 40) + 2, startPosX + i].active == false)
                            break;
                    }
                    ground[Mathf.Clamp(startPosY + Mathf.RoundToInt(startingY - groundLevel + j), 1, 40), startPosX + i].active = false;
                }
                if (startingY + yMod < groundLevel)
                {
                    ground[groundLevel - 1, startPosX + i].active = false;
                }
                yMod += step;
            }
        }
    }
    void Step3Pits()
    {
        if (pitRadiusVariance >= pitRadius)
            pitRadiusVariance = pitRadius - 1;

        for (int i = 0; i < pitCount; i++)
            pitXPositions.Add(0);
        for (int i = 0; i < pitXPositions.Count; i++)
        {
            int pit = 74 / pitCount * (i + 1);
            pitXPositions[i] = pit;
        }
        foreach (int pit in pitXPositions)
        {
            int startX = Mathf.Clamp(pit - Random.Range(pitRadius - pitRadiusVariance, pitRadius + pitRadiusVariance), 3, 70);
            int endX = Mathf.Clamp(pit + Random.Range(pitRadius - pitRadiusVariance, pitRadius + pitRadiusVariance), 3, 70);

            for (int i = startX; i < endX; i++)
            {
                for (int j = 0; j < 42;  j++)
                    ground[j, i].active = false;
            }
        }
    }
    void Step4SpikePlacement()
    {
        for (int i = 0; i < spikesCount; i++)
            spikesList.Add(Instantiate(spikesPrefab));

        foreach (GameObject spikes in spikesList)
        {
            float xInd = 0;
            float yInd = 0;
            xInd = 60 / spikesCount * spikesList.IndexOf(spikes) + 10;
            for (int i = 0; i < ground.GetLength(0); i++)
            {
                float yMod = Random.Range(-spikesPosVariance, spikesPosVariance);
                if (ground[i, (int)xInd].active)
                    yInd = i + yMod;
            }
            xInd += Random.Range(-spikesPosVariance, spikesPosVariance);
            if (yInd == 0)
            {
                yInd = Random.Range(0, spikesPosVariance);
            }
            spikes.transform.position = new Vector2(xInd / 4 - 9, yInd / 4 - 5);
        }
    }
    void Step5Smoothing()
    {
        for (int smoothCounter = 0; smoothCounter < smoothings;  smoothCounter++)
        {
            for (int i = 0; i < ground.GetLength(0); i++)
            {
                for (int j = 0; j < ground.GetLength(1); j++)
                {
                    if (i > 0 && i < ground.GetLength(0) - 1 && j > 0 && j < ground.GetLength(1) - 1)
                    {
                        Tile currentTile = ground[i, j];
                        currentTile.leftTile = ground[i, j - 1];
                        currentTile.rightTile = ground[i, j + 1];
                        currentTile.bottomTile = ground[i - 1, j];
                        currentTile.topTile = ground[i + 1, j];

                        if (!currentTile.active)
                        {
                            if (currentTile.leftTile.active && currentTile.rightTile.active)
                                currentTile.active = true;

                            if (currentTile.topTile.active && currentTile.bottomTile.active)
                                currentTile.active = true;

                            if (currentTile.bottomTile.active && currentTile.topTile.active && currentTile.leftTile.active && currentTile.rightTile.active)
                                currentTile.active = true;
                            
                        }
                        else
                        {
                            if (!currentTile.topTile.active)
                                currentTile.bottomTile.active = true;

                            if (!currentTile.leftTile.active && !currentTile.rightTile.active)
                            {
                                if (!currentTile.topTile.active || !currentTile.bottomTile.active)
                                    currentTile.active = false;
                            }

                            if (!currentTile.bottomTile.active && !currentTile.topTile.active && !currentTile.leftTile.active && !currentTile.rightTile.active)
                                currentTile.active = false;
                        }
                    }
                    
                }
            }
        }
    }
    void Step6GoalPlacement()
    {
        
    }

    void PrefabPlacement()
    {
        for (float i = 0; i < ground.GetLength(0); i++)
        {
            for (float j = 0; j < ground.GetLength(1); j++)
            {
                if (ground[(int)i, (int)j].active)
                {
                    GameObject newTile = Instantiate(groundPrefab);
                    newTile.transform.position = new Vector2(-9 + j / 4, -5 + i / 4);
                    groundList.Add(newTile);
                }
            }
        }
    }
}

class Tile
{
    public bool active;
    public Tile leftTile;
    public Tile rightTile;
    public Tile topTile;
    public Tile bottomTile;

    public Tile()
    {
        active = false;
    }
}
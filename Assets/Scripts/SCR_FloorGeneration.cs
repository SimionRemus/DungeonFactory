using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SCR_FloorGeneration : MonoBehaviour
{
    public TileWithAttributes[] tilePrefabs;
    public TileWithAttributes wallPrefab;
    public Tilemap groundTilemap;
    public Tilemap wallTilemap;
    public GameObject player;
    [SerializeField] private GameObject manager;
    [SerializeField] private GameObject eventContainer;
    [SerializeField] private GameObject npcContainer;
    public int gameSeed;

    private int cellSize = 5;
    public int rows; //each cell is cellSize x cellSize so multiply this for floor generation
    public int cols; //each cell is cellSize x cellSize so multiply this for floor generation


    public int numberOfFloorSeeds;
    
    List<Vector3> seeds = new List<Vector3>();
    List<int> tilePrefabIndex = new List<int>();

    private int[,] CaveGrid;
    public int InitialFill = 60;

    List<Vector3> doorPositions = new List<Vector3>();
    private int ClosedThreshold = 63;

    private float NPCThreshold = 0.7f;
    private float EventThreshold = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        if(gameSeed!=0)
        {
            Random.InitState(gameSeed);
        }
        wallPrefab.colliderType = Tile.ColliderType.Sprite;
        createSeedTiles();
        GenerateMap();
        GenerateRooms();
        EventSpawner();
        NPCSpawner();
    }

    #region groundGenerations
    public void GenerateMap()
    {
        for (int row = 0; row < (rows* (cellSize + 1) + 1); row++)
        {
            for (int col = 0; col < (cols*(cellSize+1) +1); col++)
            {
                Vector3 point = new Vector3(row, col, 0);
                if(!seeds.Contains(point))
                {
                    int closestPointIndex = FindClosestPoint(point);
                    groundTilemap.SetTile(groundTilemap.WorldToCell(point), tilePrefabs[tilePrefabIndex[closestPointIndex]]);
                }
            }
        }
    }

    private void createSeedTiles()
    {
        for (int i = 0; i < numberOfFloorSeeds; i++)
        {
            Vector3 randomPosition = new Vector3(Random.Range(0, rows * (cellSize + 1) + 1), Random.Range(0, cols * (cellSize + 1) + 1), 0);
            seeds.Add(randomPosition);
            int randomTileNumber = Random.Range(0, tilePrefabs.Length);
            tilePrefabIndex.Add(randomTileNumber);
            groundTilemap.SetTile(groundTilemap.WorldToCell(randomPosition), tilePrefabs[randomTileNumber]);
        }
    }

    private int FindClosestPoint(Vector3 point)
    {
        int closestPointIndex = 0;
        var distance = Vector3.Distance(point, seeds[0]);
        for (int i = 0; i < seeds.Count; i++)
        {
            var tempDist = Vector3.Distance(point, seeds[i]);
            if(tempDist<distance)
            {
                distance = tempDist;
                closestPointIndex = i;
            }
        }
        return closestPointIndex;
    }
    #endregion

    #region Room System

    public void GenerateRooms()
    {
        for (int row = 0; row < (rows * (cellSize + 1) + 1); row++)
        {
            for (int col = 0; col < (cols * (cellSize + 1) + 1); col++)
            {
                if ((row % (cellSize + 1) == 0) || (col % (cellSize + 1) == 0))
                {
                    if ((row % (cellSize + 1) != (cellSize + 1) / 2) && (col % (cellSize + 1) != (cellSize + 1) / 2))
                    {
                        Vector3 point = new Vector3(row, col, 0);
                        wallTilemap.SetTile(wallTilemap.WorldToCell(point), wallPrefab);
                    }
                    else
                    {
                        if ((row != 0) && (col != 0) && (row != rows * (cellSize + 1)) && (col != cols * (cellSize + 1)))
                        {
                            doorPositions.Add(new Vector3(row, col, 0)); //save these points for DOORS!!!
                        }

                    }
                }
                if ((row == 0) || (col == 0) || (row == rows * (cellSize + 1)) || (col == cols * (cellSize + 1)))
                {
                    Vector3 point = new Vector3(row, col, 0);
                    wallTilemap.SetTile(wallTilemap.WorldToCell(point), wallPrefab);
                }

            }
        }
    }

    public void ToggleDoors()
    
    {
        foreach (Vector3 door in doorPositions)
        {
            int number = Random.Range(0, 100);
            if (number > (100 - ClosedThreshold))
            {
                wallTilemap.SetTile(wallTilemap.WorldToCell(door), wallPrefab);
            }
            else
            {
                wallTilemap.SetTile(wallTilemap.WorldToCell(door), null);
            }
        }
    }
    #endregion

    #region NPCs&Events

    private void NPCSpawner()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                int[] elemWeights = new int[7];
                List<GameObject> filter1 = new List<GameObject>();
                List<GameObject> filter2 = new List<GameObject>();

                if (Random.Range(0f, 1f) <= NPCThreshold && !(col==0 && row==0) && !(col == cols-1 && row == rows-1))
                {
                    bool isPositionTaken = true;
                    //spawn random Event on random position within room
                    while (isPositionTaken)
                    {
                        Vector3 defaultposition= new Vector3(row * (cellSize + 1) + 1.5f, col * (cellSize + 1) + 1.5f, 0);
                        Vector3 position = new Vector3(row * (cellSize + 1) + 1.5f + Random.Range(0, cellSize), col * (cellSize + 1) + 1.5f + Random.Range(0, cellSize), 0);
                        Collider2D collider = Physics2D.OverlapCircle(position, 0.45f);
                        if (!collider)
                        {
                            isPositionTaken = false;
                            List<GameObject> npcs = manager.GetComponent<SCR_ObjectLists>().NPCs;
                            for (int i = 0; i < 5; i++)
                            {
                                for (int j = 0; j < 5; j++)
                                {
                                    TileWithAttributes tile = (TileWithAttributes)groundTilemap.GetTile(new Vector3Int((int)(i+defaultposition.x),(int)(j+defaultposition.y), 0));
                                    if(tile!=null)
                                        switch (tile.element)
                                    {
                                        case elementType.None:
                                            break;
                                        case elementType.Earth:
                                            elemWeights[0]++;
                                            break;
                                        case elementType.Water:
                                            elemWeights[1]++;
                                            break;
                                        case elementType.Fire:
                                            elemWeights[2]++;
                                            break;
                                        case elementType.Air:
                                            elemWeights[3]++;
                                            break;
                                        case elementType.Divination:
                                            elemWeights[4]++;
                                            break;
                                        case elementType.Illusion:
                                            elemWeights[5]++;
                                            break;
                                        case elementType.Life:
                                            elemWeights[6]++;
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                            elementType npcElement =NPCWeightedRandomElement(elemWeights);
                            //Filter NPCs by element type (+none)
                            foreach (GameObject npc in npcs)
                            {
                                if (npc.transform.GetComponent<SCR_NPC>().npcData.elementType==elementType.None || npc.transform.GetComponent<SCR_NPC>().npcData.elementType == npcElement)
                                {
                                    filter1.Add(npc);
                                }
                            }
                            //Filter NPCs by difficulty
                            foreach (GameObject npc in filter1)
                            {
                                switch (row+col)
                                {
                                    case int n when n < (cols+rows)/3:
                                        if (npc.transform.GetComponent<SCR_NPC>().npcData.difficulty == difficultyType.Easy)
                                            filter2.Add(npc);
                                        break;
                                    case int n when n < (cols + rows) * 2 / 3:
                                        if (npc.transform.GetComponent<SCR_NPC>().npcData.difficulty == difficultyType.Medium)
                                            filter2.Add(npc);
                                        break;
                                    default:
                                        if (npc.transform.GetComponent<SCR_NPC>().npcData.difficulty == difficultyType.Hard)
                                            filter2.Add(npc);
                                        break;
                                }
                            }
                            if (filter2.Count != 0)
                            {
                                GameObject NPC = filter2[Random.Range(0, filter2.Count)];
                                GameObject thisNPC = GameObject.Instantiate(NPC, position, Quaternion.identity);
                                thisNPC.transform.SetParent(npcContainer.transform);
                                thisNPC.name = NPC.name;
                            }
                        }
                    }
                }
            }
        }
    }

    private void EventSpawner()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                if (Random.Range(0f, 1f) <= EventThreshold && !(col == 0 && row == 0) && !(col == cols - 1 && row == rows - 1))
                {
                    bool isPositionTaken = true;
                    //spawn random Event on random position within room
                    while (isPositionTaken)
                    {
                        Vector3 position = new Vector3(row * (cellSize + 1) + 1.5f + Random.Range(0, cellSize), col * (cellSize + 1) + 1.5f + Random.Range(0, cellSize), 0);
                        Collider2D collider = Physics2D.OverlapCircle(position, 0.45f);
                        if (!collider)
                        {
                            isPositionTaken = false;
                            List<GameObject> events = manager.GetComponent<SCR_ObjectLists>().events;
                            int index = EventsWeightedRandomIndex(events);
                            GameObject eventType = null;
                            if (index != -1)
                            {
                                eventType = events[index];
                            }
                            else
                            {
                                eventType = events[0];
                            }
                            GameObject thisEvent = GameObject.Instantiate(eventType, position, Quaternion.identity);
                            thisEvent.transform.SetParent(eventContainer.transform);
                            thisEvent.name = eventType.name;
                        }
                    }

                    
                }
            }
        }
    }

    private void BBEGSpawner()
    {
        //TO BE DEFINED
    }

    private int EventsWeightedRandomIndex(List<GameObject> events)
    {
        int weightSum = 0;
        for (int i = 0; i < events.Count; i++)
        {
            SO_Events eventSO = events[i].GetComponent<SCR_Events>().thisEvent;
            weightSum += eventSO.probabilityWeight;
        }
        float rand = Random.value;
        float s = 0f;
        for (int i = 0; i < events.Count; i++)
        {
            SO_Events eventSO = events[i].GetComponent<SCR_Events>().thisEvent;
            s += eventSO.probabilityWeight / (float)weightSum;
            if (s >= rand)
            {
                return i;
            }
        }
        return -1;
    }

    private elementType NPCWeightedRandomElement(int[] elemWeights)
    {
        int weightSum = 0;
        for (int i = 0; i < elemWeights.Length; i++)
        {
            weightSum = elemWeights[i];
        }
        float rand = Random.value;
        float s = 0f;
        for (int i = 0; i < elemWeights.Length; i++)
        {
            s += elemWeights[i] / (float)weightSum;
            if (s >= rand)
            {
                switch (i)
                {
                    case 0:
                        return elementType.Earth;
                    case 1:
                        return elementType.Water;
                    case 2:
                        return elementType.Fire;
                    case 3:
                        return elementType.Air;
                    case 4:
                        return elementType.Divination;
                    case 5:
                        return elementType.Illusion;
                    case 6:
                        return elementType.Life;
                    default:
                        break;
                }
            }
        }
        return elementType.None;
    }
    #endregion

    #region Cave System
    void CaveInit()
    {
        int isTileSet = 0;
        CaveGrid = new int[(rows * (cellSize + 1) + 1), (cols * (cellSize + 1) + 1)];

        //Create initial random map with closed perimeter
        for (int i = 0; i < (rows * (cellSize + 1) + 1); i++)
        {
            for (int j = 0; j < (cols * (cellSize + 1) + 1); j++)
            {
                if((i==0) || (j==0) || (i== (rows * (cellSize + 1))) || (j== (cols * (cellSize + 1))))
                {
                    Vector3 point = new Vector3(i, j, 0);
                    wallTilemap.SetTile(wallTilemap.WorldToCell(point), wallPrefab);
                    CaveGrid[i,j] = 1;
                }
                else
                {
                    isTileSet = Random.Range(0, 100);
                    if(isTileSet>=100-InitialFill)
                    {
                        Vector3 point = new Vector3(i, j, 0);
                        wallTilemap.SetTile(wallTilemap.WorldToCell(point), wallPrefab);
                        CaveGrid[i,j] = 1;
                    }
                    else
                    {
                        Vector3 point = new Vector3(i, j, 0);
                        wallTilemap.SetTile(wallTilemap.WorldToCell(point), null);
                        CaveGrid[i,j] = 0;
                    }
                }
            }
        }
    }

    void CaveUpdate(int CAType)
    {
        int[,] CaveGridLocal = CaveGrid;
        for (int i = 0; i < (rows * (cellSize + 1)+1); i++) 
        {
            for (int j = 0; j < (cols * (cellSize + 1)+1); j++)
            {
                int neighbourCount = 0;
                for (int x = i-1; x < i+2; x++)
                {
                    for (int y = j - 1; y < j + 2; y++)
                    {
                        if (((i - 1) < 0) || ((j - 1) < 0) || ((i + 1) > (rows * (cellSize + 1))) || ((j + 1) > (cols * (cellSize + 1))))
                        {
                            neighbourCount++;
                        }
                        else
                        {
                            if(x!=i || j!=y)
                            {
                                if (CaveGridLocal[x,y] == 1)
                                {
                                    neighbourCount++;
                                }
                            }
                        }
                    }
                }
                if (CAType == 0)
                {
                    if (neighbourCount > 4)
                    {
                        Vector3 point = new Vector3(i, j, 0);
                        wallTilemap.SetTile(wallTilemap.WorldToCell(point), wallPrefab);
                        CaveGrid[i, j] = 1;
                    }
                    else
                    {
                        Vector3 point = new Vector3(i, j, 0);
                        wallTilemap.SetTile(wallTilemap.WorldToCell(point), null);
                        CaveGrid[i, j] = 0;
                    }
                }
                else if (CAType == 1)
                {
                    if (CaveGridLocal[i, j] == 1 && (neighbourCount >= 2 || neighbourCount <= 3))
                    {
                        Vector3 point = new Vector3(i, j, 0);
                        wallTilemap.SetTile(wallTilemap.WorldToCell(point), wallPrefab);
                        CaveGrid[i, j] = 1;
                    }

                    else if (CaveGridLocal[i, j] == 0 && neighbourCount == 3)
                    {
                        Vector3 point = new Vector3(i, j, 0);
                        wallTilemap.SetTile(wallTilemap.WorldToCell(point), wallPrefab);
                        CaveGrid[i, j] = 1;
                    }

                    else
                    {
                        Vector3 point = new Vector3(i, j, 0);
                        wallTilemap.SetTile(wallTilemap.WorldToCell(point), null);
                        CaveGrid[i, j] = 0;
                    }
                }
            }
        }
    }
    #endregion
}

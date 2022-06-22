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
    private int ClosedThreshold = 33;


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
        //CaveInit();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToggleDoors();
            //CaveUpdate(0);
        }
    }

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

    private void ToggleDoors()
    
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

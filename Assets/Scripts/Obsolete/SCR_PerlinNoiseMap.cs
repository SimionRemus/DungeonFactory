using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_PerlinNoiseMap : MonoBehaviour
{
    Dictionary<int, GameObject> tileset;
    Dictionary<int, GameObject> tile_groups;
    
    public GameObject prefab_lava;
    public GameObject prefab_water;
    public GameObject prefab_grass;
    public GameObject prefab_snow;

    float magnification = 7f;
    int xOffset = 0;
    int yOffset = 0;

    int map_width = 16;
    int map_height = 9;

    List<List<int>> noise_grid = new List<List<int>>();
    List<List<GameObject>> tile_grid = new List<List<GameObject>>();

    void Start()
    {
        CreateTileset();
        CreateTileGroups();
        GenerateMap();
    }

    void CreateTileset()
    {
        tileset = new Dictionary<int, GameObject>();
        tileset.Add(0, prefab_lava);
        tileset.Add(1, prefab_water);
        tileset.Add(2, prefab_grass);
        tileset.Add(3, prefab_snow);
    }

    void CreateTileGroups()
    {
        tile_groups = new Dictionary<int, GameObject>();
        foreach(KeyValuePair<int,GameObject> prefab_pair in tileset)
        {
            GameObject tile_group = new GameObject(prefab_pair.Value.name);
            tile_group.transform.parent = gameObject.transform;
            tile_group.transform.localPosition = new Vector3(0, 0, 0);
            tile_groups.Add(prefab_pair.Key, tile_group);
        }
    }

    void GenerateMap()
    {
        for(int x=0;x<map_width;x++)
        {
            noise_grid.Add(new List<int>());
            tile_grid.Add(new List<GameObject>());

            for(int y=0;y<map_height;y++)
            {
                int tile_id = GetIdUsingPerlin(x, y);
                noise_grid[x].Add(tile_id);
                CreateTileGroups(tile_id, x, y);
            }
        }
    }

    int GetIdUsingPerlin(int x, int y)
    {
        float raw_perlin = Mathf.PerlinNoise((x - xOffset) / magnification, (y - yOffset) / magnification);
        float clamped_perlin = Mathf.Clamp(raw_perlin, 0f, 1f);
        float scaled_perlin = clamped_perlin * tileset.Count;
        if(scaled_perlin == 4)
        {
            scaled_perlin = 3;
        }
        return Mathf.FloorToInt(scaled_perlin);
    }

    void CreateTileGroups(int tile_id, int x, int y)
    {
        GameObject tile_prefab = tileset[tile_id];
        GameObject tile_group = tile_groups[tile_id];
        GameObject tile = Instantiate(tile_prefab, tile_group.transform);

        tile.name = string.Format("tile_X:{0}_Y:{1}", x, y);
        tile.transform.localPosition = new Vector3(x, y, 0);
        tile_grid[x].Add(tile);
    }

}

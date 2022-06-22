using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TileWithAttributes : Tile
{
    public string tileName;
    public int tileIndex;


#if UNITY_EDITOR
    [MenuItem("Assets/Create/2D/Custom Tiles/Tile With Attributes")]
    public static void CreateTileWithAttributes()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Tile with Attributes","New Tile with Attributes","Asset", "Save Tile with Attributes","Assets");
        if (path == null) return;
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<TileWithAttributes>(), path);
    }
#endif
}

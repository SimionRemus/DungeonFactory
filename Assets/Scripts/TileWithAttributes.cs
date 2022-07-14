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
    public elementType element;
    public tileEffect tileEffect;


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

public enum tileEffect
{
    None,
    waterHalfEffect,
    halfHealEffect,
    healFiveIfPassed,
    healToDamage,
    useTorchIfPassed,
    fireHalfEffect,
    doubleWaterDamage,
    doubleFireDamage,
    doubleAirDamage,
    doubleEarthDamage,
    dmgFiveIfPassed,
    dmgToHeal,
    noTorchUsed,
    halfSpeed,
    doubleSpeed,
    createWall,
    halfExtraDmg,
    dmgTenOnEOT,
    lifeTwice,
    lifeCancel,
    healTenOnEOT
}

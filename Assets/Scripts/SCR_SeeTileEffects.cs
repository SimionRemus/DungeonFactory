using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SCR_SeeTileEffects : MonoBehaviour
{
    [SerializeField] private GameObject tileEffectContainer;
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private Text tileEffectDescriptionText;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1) && (!EventSystem.current.IsPointerOverGameObject()))
        {

            for (int i = 0; i < tileEffectContainer.transform.childCount; i++)
            {
                Vector3 position = groundTilemap.WorldToCell(tileEffectContainer.transform.GetChild(i).position) + new Vector3(0.5f, 0.5f);
                Vector3 mousePosition = groundTilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition)) + new Vector3(0.5f, 0.5f);
                if (position == mousePosition)
                {
                    TileWithAttributes tile = (TileWithAttributes)groundTilemap.GetTile(groundTilemap.WorldToCell(tileEffectContainer.transform.GetChild(i).position));
                    tileEffectDescriptionText.text = tile.tileEffect.ToString();
                    tileEffectDescriptionText.transform.parent.gameObject.SetActive(true);
                }
                else
                {
                    tileEffectDescriptionText.transform.parent.gameObject.SetActive(false);
                }
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            tileEffectDescriptionText.transform.parent.gameObject.SetActive(false);
        }
    }
}

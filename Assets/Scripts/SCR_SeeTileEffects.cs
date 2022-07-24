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
                    switch (tile.tileEffect)
                    {
                        case tileEffect.None:
                            break;
                        case tileEffect.waterHalfEffect:
                            tileEffectDescriptionText.text = "Any creature standing on this tile will receive half the damage or heal from water spells this turn.";
                            break;
                        case tileEffect.halfHealEffect:
                            tileEffectDescriptionText.text = "Any creature standing on this tile will receive half the damage or heal from fire spells this turn.";
                            break;
                        case tileEffect.healFiveIfPassed:
                            tileEffectDescriptionText.text = "Any creature passing this tile will be healed by 5.";
                            break;
                        case tileEffect.healToDamage:
                            tileEffectDescriptionText.text = "Any creature standing on this tile will receive damage instead of heal this turn.";
                            break;
                        case tileEffect.useTorchIfPassed:
                            tileEffectDescriptionText.text = "Any creature passing this tile will lose a torch.";
                            break;
                        case tileEffect.fireHalfEffect:
                            tileEffectDescriptionText.text = "Any creature standing on this tile will receive half the damage or heal from fire spells this turn.";
                            break;
                        case tileEffect.extraFiftyPercentDamage:
                            tileEffectDescriptionText.text = "Any creature standing on this tile will receive 150% damage this turn.";
                            break;
                        case tileEffect.doubleWaterDamage:
                            tileEffectDescriptionText.text = "Any creature standing on this tile will receive 200% damage from water spells this turn.";
                            break;
                        case tileEffect.doubleFireDamage:
                            tileEffectDescriptionText.text = "Any creature standing on this tile will receive 200% damage from fire spells this turn.";
                            break;
                        case tileEffect.doubleAirDamage:
                            tileEffectDescriptionText.text = "Any creature standing on this tile will receive 200% damage from air spells this turn.";
                            break;
                        case tileEffect.doubleEarthDamage:
                            tileEffectDescriptionText.text = "Any creature standing on this tile will receive 200% damage from earth spells this turn.";
                            break;
                        case tileEffect.doubleOrNothingEarthDamage:
                            tileEffectDescriptionText.text = "Any creature standing on this tile will receive either 200% or 0% damage from earth spells this turn.";
                            break;
                        case tileEffect.doubleOrNothingWaterDamage:
                            tileEffectDescriptionText.text = "Any creature standing on this tile will receive either 200% or 0% damage from water spells this turn.";
                            break;
                        case tileEffect.doubleOrNothingFireDamage:
                            tileEffectDescriptionText.text = "Any creature standing on this tile will receive either 200% or 0% damage from fire spells this turn.";
                            break;
                        case tileEffect.doubleOrNothingAirDamage:
                            tileEffectDescriptionText.text = "Any creature standing on this tile will receive either 200% or 0% damage from air spells this turn.";
                            break;
                        case tileEffect.dmgNeighboursFive:
                            tileEffectDescriptionText.text = "Any creature standing adjacent to this tile will receive 5 damage this turn.";
                            break;
                        case tileEffect.dmgFiveIfPassed:
                            tileEffectDescriptionText.text = "Any creature passing this tile will receive 5 damage this turn.";
                            break;
                        case tileEffect.dmgToHeal:
                            tileEffectDescriptionText.text = "Any creature standing on this tile will receive healing instead of damage this turn.";
                            break;
                        case tileEffect.noBonusOnIllusion:
                            tileEffectDescriptionText.text = "Any creature standing on this tile will receive no bonus from optional elements when casting Illusion Spells this turn.";
                            break;
                        case tileEffect.noTorchUsed:
                            tileEffectDescriptionText.text = "Any creature standing in the same room as this tile will use no torches this turn.";
                            break;
                        case tileEffect.halfSpeed:
                            tileEffectDescriptionText.text = "Any creature passing this tile will move 50% slower on it this turn. The range of movement is reduced by 1";
                            break;
                        case tileEffect.doubleSpeed:
                            tileEffectDescriptionText.text = "Any creature passing this tile will move for free on it this turn.The range of movement is increased by 1";
                            break;
                        case tileEffect.createWall:
                            tileEffectDescriptionText.text = "A wall will be created here for a turn. If there is a creature, it will receive 10 damage instead.";
                            break;
                        case tileEffect.dmgTenOnEOT:
                            tileEffectDescriptionText.text = "Any creature stopping on this tile upon end of turn will receive 10 damage.";
                            break;
                        case tileEffect.lifeTwice:
                            tileEffectDescriptionText.text = "Any creature standing on this tile will cast any Life Spells twice this turn..";
                            break;
                        case tileEffect.lifeCancel:
                            tileEffectDescriptionText.text = "Any creature standing on this tile will not be able to cast Life Spells this turn. Willpower will still be consumed!";
                            break;
                        case tileEffect.healTenOnEOT:
                            tileEffectDescriptionText.text = "Any creature stopping on this tile upon end of turn will heal 10 damage.";
                            break;
                    }
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

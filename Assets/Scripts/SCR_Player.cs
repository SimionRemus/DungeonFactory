using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCR_Player : MonoBehaviour
{
    #region movement attributes
    public Transform movePoint;
    public float moveSpeed = 5f;
    public GameObject cam;
    public LayerMask whatStopsMovement;
    private float prevDirection = 1f;
    #endregion
    #region  Player Resources
    public PlayerClasses playerClass;
    public elementType[] infusionslots;
    public int numberOfTorches;
    public int numberOfWillpower;
    public int numberOfHitpoints;
    public int baseNumberOfWillpower;
    public int numberOfTorchesModifier;
    public int numberOfWillpowerModifier;
    public int numberOfHitpointsModifier;
    [SerializeField]
    private Text numberOfTorchesText;
    [SerializeField]
    private Text numberOfWillPowerText;
    [SerializeField]
    private Text numberOfHitpointsText;
    #endregion
    #region UI stuff
    [SerializeField] Image[] Elements;
    [SerializeField] Sprite[] elementSprites;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        movePoint.parent = null;
        infusionslots = new elementType[7];
        numberOfHitpoints = 100;
        numberOfTorches = 30;
        numberOfWillpower = 3;
        baseNumberOfWillpower = 3;
        numberOfHitpointsModifier = 0;
        numberOfTorchesModifier = 0;
        numberOfWillpowerModifier = 0;
    }

    // Update is called once per frame
    void Update()
    {

        SetCamera();
        UpdateUI();
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
        if(Vector3.Distance(transform.position,movePoint.position)<=0.05f)
        {
            DebugWASDMovement();
        }
    }

    public void SetCamera()
    {

        cam.transform.position = this.transform.position + new Vector3(0, -2, -10);
    }

    public void GetNewInfusionElement(elementType element)
    {
        switch (element)
        {
            case elementType.None:
                Debug.Log("This should not have happened!");
                break;
            case elementType.Earth:
                infusionslots[0] = element;
                break;
            case elementType.Water:
                infusionslots[1] = element;
                break;
            case elementType.Fire:
                infusionslots[2] = element;
                break;
            case elementType.Air:
                infusionslots[3] = element;
                break;
            case elementType.Divination:
                infusionslots[4] = element;
                break;
            case elementType.Illusion:
                infusionslots[5] = element;
                break;
            case elementType.Life:
                infusionslots[6] = element;
                break;
        }
        UpdateElementsUI();
    }

    public void RotateSlots()
    {
        elementType auxElem = infusionslots[6];
        for (int i = 5; i >=0; i--)
        {
            infusionslots[i+1] = infusionslots[i];
        }
        infusionslots[0] = auxElem;
        UpdateElementsUI();
    }

    private void DebugWASDMovement()
    {
        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
        {

            transform.Rotate(new Vector3(0, 1, 0), 180 * System.Convert.ToInt32(Input.GetAxisRaw("Horizontal") != prevDirection));
            if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), 0.1f,1024))
            {
                movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
            }
            prevDirection = Input.GetAxisRaw("Horizontal");
        }
        if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
        {
            if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f), 0.1f, 1024))
            {
                movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
            }
        }
    }

    private void UpdateUI()
    {
        numberOfHitpointsText.text = numberOfHitpoints.ToString();
        numberOfTorchesText.text = numberOfTorches.ToString();
        numberOfWillPowerText.text = numberOfWillpower.ToString();
    }

    public void UpdateWillpower()
    {
        numberOfWillpower = numberOfWillpowerModifier+baseNumberOfWillpower;
    }

    public void UpdateHitpoints()
    {
        numberOfHitpoints += numberOfHitpointsModifier;
    }

    public void UpdateTorches()
    {
        numberOfTorches += numberOfTorchesModifier;
        
    }

    private void UpdateElementsUI()
    {
        for (int i = 0; i < infusionslots.Length; i++)
        {
            switch (infusionslots[i])
            {
                case elementType.None:
                    Elements[i].sprite = elementSprites[7];
                    break;
                case elementType.Earth:
                    Elements[i].sprite = elementSprites[0];
                    break;
                case elementType.Water:
                    Elements[i].sprite = elementSprites[1];
                    break;
                case elementType.Fire:
                    Elements[i].sprite = elementSprites[2];
                    break;
                case elementType.Air:
                    Elements[i].sprite = elementSprites[3];
                    break;
                case elementType.Divination:
                    Elements[i].sprite = elementSprites[4];
                    break;
                case elementType.Illusion:
                    Elements[i].sprite = elementSprites[5];
                    break;
                case elementType.Life:
                    Elements[i].sprite = elementSprites[6];
                    break;
            }
        }
    }

}

public enum PlayerClasses
{
    Shaman,
    Mage,
    Sorceress,
    Thaumaturge,
    Seer,
    Warlock,
    Druid
}
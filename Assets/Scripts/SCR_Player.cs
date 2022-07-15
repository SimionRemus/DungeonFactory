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
    public elementType[] infusionslots;
    public int numberOfTorches;
    public int numberOfWillpower;
    public int numberOfHitpoints;
    [SerializeField]
    private Text numberOfTorchesText;
    [SerializeField]
    private Text numberOfWillPowerText;
    [SerializeField]
    private Text numberOfHitpointsText;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        movePoint.parent = null;
        infusionslots = new elementType[7];
        numberOfHitpoints = 100;
        numberOfTorches = 30;
        numberOfWillpower = 3;
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
    }

    public void RotateSlots()
    {
        elementType auxElem = infusionslots[0];
        for (int i = 0; i < 6; i++)
        {
            infusionslots[i] = infusionslots[i + 1];
        }
        infusionslots[6] = auxElem;
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_NPC : MonoBehaviour
{
    public SO_NPC npcData;

    private GameObject grid;
    private int cellSize;
    private int rows, cols;
    private Vector3 currentPosition;
    private float maxAttackDistance; //to be defined in SO
    public int actualHP;

    // Start is called before the first frame update
    void Start()
    {
        grid = GameObject.Find("Grid");
        cellSize=grid.GetComponent<SCR_FloorGeneration>().cellSize;
        rows = grid.GetComponent<SCR_FloorGeneration>().rows;
        cols = grid.GetComponent<SCR_FloorGeneration>().cols;
        currentPosition = gameObject.transform.position;
        actualHP = npcData.HP;
        maxAttackDistance=1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(actualHP <= 0)
        {
            //NPC DIES
            GameObject.Destroy(this.gameObject);
        }

    }

    public void MoveNPC(GameObject aggroTarget)
    {
        int layerMask = 3840;
        if (npcData.movement.Count!=0)
        {
            if (aggroTarget == null)
            {
                Vector2Int newPos = npcData.movement[Random.Range(0, npcData.movement.Count)];
                if (!Physics2D.OverlapCircle(gameObject.transform.position + new Vector3(newPos.x, newPos.y, 0), 0.01f, layerMask))
                {
                    gameObject.transform.position += new Vector3(newPos.x, newPos.y, 0);
                }
            }
            else
            {
                float min = 1000;
                int mini = 1000;
                for (int i = 0; i < npcData.movement.Count; i++)
                {
                    if (Vector3.Distance(gameObject.transform.position + new Vector3(npcData.movement[i].x, npcData.movement[i].y, 0), aggroTarget.transform.position) < min && Vector3.Distance(gameObject.transform.position + new Vector3(npcData.movement[i].x, npcData.movement[i].y, 0), aggroTarget.transform.position)>maxAttackDistance)
                    {
                        min = Vector3.Distance(gameObject.transform.position + new Vector3(npcData.movement[i].x, npcData.movement[i].y, 0), aggroTarget.transform.position);
                        mini = i;
                    }
                }
                if (mini != 1000)
                {
                    Vector2Int newPos = npcData.movement[mini];
                    if (!Physics2D.OverlapCircle(gameObject.transform.position + new Vector3(newPos.x, newPos.y, 0), 0.01f, layerMask))
                    {
                        gameObject.transform.position += new Vector3(newPos.x, newPos.y, 0);
                    }
                }
            } 
        }
    }

    public GameObject FindAggro()
    {
        Vector2 colliderBoxSize;
        Vector3 colliderBoxPosition;
        Collider2D[] colliders;
        int layerMask = 768;
        bool isInDoor = ((int)currentPosition.x % (cellSize + 1) == 0 && (int)currentPosition.y % (cellSize+1)==3) || ((int)currentPosition.y % (cellSize + 1) == 0 && (int)currentPosition.x % (cellSize + 1) == 3);
        if(!isInDoor) //inside room
        {
            Vector2 currentRoom = new Vector2((int)currentPosition.x / (cellSize + 1), (int)currentPosition.y / (cellSize + 1));
            colliderBoxSize = new Vector2(7,7);
            colliderBoxPosition = new Vector3(currentRoom.x *(cellSize+1)+3.5f,currentRoom.y*(cellSize+1)+3.5f,0);
            colliders=Physics2D.OverlapBoxAll(colliderBoxPosition, colliderBoxSize, 0, layerMask);
            if (colliders.Length!=0)
            {
                GameObject aggroTarget=colliders[Random.Range(0, colliders.Length)].gameObject;
                return aggroTarget;
            }
        }
        else
        {
            if(currentPosition.x % (cellSize + 1) == 0) //horizontal door
            {
                colliderBoxSize = new Vector2(5,11);
                colliders = Physics2D.OverlapBoxAll(currentPosition, colliderBoxSize, 0, layerMask);
                if (colliders.Length != 0)
                {
                    return colliders[Random.Range(0, colliders.Length)].gameObject;
                }
            }
            else //vertical door
            {
                colliderBoxSize = new Vector2(11,5);
                colliders = Physics2D.OverlapBoxAll(currentPosition, colliderBoxSize, 0, layerMask);
                if (colliders.Length != 0)
                {
                    return colliders[Random.Range(0, colliders.Length)].gameObject;
                }
            }
        } 
        return null;
    }

    public void DoNPCAction()
    {
        GameObject aggroTarget = FindAggro();
        if(aggroTarget==null)
        {
            MoveNPC(null);
            aggroTarget = FindAggro();
            if(aggroTarget!=null)
            {
                if(Vector3.Distance(gameObject.transform.position,aggroTarget.transform.position)<= maxAttackDistance)
                {
                    aggroTarget.GetComponent<SCR_Player>().AffectHitpoints(-10);
                    Debug.Log("Just aggroed and attacked after random move");
                }
                else{
                    Debug.Log("Just moved randomly");
                }
            }
        }
        else
        {
            //If aggroTarget can be attacked : attack
            if (Vector3.Distance(gameObject.transform.position, aggroTarget.transform.position) <= maxAttackDistance)
            {
                Debug.Log("Already aggro, i can attack!");
                aggroTarget.GetComponent<SCR_Player>().AffectHitpoints(-10);
            }
            else
            {
                Debug.Log("Already aggro, moving towards target");
                MoveNPC(aggroTarget);
                if (Vector3.Distance(gameObject.transform.position, aggroTarget.transform.position) <= maxAttackDistance)
                {
                    Debug.Log("Agroed, moved towards target and attacking!");
                    aggroTarget.GetComponent<SCR_Player>().AffectHitpoints(-10);
                }
            }
        }
    }

    public void AffectHitpoints(int ammount)
    {
        if (actualHP + ammount > npcData.HPMax)
        {
            actualHP = npcData.HPMax;
        }
        else
        {
            if (actualHP + ammount < 0)
                actualHP = 0;
            else
                actualHP += ammount;
        }
    }

    public void ApplyEffects()
    {

    }
}

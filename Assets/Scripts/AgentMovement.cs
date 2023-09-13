using UnityEngine;
using UnityEngine.AI;

public class AgentMovement : MonoBehaviour
{
    public Vector2 target;
    NavMeshAgent agent;
    public bool moving;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        if(Vector2.Distance(target, (Vector2)transform.position) <= 0.01f)
        {
            moving = false;
            
        }
        else
        {
            moving = true;
            GameObject.Find("SelectionSystem").GetComponent<SelectingAgent>().UpdateRange();
        }
    }

    public void SetTargetPosition()
    {
        if (Input.GetMouseButtonDown(0))
        {
            target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            SetAgentPosition();
        }
    }

    public void SetTargetPosition(Vector3 pos)
    {
        if (Input.GetMouseButtonDown(0))
        {
            target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            SetAgentPosition();
        }
    }

    public void SetTargetPositionAuto(Vector3 pos)
    {
        target = pos;
        SetAgentPosition();
    }

    public void SetAgentPosition()
    {
        agent.SetDestination(new Vector3(target.x, target.y, transform.position.z));
    }
}

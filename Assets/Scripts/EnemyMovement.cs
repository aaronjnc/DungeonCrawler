using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyMovement : MonoBehaviour
{
    public float radius = 10f;
    IAstarAI ai;
    Vector3 centerpos;
    GraphNode startnode;
    EnemyAttack attack;
    // Start is called before the first frame update
    void Start()
    {
        attack = GetComponent<EnemyAttack>();
        ai = GetComponent<IAstarAI>();
        centerpos = transform.position;
        startnode = AstarPath.active.GetNearest(centerpos, NNConstraint.Default).node;
    }
    Vector3 PickRandomPoint()
    {
        var point = Random.insideUnitSphere * radius;
        point.y = 0;
        point += ai.position;
        GraphNode node2 = AstarPath.active.GetNearest(point, NNConstraint.Default).node;
        if (PathUtilities.IsPathPossible(startnode, node2))
        {
            return point;
        }
        else
            return centerpos;
    }
    float waittime = 10f;
    float timewaited = 0f;
    // Update is called once per frame
    void Update()
    {
        if (!attack.spotted)
        {
            timewaited += Time.deltaTime;
            if ((!ai.pathPending && (ai.reachedEndOfPath || !ai.hasPath) && !ai.isStopped) || (timewaited > waittime))
            {
                timewaited = 0f;
                ai.destination = PickRandomPoint();
                ai.SearchPath();
            }
        }
    }
}

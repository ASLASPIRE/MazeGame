using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PatrolEnemy : MonoBehaviour
{
    [SerializeField] private NavMeshAgent enemyAgent;
    [SerializeField] private Transform player;
    public LayerMask whatIsGround;
    public LayerMask whatIsPlayer;

    // Patrolling
    public Vector3 walkPoint;
    private bool isWalkPointSet;
    public float walkPointRange;

    // States
    public float renderRange; // Enemy only moves if player can see it, done for performance purposes
    public bool isPlayerInRenderRange;
    public float sightRange;
    public bool isPlayerInSightRange;

    // Start is called before the first frame update
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyAgent = GetComponent<NavMeshAgent>();
        StartCoroutine(Delay());
    }

    // Update is called once per frame
    private void Update()
    {
        isPlayerInRenderRange = Physics.CheckSphere(transform.position, renderRange, whatIsPlayer);
        isPlayerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);

        if (!isPlayerInRenderRange) return;
        if (isPlayerInRenderRange && !isPlayerInSightRange) Patrol();
        if (isPlayerInSightRange) ChasePlayer();
    }

    private void Patrol()
    {
        if (!isWalkPointSet) SearchWalkPoint();
        else enemyAgent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        // Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
        {
            isWalkPointSet = false;
        }
        
    }

    private void SearchWalkPoint()
    {
        // Collect random point in range
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        float randomZ = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2.0f, whatIsGround))
        {
            isWalkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        enemyAgent.SetDestination(player.position);
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSecondsRealtime(3.0f);
    }
}

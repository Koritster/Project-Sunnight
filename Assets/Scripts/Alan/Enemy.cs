using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour
{
    //identificador del NavMeshAgent
    public NavMeshAgent agent;

    public Transform player;

    //identificar capas del suelo y jugador
    public LayerMask isGround, isPlayer;

    //salud enemiga
    public float health;

    public float daño;

    //patrullar----------------------------------------------
    public Vector3 walkpoint;

    public float patrolPauseTime = 2f;

    private bool isPatrolling = true;

    //verificar si ya se configuro
    bool walkpointSet;

    //controlar el rango del punto de caminado
    public float walkpointRange;

    //Attaking------------------------------------------------
    // tiempo entre cada ataque
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //States
    public float sightRange, attackRange;
    bool playerInSightRange, playerInaAttackRange;

    [SerializeField]
    private AudioClip onHit;
    [SerializeField]
    private AudioClip[] randomNoises;

    private Animator anim;
    private AudioSource audSource;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // verificar que el jugador este en el rango de busqueda o ataque
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, isPlayer);
        playerInaAttackRange = Physics.CheckSphere(transform.position, attackRange, isPlayer);

        if (!playerInSightRange && !playerInaAttackRange && isPatrolling)
        {
            Patroling();
        }
        if (playerInSightRange && !playerInaAttackRange)
        {
            CahsePlayer();
        }
        if (playerInSightRange && playerInaAttackRange)
        {
            AttackPlayer();
        }
    }

    //navegación
    public void Awake()
    {
        agent = transform.GetChild(0).GetComponent<NavMeshAgent>();
        //player = GameObject.FindWithTag("Player").transform;
        player = Scripter.scripter.player.transform;
    }

    private void Patroling()
    {
        if (!walkpointSet)
        {
            SearchWalkpoint();
        }
        if (walkpointSet)
        {
            agent.SetDestination(walkpoint);
        }
        
        Vector3 distanceToWalkPoint = transform.position - walkpoint;

        // Se llego al Punto de destino
        if (distanceToWalkPoint.magnitude < 1)
        {
            walkpointSet = false;
            isPatrolling = false;
            StartCoroutine(PatrolWithPause());
        }
    }
    private void SearchWalkpoint()
    {
        // Intentar encontrar un punto en el NavMesh en 30 intentos
        int maxAttempts = 30;
        int attempts = 0;

        while (attempts < maxAttempts)
        {
            // Calcular una posición aleatoria dentro del rango
            float randomZ = Random.Range(-walkpointRange, walkpointRange);
            float randomX = Random.Range(-walkpointRange, walkpointRange);

            // Generar el nuevo punto
            walkpoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

            // Comprobar si el punto está dentro del NavMesh
            NavMeshHit hit;
            if (NavMesh.SamplePosition(walkpoint, out hit, walkpointRange, NavMesh.AllAreas))
            {
                // Si está dentro del NavMesh, establecer el punto
                walkpoint = hit.position;
                walkpointSet = true;
                return;
            }

            attempts++;
        }
        walkpointSet = false;
    }
    private void CahsePlayer()
    {
        //El enemigo persigue al jugador
        agent.SetDestination(player.position);
        
        if (!anim.GetBool("Running"))
        {
            anim.SetBool("Running", true);
            anim.SetBool("Walking", false);
        }
    }
    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            hambre_vida_Agua playerHealth = player.GetComponent<hambre_vida_Agua>();
            if (playerHealth != null)
            {
                playerHealth.RecibirDaño(daño); // Aplicar el daño
            }

            alreadyAttacked = true;
            anim.SetTrigger("Attack");
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }

    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Invoke(nameof(DestroyEnemy), .5f);
        }
        audSource.clip = onHit;
        audSource.Play();
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

    // Coroutine para la pausa del patrullaje
    IEnumerator PatrolWithPause()
    {
        // Esperar el tiempo de pausa antes de continuar
        agent.isStopped = true;
        anim.SetBool("Walking", false);
        yield return new WaitForSeconds(patrolPauseTime);
        agent.isStopped = false;
        isPatrolling = true;
        anim.SetBool("Walking", true);
    }
}

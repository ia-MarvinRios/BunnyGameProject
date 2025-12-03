using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float viewRange = 10f;       // Distancia a la que ve al jugador
    public float attackRange = 2f;      // Distancia a la que ataca
    public float speed = 4f;
    public float attackCooldown = 1.5f; // Tiempo entre ataques

    private float nextAttackTime = 0f;

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        // 1. Si el jugador está dentro del rango de visión → seguirlo
        if (distance < viewRange && distance > attackRange)
        {
            ChasePlayer();
        }

        // 2. Si está suficientemente cerca → atacar
        if (distance <= attackRange)
        {
            AttackPlayer();
        }
    }

    void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0; // Mantener al enemigo en el plano (evita que salte hacia arriba)

        transform.position += direction * speed * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    void AttackPlayer()
    {
        if (Time.time >= nextAttackTime)
        {
            Debug.Log("Enemy attacks!");

            // Aquí puedes llamar a un script del jugador para hacer daño:
            // player.GetComponent<PlayerHealth>().TakeDamage(10);

            nextAttackTime = Time.time + attackCooldown;
        }
    }

    // Para visualizar los rangos en Unity
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}

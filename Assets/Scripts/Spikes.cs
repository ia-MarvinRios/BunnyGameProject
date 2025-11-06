using UnityEngine;
using UnityEngine.Events;

public class Spikes : MonoBehaviour
{
    [Header("Spike Settings")]
    [SerializeField] private float _forceAmount = 10f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRb = other.gameObject.GetComponent<Rigidbody>();
            PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
            if (playerRb != null)
            {
                // Reset player's velocity
                playerRb.linearVelocity = Vector3.zero;

                // Calculate a random force
                int randomX = Random.Range(-1, 1);
                Vector3 direction = new Vector3(randomX, 1, 0);
                Vector3 force = direction.normalized * _forceAmount;

                // Apply the force to the player's Rigidbody
                playerRb.AddForce(force, ForceMode.Impulse);

                // Substract a life from the player
                playerController.UpdateLives();
            }
        }
    }
}

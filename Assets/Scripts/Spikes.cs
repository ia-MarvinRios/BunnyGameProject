using UnityEngine;

public class Spikes : MonoBehaviour
{
    [Header("Spike Settings")]
    [SerializeField] private float _upwardForceAmount = 10f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRb = other.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                playerRb.linearVelocity = Vector3.zero; // Reset the player's velocity
                // Apply an upward force to the player
                Vector3 upwardForce = new Vector3(Random.Range(-1, 1), _upwardForceAmount, 0); // Adjust the force value as needed
                playerRb.AddForce(upwardForce, ForceMode.Impulse);
            }
        }
    }
}

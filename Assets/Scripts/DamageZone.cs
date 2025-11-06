using UnityEngine;
using UnityEngine.Events;


[RequireComponent (typeof(Collider))]
public class DamageZone : MonoBehaviour
{
    [SerializeField] UnityEvent _OnGameOverZone;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _OnGameOverZone?.Invoke();
        }
    }


}

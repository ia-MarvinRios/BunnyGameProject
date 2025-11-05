using UnityEngine;


[RequireComponent (typeof(Collider))]
public class GameOverZone : MonoBehaviour
{
    [SerializeField] GameObject GameOverScream;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.ToggleGameObject(GameOverScream);
        }
    }


}

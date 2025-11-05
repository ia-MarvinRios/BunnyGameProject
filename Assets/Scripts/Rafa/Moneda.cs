using UnityEngine;

public class Moneda : MonoBehaviour
{

    [SerializeField] bool isGolden;
    [SerializeField] string SceneName;

    public bool IsGolden { get => isGolden; }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(isGolden)
            {
                GameManager.Instance.GoToScene(SceneName);
            }

            GameManager.Instance.UpdateCarrotCount(1);
            Destroy(gameObject);

        }
        
    }
}

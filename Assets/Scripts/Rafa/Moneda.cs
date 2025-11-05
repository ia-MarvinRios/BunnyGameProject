using UnityEngine;

public class Moneda : MonoBehaviour
{

    [SerializeField] bool IsGolden;
    [SerializeField] string SceneName;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(IsGolden)
            {
                GameManager.Instance.GoToScene(SceneName);
            }

            Destroy(gameObject);


        }
        
    }
}

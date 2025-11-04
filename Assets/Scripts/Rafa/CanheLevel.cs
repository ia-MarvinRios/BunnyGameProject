using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class changeLevel : MonoBehaviour
{
    [SerializeField] private string NextScene;
    [SerializeField] private float Wait;

    private void OnTrigger(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(LoadLevel());
        }
    }

    private IEnumerator LoadLevel()
    {
        yield return new WaitForSeconds(Wait);
        SceneManager.LoadScene(NextScene);
    }
}
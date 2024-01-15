using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Victory : MonoBehaviour
{
    public int sceneBuildIndex;
    private bool colliderEnabled = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EnableColliderAfterDelay());
    }
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (colliderEnabled && collision.CompareTag("Player"))
        {
            GameObject[] slimes = GameObject.FindGameObjectsWithTag("Enemy");
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            //Transform t = player.GetComponent<Transform>();
            //t.position = new Vector3(t.position.x - 3, t.position.y, t.position.z);

            foreach (var slime in slimes)
            {
                slime.SetActive(false);
                //int points = slime.GetComponent<SlimeMovement>().GetPoints();
                //Debug.Log("Slime with " + points);
            }
            
            foreach (var obj in GetDontDestroyOnLoadObjects())
            {
                if (!obj.tag.Equals("audio"))
                {
                    Destroy(obj);
                }
                //Debug.Log(obj.name + " destroyed!");
            }
            SlimeManager.Instance.scenesSlimes[sceneBuildIndex - 1] = slimes;

            SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Single);
        }
    }
    IEnumerator EnableColliderAfterDelay()
    {
        yield return new WaitForSeconds(3f); // Aguarda 3 segundos

        // Ativa o collider após o atraso
        colliderEnabled = true;
    }
    public static GameObject[] GetDontDestroyOnLoadObjects()
    {
        GameObject temp = null;
        try
        {
            temp = new GameObject();
            DontDestroyOnLoad(temp);
            UnityEngine.SceneManagement.Scene dontDestroyOnLoad = temp.scene;
            DestroyImmediate(temp);
            temp = null;

            return dontDestroyOnLoad.GetRootGameObjects();
        }
        finally
        {
            if (temp != null)
                DestroyImmediate(temp);
        }
    }
}


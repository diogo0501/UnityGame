using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    public GameObject DeathMenu;
    private GameObject[] slimes;

    // Start is called before the first frame update
    void Start()
    {
        DeathMenu = GameObject.FindGameObjectWithTag("Death");
        slimes    = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var s in slimes)
        {
            s.GetComponent<SlimeMovement>().DeathMenu = DeathMenu;
        }
        DeathMenu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            DeathMenu.SetActive(true);
        }
    }

    // Função para reiniciar o jogo
    public void RestartGame()
    {
        foreach(var obj in GetDontDestroyOnLoadObjects())
        {
            if (!obj.tag.Equals("audio"))
            {
                Destroy(obj);
            }
        }
        SceneManager.LoadSceneAsync(1);

    }

    public void Quit()
    {
        Application.Quit();
    }

    public static GameObject[] GetDontDestroyOnLoadObjects()
    {
        GameObject temp = null;
        try
        {
            temp = new GameObject();
            Object.DontDestroyOnLoad(temp);
            UnityEngine.SceneManagement.Scene dontDestroyOnLoad = temp.scene;
            Object.DestroyImmediate(temp);
            temp = null;

            return dontDestroyOnLoad.GetRootGameObjects();
        }
        finally
        {
            if (temp != null)
                Object.DestroyImmediate(temp);
        }
    }

}

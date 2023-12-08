using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    public GameObject DeathMenu;

    // Start is called before the first frame update
    void Start()
    {
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

        SceneManager.LoadSceneAsync(1);
    }
}

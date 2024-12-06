using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    [SerializeField]
    private GameObject fades;
    

    //Cambiar escena por nombre
    public void ChangeSceneByName(string scene)
    {
        StartCoroutine(FadeToChanceScene(scene));
    }

    //Cambiar escena por ID
    public void ChangeSceneByID(int scene)
    {
        StartCoroutine(FadeToChanceScene(scene));
    }

    //Salir del juego
    public void Exit()
    {
        Application.Quit();
    }

    //Inicio de la escena
    public void Start()
    {
        StartCoroutine(FadeInStart());
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    IEnumerator FadeToChanceScene(string scene)
    {
        fades.SetActive(true);
        fades.GetComponent<Animator>().Play("Fade out");
        yield return new WaitForSeconds(1.0f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
    }

    IEnumerator FadeToChanceScene(int scene)
    {
        fades.SetActive(true);
        fades.GetComponent<Animator>().Play("Fade out");
        yield return new WaitForSeconds(1.0f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
    }

    IEnumerator FadeInStart()
    {
        yield return new WaitForSeconds(1.0f);
        fades.SetActive(false);
    }
}

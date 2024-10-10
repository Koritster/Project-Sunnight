using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLoose : MonoBehaviour
{
    [SerializeField]
    GameObject scripter;

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Loose Trigger")
        {
            scripter.GetComponent<SceneManager>().ChangeSceneByName("game over");
            Debug.Log("Perdiste");
            return;
        }
        if (other.name == "Win Trigger")
        {
            scripter.GetComponent<SceneManager>().ChangeSceneByName("win");
            Debug.Log("Ganaste");
            return;
        }
    }
}

using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : MonoBehaviour
{
    public bool onFire;
    public bool withLogs = true;

    [SerializeField] private ParticleSystem fireParticles;
    [SerializeField] private GameObject logs;
    [SerializeField] private ItemClass logItemReference;

    private void Awake()
    {
        withLogs = true;
        onFire = false;

        logs.SetActive(withLogs);
    }

    public void Encender()
    {               
        Scripter.scripter.useCampfireFeedbackUI.transform.GetChild(1).gameObject.SetActive(false);
        
        onFire = true;
        fireParticles.gameObject.SetActive(onFire);
        withLogs = false;
        logs.SetActive(withLogs);
        fireParticles.Play();
        StartCoroutine(DuracionFuego(3));
    }

    public void ColocarTroncos()
    {
        Scripter.scripter.useCampfireFeedbackUI.transform.GetChild(0).gameObject.SetActive(false);

        if (Scripter.scripter.inventory.Contains(logItemReference, 3))
        {
            Scripter.scripter.inventory.Remove(logItemReference, 3);
            withLogs = true;
            logs.SetActive(withLogs);
        }
    }

    IEnumerator DuracionFuego(float secs)
    {
        yield return new WaitForSeconds(secs);
        onFire = false;
        fireParticles.gameObject.SetActive(onFire);
    }

}

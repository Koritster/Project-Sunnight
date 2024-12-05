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
        onFire = true;
        fireParticles.gameObject.SetActive(onFire);
        withLogs = false;
        logs.SetActive(withLogs);
        fireParticles.Play();
        StartCoroutine(DuracionFuego(300));
    }

    public void ColocarTroncos()
    {
        if (Scripter.scripter.inventory.Contains(logItemReference, 3))
        {
            withLogs = true;
            logs.SetActive(withLogs);
        }
    }

    IEnumerator DuracionFuego(float secs)
    {
        yield return new WaitForSeconds(secs);
        fireParticles.Stop();
        fireParticles.gameObject.SetActive(false);
    }

}

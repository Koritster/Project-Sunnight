using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AttackableObject : MonoBehaviour, IAttackable
{
    [SerializeField] AudioSource audSource;
    [SerializeField] private AttackableObjectReference obj;

    private int lifePoints;

    private void Awake()
    {
        audSource = GetComponent<AudioSource>();
        lifePoints = obj.lifePoints;
        audSource.clip = obj.soundEffect;
    }

    public void Hurt(ToolClass tool, int damagePoints)
    {
        if (obj.toolType.ToString() == "None")
        {
            return;
        }

        if (obj.toolType.ToString() != "Any")
        {
            //Si la herramienta coincide
            if (tool.toolType.ToString() == obj.toolType.ToString())
            {
                lifePoints -= damagePoints;
            }
            //Si la herramienta no coincide
            else
            {
                lifePoints -= 1;
            }
        }
        //Si es "Any"
        else
        {
            lifePoints -= damagePoints;
        }

        if(lifePoints > 0)
        {
            audSource.volume = 0.75f;
        }
        else
        {
            //Death
            audSource.volume = 1;
        }

        audSource.Play();

        StartCoroutine(CheckDead());
        Debug.Log(lifePoints);
    }

    IEnumerator CheckDead()
    {
        if(lifePoints <= 0) 
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<BoxCollider>().enabled = false;
            yield return new WaitForSeconds(1f);
            gameObject.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AttackableObject : MonoBehaviour, IAttackable
{
    [SerializeField] private AttackableObjectReference obj;

    private AudioSource audSource;
    private int lifePoints;

    private void Awake()
    {
        audSource = GetComponent<AudioSource>();
        lifePoints = obj.lifePoints;
        audSource.clip = obj.soundEffect;
    }

    public void Hurt(ToolClass tool, int damagePoints)
    {
        Debug.Log(obj.toolType.ToString());

        if (obj.toolType.ToString() == "none")
        {
            return;
        }

        if (obj.toolType.ToString() != "any")
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

        Debug.Log(lifePoints);

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
            DropItems();
            yield return new WaitForSeconds(0.1f);
            Destroy(gameObject);
        }
    }

    private void DropItems()
    {
        Renderer render = gameObject.GetComponent<Renderer>();
        for (int i = 0; i < obj.dropItems.Length; i++)
        {
            obj.dropItems[i].quantity = Random.Range((int)obj.dropItems[i].probabilities.x, (int)obj.dropItems[i].probabilities.y + 1);
            if (obj.dropItems[i].quantity <= 0)
            {
                continue;
            }

            for (int j = 0; j < obj.dropItems[i].quantity; j++)
            {
                GameObject itemPickable = Instantiate(obj.dropItems[i].item.objectPrefabPickable, Vector3.zero, Quaternion.identity, gameObject.transform);
                itemPickable.transform.localPosition = new Vector3(Random.Range(-1f, 1f), 3f, Random.Range(-1f, 1f));
                itemPickable.transform.parent = null;
                itemPickable.transform.localScale = obj.dropItems[i].item.objectPrefabPickable.transform.localScale;
            }
        }
    }
}

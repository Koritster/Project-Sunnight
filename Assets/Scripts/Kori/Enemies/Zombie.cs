using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour, IAttackable
{
    AttackableObject typeOfObject;

    public void Hurt(ToolClass tool, int damagePoints)
    {
        //Debug.Log();
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(GetComponent<IAttackable>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

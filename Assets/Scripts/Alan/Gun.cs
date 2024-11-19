
using UnityEngine;

public class Gun : MonoBehaviour
{
    public static Gun gun;

    public float GunDamage = 20f;
    public float Range = 30f;

    public Camera FpsCamera;

    public ParticleSystem MuzzleFlash;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot(); 
        }
    }

    public void Shoot()
    {
        MuzzleFlash.Play();
        RaycastHit hit;
        if (Physics.Raycast(FpsCamera.transform.position, FpsCamera.transform.forward, out hit, Range))
        {
            Debug.Log(hit.transform.name);
            Enemy target = hit.transform.GetComponent<Enemy>();
            if (target != null)
            {
                target.TakeDamage(GunDamage);
            }
        }
    }
}

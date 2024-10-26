using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemiesSpawn;

    public float spawnRange;
    public float timeBetweenSpawn;
    public float timeBetweenSpawn_2;

    void Start()
    {
        // Invocar la función repetidamente, después de x tiempo
        InvokeRepeating("SpawnNow", timeBetweenSpawn, timeBetweenSpawn_2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Vector3 RandomSpawn()
    {
        // Generar coordenadas aleatorias en X y Z dentro del rango
        float randomZ = Random.Range(-spawnRange, spawnRange);
        float randomX = Random.Range(-spawnRange, spawnRange);

        Vector3 newPos = new Vector3 (transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        
        return newPos;
    }

    private void SpawnNow()
    {
        // Elegir un enemigo aleatorio del arreglo y spawnearlo en una posición aleatoria
        Instantiate(enemiesSpawn[Random.Range(0, enemiesSpawn.Length)], RandomSpawn(), Quaternion.identity);
    }
}

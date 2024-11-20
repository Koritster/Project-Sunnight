using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemiesSpawn;

    // Rango de spawn
    public float spawnRange;

    // Configuración de la generación de hordas
    public int enemiesPerWave = 5;       
    public float timeBetweenSpawns = 1f;
    public float timeBetweenWaves = 10f;

    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    private Vector3 RandomSpawn()
    {
        float randomZ = Random.Range(-spawnRange, spawnRange);
        float randomX = Random.Range(-spawnRange, spawnRange);

        Vector3 newPos = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        return newPos;
    }

    private IEnumerator SpawnWaves()
    {
        while (true)
        {
            for (int i = 0; i < enemiesPerWave; i++)
            {
                SpawnEnemy();
                // Esperar entre cada enemigo en la misma horda
                yield return new WaitForSeconds(timeBetweenSpawns);
            }
            // Esperar el tiempo entre hordas
            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    private void SpawnEnemy()
    {
        // Elegir un enemigo aleatorio del arreglo y spawnearlo en una posición aleatoria
        Instantiate(enemiesSpawn[Random.Range(0, enemiesSpawn.Length)], RandomSpawn(), Quaternion.identity);
    }

    private void OnDrawGizmos()
    {
        // Configurar el color del Gizmo
        Gizmos.color = new Color(1, 0, 0, 0.5f); // Rojo semitransparente

        // Dibujar un círculo que representa el rango de spawn
        Gizmos.DrawWireSphere(transform.position, spawnRange);
    }
}

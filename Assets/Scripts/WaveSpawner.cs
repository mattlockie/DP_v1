using UnityEngine;
using System.Collections;

public class WaveSpawner : MonoBehaviour {

    public enum SpawnState
    {   
        Spawning,
        Waiting,
        Counting
    };

    [System.Serializable]
    public class Wave
    {
        public string name;
        public Transform enemy;
        public int count;
        public float rate;
    }

    public Wave[] waves;
    private int nextWave = 0;

    public Transform[] spawnPoints;

    public float timeBetweenWaves = 5.0f;
    private float waveCountdown;

    private SpawnState state = SpawnState.Counting;

    private float searchCountdown = 1.0f;

    void Start()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.Log("No spawn points referenced!");
        }
        if (waves.Length == 0)
        {
            Debug.Log("No waves created!");
        }

        waveCountdown = timeBetweenWaves;   
    }

    void Update()
    {
        // new: wrapped this around a GameEnded check to stop spawning when the game finishes
        if (!GameManager.GameEnded)
        {
            if (state == SpawnState.Waiting)
            {
                if (!EnemyIsAlive())
                {
                    // begin new round
                    WaveCompleted();
                }
                else
                {
                    return;
                }
            }

            if (waveCountdown <= 0.0f)
            {
                if (state != SpawnState.Spawning)
                {
                    StartCoroutine(SpawnWave(waves[nextWave]));
                }
            }
            else
            {
                waveCountdown -= Time.deltaTime;
            }
        }
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        state = SpawnState.Spawning;

        for (int i = 0; i < _wave.count; i++)
        {
            if (!GameManager.GameEnded)
            {
                SpawnEnemy(_wave.enemy);
                yield return new WaitForSeconds(1.0f / _wave.rate);
            }
        }

        state = SpawnState.Waiting;

        yield break;
    }

    void WaveCompleted()
    {
        //Debug.Log("Wave completed!");
        state = SpawnState.Counting;
        waveCountdown = timeBetweenWaves;

        if (nextWave + 1 > waves.Length - 1)
        {
            nextWave = 0;
            Debug.Log("All waves complete! Looping...");
        }
        else
        {
            nextWave++;
        }
    }

    bool EnemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0.0f)
        {
            searchCountdown = 1.0f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return false;
            }
        }
        return true;
    }

    void SpawnEnemy(Transform _enemy)
    {
        // spawn an enemy at a random spawn point
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(_enemy, spawnPoint.position, spawnPoint.rotation);
    }
}
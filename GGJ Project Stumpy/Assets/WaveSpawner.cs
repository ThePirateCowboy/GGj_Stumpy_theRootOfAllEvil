using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveSpawner : MonoBehaviour
{

    
    public enum SpawnState { SPAWNING, WAITING, COUNTING};

    [System.Serializable]
    public class Wave
    {
        public string name;
        public GameObject []enemy;
        public int count;
        public float rate;
    }
    //countdown text
    public TextMeshProUGUI CountdownUIText;

    public Transform[] spawnPoints;

    public Wave[] waves;
    private int nextWave = 0;
    public float timeBetweenWaves = 5f;
    private float waveCountdown;
    public bool canSpawn;
    private float searchCountdown = 1f;
    private bool FirstTime = true;

    public TextMeshProUGUI WaveTitle, WaveDescription;

    private SpawnState state = SpawnState.COUNTING;


    void Start()
    {
        waveCountdown = timeBetweenWaves;

    }

    

   
    private void Update()
    {
       
        if(state == SpawnState.WAITING)
        {
            //check if enemies are still alive.
            if(!EnemyIsAlive())
            {
                WaveCompleted();
            }
            else
            {
                return;
            }
        }

        if (waveCountdown <= 0 && canSpawn)
        {
            if (FirstTime)
            {
                StartCoroutine(ShowFirstWaveText());
                FirstTime = false;
            }
            if (state != SpawnState.SPAWNING)
            {
                //start spawning a wave
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
            
        }
        else
        {
            waveCountdown -= Time.deltaTime;
        }
    }
    private void WaveCompleted()
    {
        StartCoroutine(ShowWaveText());
        Debug.Log("WaveCompleted" + waves[nextWave].name);
        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;

        if (nextWave +1 > waves.Length - 1)
        {
            nextWave = 0;
            Debug.Log("completed all waves");
        }
        else
        {

            nextWave++;
        }
    }
   

  IEnumerator ShowWaveText()
    {
        /// Make a sound for completeing the level. 
        /// 
        yield return new WaitForSeconds(2);
        WaveTitle.text = "Wave: " + (nextWave +1);
        WaveDescription.text = "" + waves[nextWave+1].name;
        WaveTitle.gameObject.SetActive(true);
        WaveDescription.gameObject.SetActive(true);
       
        yield return new WaitForSeconds(2.5f);
        WaveTitle.gameObject.SetActive(false);
        WaveDescription.gameObject.SetActive(false);

        yield break;
    }
    IEnumerator ShowFirstWaveText()
    {
        /// Make a sound for completeing the level. 
        if(WaveTitle !=null&& WaveDescription!=null)
        {
            yield return new WaitForSeconds(2);
            WaveTitle.text = "Wave: 1";
            WaveDescription.text = "" + waves[nextWave].name;
            WaveTitle.gameObject.SetActive(true);
            WaveDescription.gameObject.SetActive(true);

            yield return new WaitForSeconds(2.5f);
            WaveTitle.gameObject.SetActive(false);
            WaveDescription.gameObject.SetActive(false);

            yield break;
        }
        else
        {
            WaveTitle = FindObjectOfType<WaveTitle>().GetComponent<TextMeshProUGUI>();
            WaveDescription = FindObjectOfType<WaveDescription>().GetComponent<TextMeshProUGUI>();
        }
       
    }

    bool EnemyIsAlive()
    {

        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0f)
        {


            searchCountdown = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {


                return false;
            }
            
        }


        return true;
    }
    IEnumerator SpawnWave (Wave _wave)
    {
        Debug.Log("Spawning Wave");
        state = SpawnState.SPAWNING;
        //spawn
        for (int i = 0; i< _wave.count; i++)
        {
            SpawnEnemy(_wave.enemy[Random.Range(0,5)]);
            yield return new WaitForSeconds(1 / _wave.rate);
        }
        state = SpawnState.WAITING;
        yield break;
    }
    void SpawnEnemy(GameObject _enemy)
    {
        Debug.Log("Spawning Enemy: " + _enemy.name);
        Transform sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(_enemy, sp.position, sp.rotation);
       
    }
}

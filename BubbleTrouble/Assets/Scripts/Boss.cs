using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

enum BossState
{
    ArcShots,
    RapidFire,
    ScatterShots,
    Hiding
}

public class Boss : MonoBehaviour
{
    public GameObject ShotPrefab;
    public Transform ShotSpawnPoint;
    public float BasicShotSpeed = 10f;
    public float TimeBetweenScatterShots = 1f;

    public int MaxHealth = 10000;
    public int CurrentHealth { get; private set; }
    
    private float _timer = 0f;
    
    BossState _state = BossState.ArcShots;

    
    [Header("RapidShot")]
    public int RapidTimesToShoot = 15;
    public float TimeBetweenRapidShots = 1f;
    
    [Header("ArcShot")]
    public float TimeBetweenWaves = 1f;
    public int ArcTimesToShoot = 3;
    public int arcShots = 9;
    [Header("ScatterShot")]
    public int ScatterTimesToShoot = 3;
    public float ScatterShotSpeed = 10f;
    public int scatterShots = 50;
    
    
    private int _timesShot = 0;
    
    [Header("Hiding")]
    public Transform hidingSpot;
    public List<EnemySpawner> spawners;
    private Coroutine _hidingCoroutine;
    public float hidingTime = 3f; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CurrentHealth = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentHealth <= 0)
            return;
        if (_hidingCoroutine != null)
            return;

        float timeBetweenShots = TimeBetweenRapidShots;
        if (_state == BossState.ScatterShots)
        {
            timeBetweenShots = TimeBetweenScatterShots;
        }
        else if (_state == BossState.ArcShots)
        {
            timeBetweenShots = TimeBetweenWaves;
        }

        float timesToShoot = RapidTimesToShoot;
        if (_state == BossState.ScatterShots)
        {
            timesToShoot = ScatterTimesToShoot;
        }
        else if (_state == BossState.ArcShots)
        {
            timesToShoot = ArcTimesToShoot;
        }
        
        if (_timer >= timeBetweenShots)
        {
            _timer = 0;

            if (_state == BossState.Hiding)
            {
                _hidingCoroutine = StartCoroutine(HidingPhase());
            }
            else if (_state == BossState.ScatterShots)
            {
                ShootBubbleShots();
            }else if (_state == BossState.RapidFire)
            {
                ShootBasicShot();
            }
            else if (_state == BossState.ArcShots)
            {
                ShootBubblesInArc();
            }
            _timesShot++;
            if(_timesShot >= timesToShoot)
            {
                ChangeState();
            }
        }

        _timer += Time.deltaTime;
    }

    void ShootBubblesInArc()
    {
        var ogRotation = ShotSpawnPoint.rotation;
        ShotSpawnPoint.rotation = Quaternion.Euler(0, Random.Range(-15, 15), 0);
        // Shoot a number of shots in an 180 degree arc with even 20 degree separation
        for (int i = 0; i < arcShots; i++)
        {
            GameObject bubbleShot = Instantiate(ShotPrefab, ShotSpawnPoint.position, ShotSpawnPoint.rotation);
            Rigidbody rb = bubbleShot.GetComponent<Rigidbody>();
            // Get vector inside a 180 degree arc
            Vector3 direction = Quaternion.Euler(0, i * 20 - 90, 0) * -ShotSpawnPoint.forward;

            rb.linearVelocity = direction * ScatterShotSpeed;
        }
        ShotSpawnPoint.rotation = ogRotation;
    }
    
    void ShootBubbleShots()
    {
        for (int i = 0; i < scatterShots; i++)
        {
            GameObject bubbleShot = Instantiate(ShotPrefab, ShotSpawnPoint.position, ShotSpawnPoint.rotation);
            Rigidbody rb = bubbleShot.GetComponent<Rigidbody>();
            // Get random vector inside a 10 degree cone
            Vector3 direction = (PlayerMovement.Instance.transform.position - transform.position).normalized;
            direction = Vector3.Scale(direction, new Vector3(1, 0, 1));
            direction = Quaternion.Euler(0, Random.Range(-10, 10), 0) * direction;

            rb.linearVelocity = direction * ScatterShotSpeed;
        }
    }
    
    void ShootBasicShot()
    {
        GameObject bubbleShot = Instantiate(ShotPrefab, ShotSpawnPoint.position, ShotSpawnPoint.rotation);
        Rigidbody rb = bubbleShot.GetComponent<Rigidbody>();
        // Get random vector inside a 2 degree cone
        Vector3 direction = (PlayerMovement.Instance.transform.position - transform.position).normalized;
        direction = Vector3.Scale(direction, new Vector3(1, 0, 1));
        direction = Quaternion.Euler(0, Random.Range(-2, 2), 0) * direction;

        rb.linearVelocity = direction * BasicShotSpeed;
    }

    void ChangeState()
    {
        var previous = _state;
        // Switch state to random state
        while (_state == previous)
            _state = (BossState)Random.Range(0, Enum.GetValues(typeof(BossState)).Length);
        _timesShot = 0;
        _timer = 0;
    }

    IEnumerator HidingPhase()
    {
        Vector3 originPoint = transform.position;
        float t = 0;
        while (t < hidingTime)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(originPoint, hidingSpot.position, t / hidingTime);
            yield return null;
        }
        foreach (var spawner in spawners)
        {
            spawner.gameObject.SetActive(true);
        }

        float totalWait = 0;
        while (FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length == 0)
        {
            Debug.Log($"Found: {FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length} enemies");
            totalWait += 0.5f;
            yield return new WaitForSeconds(0.5f);
            if (totalWait > 20)
            {
                break;
            }
        }

        while (FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length > 0)
        {
            Debug.Log($"Found: {FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length} enemies");
            totalWait += 1f;
            yield return new WaitForSeconds(1);
            if (totalWait > 20)
            {
                break;
            }
        }
        
        foreach (var spawner in spawners)
        {
            spawner.gameObject.SetActive(false);
            spawner.ResetAmount();
        }

        t = 0;
        while (t < hidingTime)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(hidingSpot.position, originPoint, t / hidingTime);
            yield return null;
        }

        ChangeState();
        _hidingCoroutine = null;
    }

    public void TakeDamage(int damage)
    {
        if (_hidingCoroutine != null)
            return;
        
        CurrentHealth -= damage;
        Debug.Log("BossHealth:" + CurrentHealth);
        if (CurrentHealth <= 0)
        {
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        Debug.Log("Boss is Ded!");
        const float timeToDie = 10f;
        const float floatAmount = 12f;

        
        Vector3 originOrigin = transform.position;
        Vector3 originDest = transform.position - (Vector3.up * floatAmount);
        Vector3 originPoint = transform.position;
        float t = 0;
        while (t < timeToDie)
        {
            t += Time.fixedDeltaTime;
            transform.position = Random.insideUnitSphere * 0.1f + originPoint;
            originPoint = Vector3.Lerp(originOrigin, originDest, t / timeToDie);
            yield return new WaitForFixedUpdate();
        }
        
        gameObject.SetActive(false);
        
        //TODO: Trigger stage end
    }
}
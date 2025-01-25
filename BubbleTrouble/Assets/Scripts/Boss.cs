using UnityEngine;

enum BossState
{
    ArcShots,
    RapidFire
}

public class Boss : MonoBehaviour
{
    public GameObject ShotPrefab;
    public Transform ShotSpawnPoint;
    public float ShotSpeed = 10f;
    public float TimeBetweenShots = 1f;
    private float _timer = 0f;
    
    BossState _state = BossState.ArcShots;

    public int timesToShoot = 3;
    private int _timesShot = 0;
    public int arcShots = 9;
    public int rapidFireShots = 50;

    public Transform hidingSpot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (_timer >= TimeBetweenShots)
        {
            _timer = 0;
            
            if (_state == BossState.ArcShots)
            {
                ShootBubbleShots();
            }
            else
            {
                ShootBubblesInArc();
            }
            _timesShot++;
            if(_timesShot >= timesToShoot)
            {
                var previous = _state;
                // Switch state to random state
                while(_state == previous)
                    _state = (BossState)Random.Range(0, 2);
                _timesShot = 0;
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

            rb.linearVelocity = direction * ShotSpeed;
        }
        ShotSpawnPoint.rotation = ogRotation;
    }
    
    void ShootBubbleShots()
    {
        for (int i = 0; i < rapidFireShots; i++)
        {
            GameObject bubbleShot = Instantiate(ShotPrefab, ShotSpawnPoint.position, ShotSpawnPoint.rotation);
            Rigidbody rb = bubbleShot.GetComponent<Rigidbody>();
            // Get random vector inside a 10 degree cone
            Vector3 direction = (PlayerMovement.Instance.transform.position - transform.position).normalized;
            direction = Quaternion.Euler(0, Random.Range(-10, 10), 0) * direction;

            rb.linearVelocity = direction * ShotSpeed;
        }
    }
}
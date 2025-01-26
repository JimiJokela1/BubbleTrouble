using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject BubbleShotPrefab;
    public Transform BubbleShotSpawnPoint;

    public int BubbleShotCount = 10;
    public float BubbleShotInterval = 0.1f;
    private float BubbleShotTimer = 0f;

    public float BubbleShotSpeedMin = 10f;
    
    public float BubbleShotSpeedMax = 20f;

    public AudioClip bubbleShotSound;

    void Update()
    {
        BubbleShotTimer += Time.deltaTime;

        if (BubbleShotTimer > BubbleShotInterval)
        {
            BubbleShotTimer = 0;
            if (Input.GetMouseButton(0))
            {
                ShootBubbleShot();
            }
        }
    }

    void ShootBubbleShot()
    {
        for (int i = 0; i < BubbleShotCount; i++)
        {
            GameObject bubbleShot = Instantiate(BubbleShotPrefab, BubbleShotSpawnPoint.position, BubbleShotSpawnPoint.rotation);
            Rigidbody rb = bubbleShot.GetComponent<Rigidbody>();
            // Get random vector inside a 10 degree cone
            Vector3 direction = bubbleShot.transform.forward;
            direction = Quaternion.Euler(0, Random.Range(-10, 10), 0) * direction;

            rb.linearVelocity = direction * Random.Range(BubbleShotSpeedMin, BubbleShotSpeedMax);

            AudioSource.PlayClipAtPoint(bubbleShotSound, transform.position);
        }
    }
}

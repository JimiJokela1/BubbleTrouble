using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject BubbleShotPrefab;
    public Transform BubbleShotSpawnPoint;

    public int BubbleShotCount = 10;

    public float BubbleShotSpeedMin = 10f;
    
    public float BubbleShotSpeedMax = 20f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ShootBubbleShot();
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
            direction = Quaternion.Euler(Random.Range(-10, 10), Random.Range(-10, 10), 0) * direction;

            rb.linearVelocity = direction * Random.Range(BubbleShotSpeedMin, BubbleShotSpeedMax);
        }
    }
}

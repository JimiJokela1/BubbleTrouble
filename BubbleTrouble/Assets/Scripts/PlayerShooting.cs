using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject BubbleShotPrefab;
    public Transform BubbleShotSpawnPoint;

    public float BubbleShotSpeed = 10f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ShootBubbleShot();
        }
    }

    void ShootBubbleShot()
    {
        GameObject bubbleShot = Instantiate(BubbleShotPrefab, BubbleShotSpawnPoint.position, BubbleShotSpawnPoint.rotation);
        Rigidbody rb = bubbleShot.GetComponent<Rigidbody>();
        // Get random vector inside a 10 degree cone
        Vector3 randomVector = Random.insideUnitSphere;
        // Limit random vector to a 10 degree cone
        randomVector.y = Mathf.Abs(randomVector.y);
        randomVector.Normalize();
        // Apply random vector to BubbleShotSpawnPoint.forward
        bubbleShot.transform.forward = randomVector;

        rb.linearVelocity = bubbleShot.transform.forward * BubbleShotSpeed;
    }
}

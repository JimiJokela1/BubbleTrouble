using System.Collections;
using UnityEngine;

public class EnemyShot : MonoBehaviour
{
    public int Damage = 10;
    public int Radius = 10;
    private Material bubbleMat;

    private void Start()
    {
        bubbleMat = GetComponent<MeshRenderer>().material;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement.Instance.TakeDamage(Damage);
            StartCoroutine(FadeOut());
        }

        if (collision.gameObject.CompareTag("PlayerShot"))
        {
            StartCoroutine(FadeOut());
            collision.gameObject.GetComponent<BubbleShot>().StartFadeOut();
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            StartCoroutine(FadeOut());
        }

        if (collision.gameObject.CompareTag("Floor"))
        {
            if (collision.gameObject.GetComponent<CleaningTest>() is CleaningTest cleaning)
            {
                cleaning?.Dirtify(transform.position, collision.GetContact(0).point, Radius);
            }

            StartCoroutine(FadeOut());
        }
    }

    IEnumerator FadeOut()
    {
        GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        GetComponent<Collider>().enabled = false;
        float fade = 0;
        while (fade < 1)
        {
            fade += Time.deltaTime * 10;
            bubbleMat.SetFloat("_Dissolve", fade);
            yield return null;
        }

        Destroy(gameObject);
    }
}
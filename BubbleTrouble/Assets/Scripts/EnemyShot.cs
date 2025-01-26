using System.Collections;
using UnityEngine;

public class EnemyShot : MonoBehaviour
{
    public int Damage = 10;
    public int Radius = 10;
    private Material bubbleMat;

    public AudioClip popSound;

    private void Start()
    {
        bubbleMat = GetComponent<MeshRenderer>().material;
    }

    private void Update()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            if (hit.collider.tag == "Floor")
            {
                if (hit.collider.GetComponent<CleaningTest>() is CleaningTest cleaning)
                {
                    cleaning?.Dirtify(transform.position, hit.point, Radius);
                }
            }
        }
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

        AudioSource.PlayClipAtPoint(popSound, transform.position);

        Destroy(gameObject);
    }
}
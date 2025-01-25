using System;
using System.Collections;
using UnityEngine;

public class BubbleShot : MonoBehaviour
{
    public int CleanRadius = 10;
    public int Damage = 10;
    private Material bubbleMat;

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
                    cleaning?.Clean(transform.position, hit.point, CleanRadius);
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponentInParent<Enemy>().TakeDamage(Damage);
            StartCoroutine(FadeOut());
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            StartCoroutine(FadeOut());
        }

        if (collision.gameObject.CompareTag("Floor"))
        {
            if (collision.gameObject.GetComponent<CleaningTest>() is CleaningTest cleaning)
            {
                cleaning?.Clean(transform.position, collision.GetContact(0).point, CleanRadius);
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

    internal void StartFadeOut()
    {
        StartCoroutine(FadeOut());
    }
}
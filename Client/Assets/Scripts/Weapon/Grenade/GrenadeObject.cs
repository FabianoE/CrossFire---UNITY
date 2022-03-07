using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeObject : MonoBehaviour
{
    public AudioSource _audio;

    public float delay = 2f;
    public GameObject explosionEffect;
    public float force = 20;
    public float radius = 10;
    float countdown;
    bool hasExploded = false;

    Rigidbody _rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
           countdown = delay;
        _rigidbody.AddForce(transform.forward * force, ForceMode.Impulse);
        Physics.IgnoreLayerCollision(1, 1);
        Physics.IgnoreCollision(this.GetComponent<Collider>(), GetComponent<Collider>());
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;

        if(countdown <= 0 && hasExploded == false)
        {
            Explode();
            hasExploded = true;
        }
    }

    void Explode()
    {
        _audio.Play();
        Collider[] coll = Physics.OverlapSphere(transform.position, radius);
        List<int> ls = new List<int>();
        foreach (Collider hit in coll)
        {
            RaycastHit ray;
            if(Physics.Linecast(transform.position, hit.transform.position, out ray))
            {
                if (ray.collider.CompareTag("TPlayer"))
                {
                    BulletPlayerCollider rb = ray.collider.GetComponent<BulletPlayerCollider>();
                    if (rb != null && !ls.Contains(rb.playerManager.id))
                    {
                        ls.Add(rb.playerManager.id);

                        if(rb.playerManager.id == Client.instance.myId)
                        {
                            CameraShake.instance.ShakeOnce();
                        }

                        Debug.LogError(rb.playerManager.username);
                    }
                }
            }

        }
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(gameObject, 1f) ;
    }
}

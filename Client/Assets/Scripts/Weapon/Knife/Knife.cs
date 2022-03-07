using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    public Animator _animator;
    public WeaponController _weaponController;
    public GameObject DamageCollider;
    public Transform DmT;

    public Transform _Start;
    public Transform End;

    [Header("COMBO 1 CONFIG")]
    public float C1 = 0f;
    public float C1_2 = 0f;
    [Space]
    [Header("COMBO 2 CONFIG")]
    public float C2 = 0f;
    public float C2_2 = 0f;
    [Space]
    [Header("BIGSHOT CONFIG")]
    public float B = 0f;
    public float B_2 = 0f;
    [Space]
    public List<int> AllColliders = new List<int>();

    private void Start()
    {
        _weaponController = GetComponent<WeaponController>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (DamageCollider.active)
            CheckMeleeCombat();
    }

    public void Attack(int t, float alternativetime)
    {
        switch (t)
        {
            case 1:
                if (Time.time > _weaponController.AlternativeTime)
                {
                    _animator.Play("Combo1");
                    _weaponController.AlternativeTime = Time.time + _weaponController.KnifeWait;
                    StartCoroutine(AttackC1());
                }
                else
                {
                    _animator.Play("Combo2");
                    _weaponController.AlternativeTime = 0;

                    StartCoroutine(AttackC2());
                }
                break;
            case 2:
                _animator.Play("BigShot");
                StartCoroutine(AttackBig());
                break;

        }
    }

    public void CheckMeleeCombat()
    {
        Ray ray = new Ray();
        RaycastHit hit;

        ray.origin = DamageCollider.transform.position;
        ray.direction = DamageCollider.transform.forward;

        if(Physics.Raycast(ray, out hit, 5))
        {
            if (hit.collider.CompareTag("TPlayer"))
            {
                BulletPlayerCollider coll = hit.collider.GetComponent<BulletPlayerCollider>();

                if (coll != null && !AllColliders.Contains(coll.playerManager.id))
                {
                    Debug.LogWarning($"Collisão: {coll.playerManager.username}");
                    _weaponController._playerManager._playerNetwork.SendDamage(Client.instance.myId, coll.playerManager.id, _weaponController._playerManager.WeaponID, coll.Part);
                    AllColliders.Add(coll.playerManager.id);
                }
            }

            Debug.DrawRay(ray.origin, ray.direction, Color.cyan);
        }
    }

    IEnumerator AttackC1()
    {
        yield return new WaitForSeconds(C1);

        CheckMeleeCombat();
        DamageCollider.SetActive(true);

        yield return new WaitForSeconds(C1_2);

        DamageCollider.SetActive(false);

        AllColliders.Clear();
    }

    IEnumerator AttackC2()
    {
        yield return new WaitForSeconds(C2);

        DamageCollider.SetActive(true);

        yield return new WaitForSeconds(C2_2);

        DamageCollider.SetActive(false);

        AllColliders.Clear();
    }

    IEnumerator AttackBig()
    {
        yield return new WaitForSeconds(B);

        DamageCollider.SetActive(true);

        yield return new WaitForSeconds(B_2);

        DamageCollider.SetActive(false);

        AllColliders.Clear();
    }
}

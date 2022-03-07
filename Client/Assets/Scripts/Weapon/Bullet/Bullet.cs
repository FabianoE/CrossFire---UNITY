using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 origin;
    private Vector3 nPrevPos;
    private List<int> PlayerDamage = new List<int>();

    public int PlayerID;
    public int WeaponID;
    public float Veloc;

    public GameObject Coll;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        origin = transform.position;
        nPrevPos = transform.position;
    }

    public void SetData(int playerid, int weaponid, float veloc)
    {
        PlayerID = playerid;
        WeaponID = weaponid;
        Veloc = veloc;
    }

    RaycastHit hit;
    void Update()
    {
        nPrevPos = transform.position; 
        Vector3 h = transform.right * 0;

        Vector3 v = transform.forward * 1;
        Vector3 veloc = (v + h).normalized * Veloc;

        rb.MovePosition(rb.position + veloc * Time.fixedDeltaTime);

        RaycastHit[] hits = Physics.RaycastAll(new Ray(nPrevPos, (rb.position - nPrevPos).normalized), (rb.position - nPrevPos).magnitude);


        for (int i = 0; i < hits.Length; i++)
        {
            var hit = hits[i].collider;
            switch (hit.tag)
            {
                case "TPlayer":
                    Debug.LogError(hit.GetComponent<BulletPlayerCollider>().Part);
                    var ha = hit.GetComponent<BulletPlayerCollider>();
                    var player = ha.playerManager;
                    if (!PlayerDamage.Contains(player.id) && player.id != Client.instance.myId && player.id != PlayerID)
                    {
                        ha.Attack(Client.instance, PlayerID, player.id, WeaponID, ha.Part);
                        //damag._playerNetwork.SendDamage(PlayerID, player.id, WeaponID, ha.Part);
                        //GameManager.players[player.id]._playerNetwork.SendDamage((PlayerID, player.id, WeaponID, ha.Part));
                        //hit.gameObject.transform.root.GetComponent<PlayerManager>().Test();
                        //player._playerNetwork.SendDamage(PlayerID, player.id, WeaponID, ha.Part);
                        PlayerDamage.Add(player.id);
                        Destroy(gameObject);
                    }
                    break;
                default:
                    if (!hit.CompareTag("Bullet") && !hit.CompareTag("Player"))
                    {
                        Instantiate(Coll, hits[i].point, Quaternion.FromToRotation(Vector3.up, hits[i].normal));
                        Destroy(gameObject);

                        Debug.LogError(hit.tag);
                    }
                    break;
            }
        }
    }
}

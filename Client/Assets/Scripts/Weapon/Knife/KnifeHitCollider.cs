using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeHitCollider : MonoBehaviour
{
    public PlayerManager _playerManager;

    public List<int> AllColliders = new List<int>();


    void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("Player")) 
        {
            var coll = collision.collider.GetComponent<BulletPlayerCollider>();
            if(coll != null && !AllColliders.Contains(coll.playerManager.id))
            {
                Debug.LogWarning(coll.Part);
                _playerManager._playerNetwork.SendDamage(Client.instance.myId, coll.playerManager.id, _playerManager.WeaponID, coll.Part);
                AllColliders.Add(coll.playerManager.id);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource WeaponAudio;
    public AudioSource FootstepAudio;
    [Space]
    public PlayerManager _playerManager;
    [Space]
    public AudioClip[] FootClipDirt;
    public AudioClip[] FootClipMetal;

    //FootStepConfig
    public float _Time = 0f;

    private void Awake()
    {
        _playerManager = GetComponent<PlayerManager>();
    }

    public void SelectWeapon(WeaponsInfo weapon)
    {
        WeaponAudio.clip = weapon.Select;
        WeaponAudio.Play();
    }

    public void WeaponShoot(WeaponsInfo weapon)
    {
        WeaponAudio.PlayOneShot(weapon.Shoot[0]);
    }

    public void ReloadWeapon(WeaponsInfo weapon)
    {
        StartCoroutine(Reload(weapon));
    }

    public void FootStepSound(bool lowed, bool walking, bool shift)
    {
        RaycastHit footRay;
        string tag = null;
        int i = 0;
        if (!lowed && walking && !shift)
        {
            if(Time.time > _Time)
            {
                //if (!_playerManager._characterController.isGrounded)
                //    return;

                if (Physics.Raycast(transform.position, -Vector3.up, out footRay, 1))
                    tag = footRay.transform.tag;
                else
                    return;

                switch (tag)
                {
                    case "Metal":
                        i = Random.Range(1, FootClipMetal.Length);
                        FootstepAudio.clip = FootClipMetal[i];
                        FootstepAudio.PlayOneShot(FootstepAudio.clip);
                        FootClipMetal[i] = FootClipMetal[0];
                        FootClipMetal[0] = FootstepAudio.clip;
                        break;

                    case "Wood":
                        i = Random.Range(1, FootClipDirt.Length);
                        FootstepAudio.clip = FootClipDirt[i];
                        FootstepAudio.PlayOneShot(FootstepAudio.clip);
                        FootClipDirt[i] = FootClipDirt[0];
                        FootClipDirt[0] = FootstepAudio.clip;
                        break;

                    default:
                        i = Random.Range(1, FootClipDirt.Length);
                        FootstepAudio.clip = FootClipDirt[i];
                        FootstepAudio.PlayOneShot(FootstepAudio.clip);
                        FootClipDirt[i] = FootClipDirt[0];
                        FootClipDirt[0] = FootstepAudio.clip;
                        break;
                }

                _Time = Time.time + FootstepAudio.clip.length;
            }
        }
    }

    IEnumerator Reload(WeaponsInfo weapon)
    {
        float t_time = weapon.ReloadTime / 3;

        if(weapon.ReloadSounds[0] != null)
        {
            WeaponAudio.clip = weapon.ReloadSounds[0];
            WeaponAudio.Play();
        }
        yield return new WaitForSeconds(t_time); 

        if (weapon.ReloadSounds[1] != null)
        {
            WeaponAudio.clip = weapon.ReloadSounds[1];
            WeaponAudio.Play();
        }
        yield return new WaitForSeconds(t_time);

        if (weapon.ReloadSounds[2] != null)
        {
            WeaponAudio.clip = weapon.ReloadSounds[2];
            WeaponAudio.Play();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : IsMine
{
    public PlayerManager _playerManager;

    public Transform SpawnBullet;
    public GameObject Bullet;
    public ParticleSystem[] MuzzleFlash;
    public Animator _animator;
    public WeaponRecoil _weaponRecoil;

    public int WEAPONID = 1;

    public int maxammo;
    public int ammo;
    public int maxammoinpaint;
    public int amooinpaint;
    public int offset = 12387;
    public int reloadTime;

    public float firerate;

    public float nextfire;
    public int bullets { get => amooinpaint - offset; set { amooinpaint = value + offset; } }
    public int ammosb { get => ammo - offset; set { ammo = value + offset; } }

    public WType Tipo;

    public WeaponsInfo _weaponInfoData;

    [Space]
    [Header("Knife Settings")]
    public float KnifeWait = 0f;
    public float AlternativeTime = 0f;
    public Knife _knifeWeaponSettings;
    [Space]
    public bool CrossHair = false;
    public bool firing = false;
    public bool cancelPosShot = false;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _weaponRecoil = GetComponent<WeaponRecoil>();

        _playerManager = transform.root.GetComponent<PlayerManager>();
        _weaponInfoData = transform.root.GetComponent<PlayerManager>()._playerWeaponsManager.GetWeaponById(WEAPONID);

        _knifeWeaponSettings = GetComponent<Knife>();
    }

    private void OnDisable()
    {
        AlternativeTime = 0;
    }
    void Update()
    {
        if (!isMine())
            return;
        if (Pvp_ChatController.instance.PVP_InputText.IsActive())
            return;

        if (!WeaponOtherAnim())
        {

            if (bullets > 0 && Tipo != WType.Knife)
            {
                if (Tipo == WType.Rifle)
                    WeaponRifle();
                else if (Tipo == WType.Sniper)
                    WeaponSniper();
                else if (Tipo == WType.Pistol)
                    WeaponPistol();

                if (_weaponInfoData.CrossHair != "" || _weaponInfoData.Zoom != 0 || iFire())
                    CheckCrossHair();

            }
            else
            {
                if (Tipo == WType.Knife)
                    WeaponKnife();
                else if (Tipo == WType.Grenade)
                    WeaponGrenade();

                if (Input.GetMouseButton(0) && Tipo != WType.Knife && !WeaponOtherAnim())
                    SendReload();
            }

            if (Input.GetKeyDown(KeyCode.R) && Tipo != WType.Knife && !WeaponOtherAnim())
            {
                SendReload();
            }
        }

        UIATT();
        UICrossHair.instance.OnFire(firing);
    }

    public void SelectedWeapon() //Disable CrossHair Image
    {
        cancelPosShot = false;
        CrossHair = false;
        UIPVPManager.instance.SetCrossHair(_weaponInfoData.CrossHair, false);
        SetCrossHair(true);

        UICrossHair.instance.SetWeapon(_weaponInfoData);

        if (isMine())
        {
            if (Tipo == WType.Sniper)
                UIPVPManager.instance.DynamicCrossHair.gameObject.SetActive(false);
            else if (Tipo == WType.Knife)
            {
                //_knifeWeaponSettings.DamageCollider.GetComponent<KnifeHitCollider>().AllColliders.Clear();
            }
            else
                UIPVPManager.instance.DynamicCrossHair.gameObject.SetActive(true);
        }
    }

    public void StartMuzzleFlash()
    {
        if(MuzzleFlash != null)
            for(int i = 0; i < MuzzleFlash.Length; i++)
                MuzzleFlash[i].Play();
    }

    public void WeaponReloaded()
    {
        //if(isMine())
        //    StartCoroutine(Reload2());
    }

    public void WeaponRespawned()
    {
        bullets = maxammoinpaint;
        ammosb = maxammo;
    }

    public void SetWeaponData(int _maxammoinpaint, int _maxammo) //Bullets
    {
        bullets = _maxammoinpaint;
        ammosb = _maxammo;

        SelectedWeapon();
    }

    void SendReload()
    {
        if (bullets < maxammoinpaint && ammosb > 0)
        {
            using (Packet packet = new Packet((int)6))
            {
                packet.Write(Client.instance.myId);
                packet.Write(106);

                packet.WriteLength();

                Client.instance.tcp.SendData(packet);
            }

            cancelPosShot = true;
        }
    }

    void CheckCrossHair()
    {
        if(Input.GetMouseButtonDown(1))
        {
            if (_weaponInfoData.CrossHair != "")
            {
                bool t;

                if (CrossHair == true)
                    t = false;
                else
                    t = true;

                CrossHair = t;
                UIPVPManager.instance.SetCrossHair(_weaponInfoData.CrossHair, t);

                SetCrossHair(!t);
            }
        }
    }

    void SetCrossHair(bool en)
    {
        _playerManager._camMainFPS.fieldOfView = en == true ? 60f : 20f;
        _playerManager._camWeaponFPS.enabled = en;
    }

    void UIATT()
    {
        UIPVPManager.instance.SetWeaponUIData(this);
    }

    void WeaponRifle()
    {
        if (Input.GetMouseButton(0))
        {
            if (Time.time - firerate > nextfire)
                nextfire = Time.time - Time.deltaTime;

            if (WeaponOtherAnim())
                return;

            while (nextfire < Time.time)
            {
                SEND_SHOOT.SEND_WEAPON_SHOOT(_client.WeaponID, SpawnBullet.transform.position, SpawnBullet.transform.rotation);

                nextfire = Time.time + firerate;
                _weaponRecoil.Fire();

                firing = true;
            }
        }
        else
            firing = false;
    }

    void WeaponSniper()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time - firerate > nextfire)
                nextfire = Time.time - Time.deltaTime;

            if (WeaponOtherAnim())
                return;

            while (nextfire < Time.time)
            {
                SEND_SHOOT.SEND_WEAPON_SHOOT(_client.WeaponID, SpawnBullet.transform.position, SpawnBullet.transform.rotation);

                nextfire = Time.time + firerate;
                _weaponRecoil.Fire();
            }

            if (CrossHair == true)
                StartCoroutine(PosFire());
        }

    }

    void WeaponPistol()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time - firerate > nextfire)
                nextfire = Time.time - Time.deltaTime;


            if (WeaponOtherAnim())
                return;

            while (nextfire < Time.time)
            {
                SEND_SHOOT.SEND_WEAPON_SHOOT(_client.WeaponID, SpawnBullet.transform.position, SpawnBullet.transform.rotation);

                nextfire = Time.time + firerate;
                _weaponRecoil.Fire();

                firing = true;
            }
        }
        else
            firing = false;
    }

    void WeaponKnife() 
    {
        bool mouse1 = Input.GetMouseButton(0);
        bool mouse2 = Input.GetMouseButton(1);

        if (mouse1 || mouse2)
        {
            if (Time.time - firerate > nextfire)
                nextfire = Time.time - Time.deltaTime;

            while (nextfire < Time.time)
            {
                if (mouse1)
                    SEND_SHOOT.SEND_KNIFE_SHOOT(_playerManager.WeaponID, 1, AlternativeTime);
                else
                    SEND_SHOOT.SEND_KNIFE_SHOOT(_playerManager.WeaponID, 2, AlternativeTime);

                nextfire = Time.time + firerate;
            }
        }
    }

    void WeaponGrenade()
    {
        bool mouse1 = Input.GetMouseButtonDown(0);
        bool mouse2 = Input.GetMouseButtonUp(0);
        if (mouse1 && AlternativeTime == 0)
        {
            SEND_SHOOT.SEND_GRENADE_SHOOT(_playerManager.WeaponID, AlternativeTime, null, true);
            AlternativeTime = 1;
        }

        if (mouse2 && AlternativeTime == 1)
        {
            Quaternion pos = SpawnBullet.parent.gameObject.transform.rotation;
            //pos = pos.normalized;
            float[] fpos = new float[4];
            fpos[0] = pos.x;
            fpos[1] = pos.y;
            fpos[2] = pos.z;
            fpos[3] = pos.w;

            Debug.LogErrorFormat("{0} {1} {2} {3} => {4}", fpos[0],fpos[1],fpos[2],fpos[3], pos);

            SEND_SHOOT.SEND_GRENADE_SHOOT(_playerManager.WeaponID, AlternativeTime, fpos);
            AlternativeTime = 0;
        }
    }
    bool WeaponOtherAnim()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Reload") || _animator.GetCurrentAnimatorStateInfo(0).IsName("Draw"))
            return true;

        return false;
    }

    bool iFire()
    {
        if (nextfire < Time.time)
            return true;

        return false;
    }

    IEnumerator Reload2()
    {
        yield return new WaitForSeconds(reloadTime);
        for (int i = 0; i < maxammoinpaint; i++)
        {
            if (ammosb > 0 && bullets < maxammoinpaint)
            {
                bullets++;
                ammosb--;
            }
            else
            {
                break;
            }
        }
    }

    IEnumerator PosFire()
    {
        CrossHair = false;
        UIPVPManager.instance.SetCrossHair(_weaponInfoData.CrossHair, false);
        SetCrossHair(true);

        yield return new WaitForSeconds(firerate);

        if (cancelPosShot == false)
        {
            CrossHair = true;
            UIPVPManager.instance.SetCrossHair(_weaponInfoData.CrossHair, true);
            SetCrossHair(false);
        }
        cancelPosShot = false;
    }
}

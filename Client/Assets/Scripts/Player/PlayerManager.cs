using AssetBundleManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : IsMine
{
    public static PlayerManager instance;

    public Transform OBJ1;
    public Transform OBJ2;
    [Space]
    public int id;
    public string username;
    [Space]
    [Header("Player Config")]
    public int ACPlayer = 0;
    public int HPPlayer = 100;
    [Space]
    public int WeaponID = 1;
    public int Kills = 0;
    public int Death = 0;
    public int IDKilled = 0;
    public int MyTeam = -1;

    public Vector3 position = Vector3.zero;

    public InfoWeapons _playerWeaponsManager;
    public PlayerController _playerController;
    public AnimController _animController;
    public PlayerWeaponsObjects _playerWeaponLocal;
    public PlayerNetwork _playerNetwork;
    public CameraController _cameraController;
    public SoundManager _soundManager;
    public Camera _MiniMapCam;
    //
    public CharacterController _characterController;
    public GameObject[] DisableComponentsTPV;
    public GameObject[] DisableComponentsFPV;
    public Transform[] upperBody;
    public Transform[] leggBody;
    //
    [Space]
    public Camera _camMainFPS;
    public Camera _camWeaponFPS;
    public Transform _FPSItems;
    public GameObject _FPSMainObject;

    private void Start()
    {
        _MiniMapCam.targetTexture = Resources.Load<RenderTexture>("MiniMap/Render/Map");
        _characterController = GetComponent<CharacterController>();

        if (instance == null)
            instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            QualitySettings.SetQualityLevel(0);
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            QualitySettings.SetQualityLevel(2);
        }

        if (Input.GetKeyDown(KeyCode.Tab))
            UIScoreBoard.instance.gameObject.SetActive(true);
        if (Input.GetKeyUp(KeyCode.Tab))
            UIScoreBoard.instance.gameObject.SetActive(false);
    }

    public void DesactiveItemsTPV()
    {
        for (int i = 0; i < DisableComponentsFPV.Length; i++)
        {
            DisableComponentsFPV[i].SetActive(false);
        }
    }

    public void DesactiveItemsFPV()
    {
        for (int i = 0; i < DisableComponentsTPV.Length; i++)
        {
            DisableComponentsTPV[i].SetActive(false);
        }
    }

    public void SpawnShoot(int weaponid, float veloc, Vector3 pos, Quaternion rot)
    {
        _animController._animatorTPS.StartAnim("Fire");
        ///////////////////////////
        var weapon = _playerWeaponLocal.GetWeaponById(weaponid);
        var wp = weapon.FpvModel.GetComponent<WeaponController>();
        GameObject bullet = wp.Bullet;

        GameObject spa = Instantiate(bullet, pos, rot);
        //CheckShoot(pos, rot, id, weaponid);
        spa.GetComponent<Bullet>().SetData(id, weaponid, veloc);

        _soundManager.WeaponShoot(_playerWeaponsManager.GetWeaponById(weaponid));

        wp.bullets--;
    }

    public void SpawnGrenade(float x, float y, float z, float w)
    {
        var weapon = _playerWeaponLocal.GetWeaponById(5);
        var wp = weapon.FpvModel.GetComponent<WeaponController>();
        GameObject grenade = wp.Bullet;

        Debug.LogError("SPAWN GRENADE DATA : " + username + " : FLOAT: (" + x + " " + y + " " + z + " " + w + ") ROT: " + wp.SpawnBullet.parent.rotation);

        GameObject spa = Instantiate(grenade, wp.SpawnBullet.position, new Quaternion(x, y, z, w));

        if (isMine())
            StartCoroutine(ChangeWeaponIEnumator(0.5f));
    }

    public void UISetHealth(int health)
    {
        HPPlayer = health;

        if (isMine())
        {
            UIPVPManager.instance.SetHP_AC(0, health);
        }
    }

    public void SetDeath()
    {
        _animController._animatorTPS.StartAnim("Death1");
        _animController._animatorTPS.StartAnim("Death2");

        if (isMine())
            StartCoroutine(Respawn());

        UISetHealth(0);
        _playerController.enabled = false;
        _FPSItems.gameObject.SetActive(false);
    }

    public void Alive()
    {
        _characterController.enabled = false;

        transform.position = RespawnManager.instance.GetRespawn(MyTeam);

        _characterController.enabled = true;

        _playerWeaponLocal.SetWeapon(WeaponID);
        _FPSItems.gameObject.SetActive(true);

        if (isMine())
            _playerController.enabled = true;
        else
            _characterController.enabled = false;

    }

    public void SetBoddyRotation()
    {
        foreach (Transform u in upperBody)
        {
            u.RotateAround(u.position, transform.right, _cameraController.verticalRotation);
        };
    }

    public void SetLeggyRotation(float rotation)
    {
        foreach (Transform u in leggBody)
        {
            u.RotateAround(u.position, transform.up, rotation);
        }
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(2);

        new SEND_RESPAWN();
    }

    IEnumerator ChangeWeaponIEnumator(float time)
    {
        yield return new WaitForSeconds(time);
        _playerNetwork.SendChangeWeapon(1);
    }

}

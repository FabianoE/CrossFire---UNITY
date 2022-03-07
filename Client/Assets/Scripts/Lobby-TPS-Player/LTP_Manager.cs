using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LTP_Manager : MonoBehaviour
{
    public static LTP_Manager instance;


    public int ID_Character = 1;
    public GameObject LTP_BL;
    public GameObject LTP_GR;
    public Transform SpawnCharacter;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        SpawnerCharacter();
    }

    public void SpawnerCharacter()
    {
        if (LTP_BL == null || LTP_GR == null)
        {
            LTP_BL = Instantiate(AssetBundleManager.ABManager.instance.LoadResourcesOBJ(ID_Character + "_BL"), SpawnCharacter);
            LTP_GR = Instantiate(AssetBundleManager.ABManager.instance.LoadResourcesOBJ(ID_Character + "_GR"), SpawnCharacter);
        }
    }

    public void EnableWeapon(int id)
    {
        foreach (var obj in LTP_BL.GetComponent<LTP_Player>().Weapons)
        {
            if (obj.ID == id)
                obj.Weapon.SetActive(true);
            else
                if (obj.Weapon.active)
                obj.Weapon.SetActive(false);

        }
        foreach (var obj in LTP_GR.GetComponent<LTP_Player>().Weapons)
        {
            if (obj.ID == id)
                obj.Weapon.SetActive(true);
            else
                if (obj.Weapon.active)
                obj.Weapon.SetActive(false);

        }
    }

    public void EnablePerson(int person)
    {
        if(person == 1)
        {
            LTP_BL.SetActive(true);
            LTP_GR.SetActive(false);
        }
        else
        {
            LTP_GR.SetActive(true);
            LTP_BL.SetActive(false);
        }
    }
}

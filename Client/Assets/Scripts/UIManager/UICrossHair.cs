using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICrossHair : MonoBehaviour
{
    public static UICrossHair instance;
    [Space]
    [Space(5)]
    [Header("CrossHair")]
    public RectTransform DynamicCrossHair;
    public Image[] CrossHairPositions;
    [Space]
    public float IdleHair, WalkHair, FireHair;
    public float Crouch;
    public bool walking = false;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("INSTANCE EXIST");
            Destroy(this);
        }
    }

    void Update()
    {
        Vector2 size = (walking) ? new Vector2(WalkHair, WalkHair) : new Vector2(IdleHair, IdleHair);
        DynamicCrossHair.sizeDelta = size;
    }

    public void SetWeapon(WeaponsInfo info)
    {
        IdleHair = info.IdleHair;
        WalkHair = info.WalkHair;
        FireHair = info.FireHair;
    }

    public void OnFire(bool firing)
    {
        if (!firing)
            return;
        
        Vector2 size = new Vector2(FireHair, FireHair);
        DynamicCrossHair.sizeDelta = size;
    }

    public void OnHitMarker()
    {
        StopCoroutine("HitMarker");
        StartCoroutine("HitMarker");
    }

    IEnumerator HitMarker()
    {
        for(int i = 0; i < CrossHairPositions.Length; i++)
        {
            CrossHairPositions[i].color = Color.red;
        }

        yield return new WaitForSeconds(0.08f);

        for (int i = 0; i < CrossHairPositions.Length; i++)
        {
            CrossHairPositions[i].color = Color.green;
        }
    }
}

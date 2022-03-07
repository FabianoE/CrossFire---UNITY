using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagItem : MonoBehaviour
{
    public Text item_name;
    public Image item_image;

    public void SetData(WeaponsInfo weaponsInfo)
    {
        if(weaponsInfo.Name == "")
        {
            this.gameObject.SetActive(false);
            return;
        }

        item_name.text = weaponsInfo.Name;
        item_image.sprite = Resources.Load<Sprite>("WeaponIcons/LOBBY_"+weaponsInfo.Resource);
    }
}

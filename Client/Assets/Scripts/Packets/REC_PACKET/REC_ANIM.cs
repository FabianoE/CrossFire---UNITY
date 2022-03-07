using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using UnityEngine;

public class REC_ANIM : PacketHandle
{
    public override void Handler()
    {
        int _fromClient = _packet.ReadInt();
        int pack = _packet.ReadInt();

        if (!GameManager.players.ContainsKey(_fromClient))
            return;

        switch (pack)
        {
            case 105:
                string upperbody = _packet.ReadString();
                int _walk = _packet.ReadInt();
                int leggybody = _packet.ReadInt();
                bool _walking = _packet.ReadBool();
                bool _lower = _packet.ReadBool();
                bool _shift = _packet.ReadBool();

                int status = 2;
                if (_walk > 0 && _lower == false && _shift == false)
                    status = 1;


                var pManager = GameManager.players[_fromClient].GetComponent<PlayerManager>();
                GameManager.players[_fromClient].GetComponent<AnimController>().SetWalk(upperbody, _walking, _walk, leggybody, _lower);

                if (pManager.id != Client.instance.myId)
                {
                    float angle = Mathf.Clamp(float.Parse(upperbody), -85, 85);
                    Quaternion final = Quaternion.Euler(angle, 0f, 0f);
                    pManager._camMainFPS.transform.rotation = Quaternion.Slerp(pManager._camMainFPS.transform.rotation, final, Time.deltaTime * 2);
                }

                pManager._playerWeaponLocal.SetIntWeaponAnim("Walk", status);
                pManager._soundManager.FootStepSound(_lower, _walking, _shift);
                break;
            case 106:
                var pMananger = GameManager.players[_fromClient].GetComponent<PlayerManager>();
                pMananger._animController._animatorTPS.StartAnim("reload");
                pMananger._playerWeaponLocal.StartWeaponAnim("Reload");
                pMananger._playerWeaponLocal.GetWeaponById(pMananger.WeaponID).FpvModel.GetComponent<WeaponController>().WeaponReloaded();
                pMananger._soundManager.ReloadWeapon(pMananger._playerWeaponsManager.GetWeaponById(pMananger.WeaponID));
                break;
        }
    }
}

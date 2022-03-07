using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : IsMine
{
    public PlayerManager player;
    public float sensitivity = 100f;
    public float clampAngle = 85f;

    public float verticalRotation;
    public float horizontalRotation;

    public string NamePlayer;

    private void Start()
    {
        if (!isMine())
            this.gameObject.SetActive(false);

        verticalRotation = transform.localEulerAngles.x;
        horizontalRotation = player.transform.eulerAngles.y;
    }

    private void Update()
    {
        if (Pvp_ChatController.instance.PVP_InputText.IsActive())
            return;

        Look();
        Debug.DrawRay(transform.position, transform.forward * 2, Color.red);

        if (isMine())
            RayCam();

    }

    void OnGUI()
    {
        GUI.contentColor = Color.red;
        if (NamePlayer != null)
            GUI.Label(new Rect((Screen.width / 2) - 20, (Screen.height / 2) + 50, 100, 30), NamePlayer);
    }

    private void RayCam()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            BulletPlayerCollider coll = hit.collider.GetComponent<BulletPlayerCollider>();
            if (coll != null)
            {
                if (coll.playerManager.MyTeam != Client.instance.myTeam || coll.playerManager.MyTeam == 3)
                    NamePlayer = coll.playerManager.username;
            }
            else
                NamePlayer = null;
        }
    }

    private void Look()
    {
        float _mouseVertical = -Input.GetAxis("Mouse Y");
        float _mouseHorizontal = Input.GetAxis("Mouse X");

        verticalRotation += _mouseVertical * sensitivity * Time.fixedDeltaTime;
        horizontalRotation += _mouseHorizontal * sensitivity * Time.fixedDeltaTime;

        verticalRotation = Mathf.Clamp(verticalRotation, -clampAngle, clampAngle);

        transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        player.transform.rotation = Quaternion.Euler(0f, horizontalRotation, 0f);
    }
}

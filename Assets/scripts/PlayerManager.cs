using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerManager : MonoBehaviour
{
    PhotonView PV;

    GameObject Controller;
    void Awake()
    {
        Cursor.visible = false;
        PV = GetComponent<PhotonView>();
    }
    void Start()
    {
        if (PV.IsMine)
        {
            CreateController();
        } 
    }

    void CreateController()
    {
        Transform SpawnPoint = SpawnManager.Instance.GetSpawnpoint();
        Controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), SpawnPoint.position, SpawnPoint.rotation ,0,new object[] {PV.ViewID  });
    }
    public void Die()
    {
        PhotonNetwork.Destroy(Controller);
        CreateController();
    }
}

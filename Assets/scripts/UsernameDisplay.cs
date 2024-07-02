using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class UsernameDisplay : MonoBehaviour
{
    [SerializeField] PhotonView PlayerPV;
    [SerializeField] TMP_Text text;
    void Start()
    {
        if (PlayerPV.IsMine)
        {
            gameObject.SetActive(false);
        }
        text.text = PlayerPV.Owner.NickName;
    }
}

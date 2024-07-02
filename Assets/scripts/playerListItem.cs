using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using TMPro;

public class playerListItem : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text text;
    Player Player;
    public void SetUp(Player _player)
    {
        Player = _player;
        text.text = _player.NickName;
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (Player == otherPlayer)
        {
            Destroy(gameObject);
        }
    }
    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }
}

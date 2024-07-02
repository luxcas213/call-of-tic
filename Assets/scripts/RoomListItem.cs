using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
public class RoomListItem : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    public RoomInfo Info;
    public void SetUp(RoomInfo info) 
    {
        Info = info;
        text.text = info.Name;
    }
    public void Onclick()
    {
        launcher.instance.JoinRoom(Info);
    }
}

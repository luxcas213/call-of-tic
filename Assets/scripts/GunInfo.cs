using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="FPS/New Gun")]
public class GunInfo : ItemInfo
{
    public float Damege;
    public float MaxDistance;
    public int bullets;
    public float reloadTime;
}

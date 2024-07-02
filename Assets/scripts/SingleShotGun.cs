using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SingleShotGun : Gun
{
    [SerializeField] Camera cam;

    [SerializeField] GameObject crosshair;
    private bool canshoot = true;

    PhotonView PV;
    public override void Use()
    {
        if (canshoot == true)
        {
            Shoot();
            Debug.Log("usando arma " + itemInfo.ItemName);
        }
    }
    void Awake()
    {
        PV = GetComponent<PhotonView>();    
    }
    void Shoot()
    {
        canshoot = false;

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        ray.origin = cam.transform.position;

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log("le pegaste a " + hit.collider.gameObject.name);

            if (hit.collider.gameObject.GetComponent<IDamagable>() != null)
            {
                hit.collider.gameObject.GetComponent<IDamagable>().TakeDamage(((GunInfo)itemInfo).Damege);
                StartCoroutine(crosshairtime());
            }
            PV.RPC("RPC_Shoot", RpcTarget.All, hit.point);
        }
        StartCoroutine(cooldown());
    }
    [PunRPC]
    void RPC_Shoot(Vector3 HitPotition)
    {
        GameObject a= Instantiate(BulletImpactPrefab, HitPotition, Quaternion.identity);
        Destroy(a, 0.3f);
    }
    IEnumerator crosshairtime()
    {
        crosshair.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        crosshair.SetActive(false);
    }
    IEnumerator cooldown()
    {
        yield return new WaitForSeconds(itemInfo.cooldown);

        canshoot = true;
    }
}

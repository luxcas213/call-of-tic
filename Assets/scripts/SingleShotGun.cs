using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
public class SingleShotGun : Gun
{
    [SerializeField] Camera cam;
    [SerializeField] GameObject crosshair;
    [SerializeField] TMP_Text balas_text;
    private bool canshoot = true;
    private bool canreload = true;
    private int cantmaxbalas;
    private int cantactbalas;
    private float reloadtime;
    PhotonView PV;
    
    public override void Use()
    {
        if (canshoot == true)
        {
            if (cantactbalas <= 0)
            {
                if(canreload==true) StartCoroutine(Reloadcooldown());
                canreload = false;
                return;
            }
            cantactbalas -= 1;
            Shoot();
            actBalas();
            Debug.Log("usando arma " + itemInfo.ItemName);
        }
    }
    void Awake()
    {
        PV = GetComponent<PhotonView>();
        cantmaxbalas = ((GunInfo)itemInfo).bullets;
        cantactbalas = cantmaxbalas;
        reloadtime = ((GunInfo)itemInfo).reloadTime;
        actBalas();
    }
    private void Start()
    {
        actBalas();
    }
    void Shoot()
    {
        canshoot = false;

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        ray.origin = cam.transform.position;
        float dist = ((GunInfo)itemInfo).MaxDistance;
        if (Physics.Raycast(ray, out RaycastHit hit,dist))
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
    IEnumerator Reloadcooldown()
    {
        yield return new WaitForSeconds(reloadtime);
        cantactbalas = cantmaxbalas;
        actBalas();
        canreload = true;
    }
    public void actBalas()
    {
        balas_text.text = "Balas = " + cantactbalas + "/" + cantmaxbalas; 
    }
}

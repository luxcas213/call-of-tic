using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;
using UnityEngine.UI;

public class PlayerController : MonoBehaviourPunCallbacks , IDamagable
{
    [SerializeField] Image barradevida;
    [SerializeField] GameObject ui;

    [SerializeField] GameObject CameraHolder;

    [SerializeField] float MouseSensitivity, sprintSpeed, WalkSpeed, JumpForce, smoothTime;

    [SerializeField] item[] items;

    int ItemIndex;
    int PreviousItemIndex=-1;


    float VerticalLookRotation;
    bool grounded;

    Vector3 SmoothMoveVelocity;
    Vector3 MoveAmount;

    Rigidbody RB;

    PhotonView PV;

    const float MaxHealth=100f;
    float CurrentHealt= MaxHealth;

    PlayerManager playerManager;

    private bool IsShoting;

    void Awake()
    {
        RB = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();

        playerManager =PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();
    }
    void Start()
    {
        if (PV.IsMine)
        {
            EquipItem(0);
        }
        else
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(RB);
            Destroy(ui);
        }
    }
    void Update()
    {
        if (!PV.IsMine)
            return;
        Look();

        Move();

        Jump();

        for (int i = 0; i < items.Length; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                EquipItem(i);
                break;
            }

        }
        if(Input.GetAxisRaw("Mouse ScrollWheel")>0f)
        {
            if (ItemIndex >= items.Length - 1)
            {
                EquipItem(0);
            }
            else
            {
                EquipItem(ItemIndex + 1);
            }
        }
        else if(Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
        { 
            if (ItemIndex <= 0)
            {
                EquipItem(items.Length - 1);
            }
            else
            {
                EquipItem(ItemIndex - 1);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            IsShoting = true;
        }
        if (Input.GetMouseButtonUp(0))
        {

            IsShoting = false;
        }
        if (IsShoting)
        {
            items[ItemIndex].Use();
        }


        if (transform.position.y < -10f)
        {
            Die();
        }
    }
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            RB.AddForce(transform.up * JumpForce);
        }
    }
    void EquipItem(int _index)
    {
        if (_index == PreviousItemIndex)
            return;
        ItemIndex = _index;

        items[ItemIndex].itemGameobjet.SetActive(true);

        if (PreviousItemIndex != -1)
        {
            items[PreviousItemIndex].itemGameobjet.SetActive(false);
        }

        PreviousItemIndex = ItemIndex;

        if (PV.IsMine)
        {
            Hashtable hash = new Hashtable();
            hash.Add("itemIndex", ItemIndex);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }

    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if(!PV.IsMine && targetPlayer == PV.Owner)
        {
            EquipItem((int)changedProps["itemIndex"]);
        }
    }
    void Move()
    {
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        MoveAmount = Vector3.SmoothDamp(MoveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : WalkSpeed), ref SmoothMoveVelocity, smoothTime);
    }

    void Look()
    {
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * MouseSensitivity);

        VerticalLookRotation += Input.GetAxisRaw("Mouse Y") * MouseSensitivity;
        VerticalLookRotation = Mathf.Clamp(VerticalLookRotation, -90f, 90f);


        CameraHolder.transform.localEulerAngles = Vector3.left * VerticalLookRotation;
    }

    public void SetGroundedState(bool _grounded)
    {
        grounded = _grounded;
    }

    void FixedUpdate()
    {
        if (!PV.IsMine)
            return;
        RB.MovePosition(RB.position + transform.TransformDirection(MoveAmount) * Time.deltaTime);
    }
    public void TakeDamage(float Damage)
    {
        PV.RPC("RPC_TakeDamage", RpcTarget.All, Damage);
    }
    [PunRPC]
    void RPC_TakeDamage(float Damage)
    {
        if (!PV.IsMine)
            return;

        CurrentHealt -= Damage;

        barradevida.fillAmount = CurrentHealt / MaxHealth;
        if (CurrentHealt <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        playerManager.Die();
    }
}

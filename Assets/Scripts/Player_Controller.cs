using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    [Header("Character Settings")]
    public float moveSpeed = 5f;

    // References
    [Header("References")]
    public GameObject playerBody;
    public GameObject playerAttackZone;
    CharacterController _con;
    Animator _anim;

    float _inX, _inY; // Input floats

    // Movement controls
    Vector3 _mov;
    bool _isAttacking;

    // Rotation controls
    private Quaternion _targetRotation;
    Vector3 _mousePosition;

    // Animation controls
    float _animMoveSpeed;

    // Follow Mouse controls
    private Vector3 _inputRotation;
    private Vector3 _mousePlacement;
    private Vector3 _screenCentre;

    private void Awake()
    {
        _con = GetComponent<CharacterController>();
        _anim = playerBody.GetComponent<Animator>();
    }

    void Update()
    {
        ReadInputs();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void LateUpdate()
    {
        Animations();
    }

    void ReadInputs()
    {
        if(GameManager.instance.playerHealth >= 0)
        {
            _inX = Input.GetAxis("Horizontal");
            _inY = Input.GetAxis("Vertical");

            if (_anim.GetCurrentAnimatorClipInfo(1)[0].clip.name == "Rogalic_idle" &&
                Input.GetButtonDown("Fire1"))
            {
                _isAttacking = true;
            }
      }
        else
        {
            _inX = _inY = 0f;
            _isAttacking = false;
        }
    }

    void Movement()
    {
        _mov = new Vector3(_inX, 0f, _inY) * moveSpeed * Time.deltaTime;

        Vector3 moveDir = transform.position + new Vector3(_inX, 0f, _inY);
        Vector3 targetDirection = moveDir - transform.position;

        if (_mov.magnitude > 0)
        {
            _targetRotation = Quaternion.LookRotation(targetDirection);
            Quaternion nextRotation = Quaternion.Lerp(playerBody.transform.localRotation, _targetRotation, 10f * Time.deltaTime);
            if(!_isAttacking)
                playerBody.transform.localRotation = nextRotation;
            _animMoveSpeed = 1f;
        }
        else
        {
            _animMoveSpeed = 0f;
        }

        _con.Move(_mov);

    }

    void Animations()
    {
        _anim.SetFloat("Movement", _con.velocity.magnitude);

        if (_isAttacking)
        {
            _anim.SetTrigger("Attack");
            _isAttacking = false;
            _mousePosition = Input.mousePosition;
        }
        if (_anim.GetCurrentAnimatorClipInfo(1)[0].clip.name == "Rogalic_attack_1")
        {
            LookAtMouse();
        }
    }

    void LookAtMouse()
    {
        _screenCentre = new Vector3(Screen.width * 0.5f, 0, Screen.height * 0.5f);
        _mousePlacement = _mousePosition;
        _mousePlacement.z = _mousePlacement.y;
        _mousePlacement.y = 0;
        _inputRotation = _mousePlacement - _screenCentre;
        playerBody.transform.rotation = Quaternion.LookRotation(_inputRotation);
    }


    public void MakeAttack()
    {
        Vector3 attackPosition = transform.position + playerBody.transform.forward;
        Instantiate(playerAttackZone, attackPosition,Quaternion.identity);
    }
}

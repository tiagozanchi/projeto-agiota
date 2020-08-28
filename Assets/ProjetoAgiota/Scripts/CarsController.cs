﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarsController : MonoBehaviour
{
    [SerializeField]
    private GameObject _nitroParticles;
    [SerializeField]
    private Material[] _carMaterialsColors;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private CarColors _color;
    public CarColors Color
    {
        get => _color;
    }

    private Vector3 _targetPosition = Vector3.zero;
    private Quaternion _defaultRotation;
    private Rigidbody _rb;
    private Animator _animator;
    private AudioSource _audioSource;
    private float step = 0f;
    private float _shakeAmount;
    private float _luckAmount;
    private float _secondsToTryNewPos;
    private float _timeArrivedTargetPos;
    private bool _canLookForNewPos = true;
    private bool _hasArrivedTarget = false;
    private bool _finishingRace = false;
    private bool _isMissionCar = false;
    private int _expectedPosition = -1;
    private const float _damageRecoveryTime = 3f;

    // Start is called before the first frame update
    void Awake()
    {
        _shakeAmount = Random.Range(-0.003f, 0.003f);
        _defaultRotation = transform.rotation;
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.RaceStarted || _finishingRace)
        {
            if (Vector3.Distance(transform.position, _targetPosition) > 5f)
            {
                _rb.velocity = Vector3.zero;
                _rb.MovePosition(transform.position + ((_targetPosition - transform.position) *_speed * Time.deltaTime));
            }
            else if (!_hasArrivedTarget)
            {
                _hasArrivedTarget = true;
                _timeArrivedTargetPos = Time.time;
            }

            if (transform.rotation != _defaultRotation)
            {
            transform.rotation = Quaternion.Lerp(transform.rotation, _defaultRotation, step*Time.deltaTime);
            step += .5f;
            }
            else
            {
                step = 0f;
            }

            _rb.position += Vector3.right * (Mathf.Sin(Time.time) * _shakeAmount);

            if (Time.time > _timeArrivedTargetPos+_secondsToTryNewPos && _hasArrivedTarget && _canLookForNewPos) TryNewPos();   
        }
    }

    public void StartRacing(float luckAmount, float secondsToTryNewPos)
    {
        GameManager.Instance.SoundManager.Play(SoundManager.Sounds.CarEngine, _audioSource);
        _secondsToTryNewPos = secondsToTryNewPos;
        _luckAmount = luckAmount;
        _targetPosition = transform.position;
    }

    private void TryNewPos()
    {
        if (_finishingRace)
        {
            _finishingRace = false;
            return; 
        }

        if(_isMissionCar && _expectedPosition == GetMyCurrentPosition()) return;

        bool forward = Random.Range(0, 10) > 2f;
        GetNewTargetPosition(forward);
        _hasArrivedTarget = false;
    }

    private void GetNewTargetPosition(bool forward, float moveAmount) 
    {
        if (moveAmount <= 0) {
            moveAmount = Random.Range(5, 15);
        }
        float newZ;

        //Clamp para nao sair do mapa
        newZ = forward ? Mathf.Clamp(transform.position.z - moveAmount, -60f, -6f) : Mathf.Clamp(transform.position.z + moveAmount, -60f, -6f);

        _targetPosition = new Vector3(Random.Range(Mathf.Clamp(transform.position.x - 5f, -11, 11f), Mathf.Clamp(transform.position.x + 5f, -11, 11f)), transform.position.y, newZ);
    }

    private void GetNewTargetPosition(bool forward) {
        GetNewTargetPosition(forward, 0);
    }

    public void SetupCar(CarColors newColor, bool isMissionCar, int expectedPosition)
    {
        _isMissionCar = isMissionCar;
        _expectedPosition = expectedPosition; 
        _color = newColor;
        MeshRenderer[] carRenderers = GetComponentsInChildren<MeshRenderer>();
        
        foreach(Renderer r in carRenderers) r.material = _carMaterialsColors[(int)_color];
    }

    public void TakeDamage(float recoilDistance) 
    {
        if (!_canLookForNewPos) {
            return;
        }

        GameManager.Instance.ShakeCam(0.05f);
        GetNewTargetPosition(false, recoilDistance);
        _canLookForNewPos = false;
        _animator.SetTrigger("Crash");
        StartCoroutine(WaitToGetNewPos());
    }

    public void Boost()
    {
        GameManager.Instance.ShakeCam(0.3f);

        _nitroParticles.SetActive(true);
        _targetPosition = new Vector3(Random.Range(Mathf.Clamp(transform.position.x - 5f, -11, 11f), Mathf.Clamp(transform.position.x + 5f, -11, 11f)), transform.position.y, -70f);
        _canLookForNewPos = false;
        StartCoroutine(WaitToGetNewPos());
    }

    private IEnumerator WaitToGetNewPos() 
    {
        yield return new WaitForSeconds(_damageRecoveryTime);
        _nitroParticles.SetActive(false);
        _canLookForNewPos = true;
    }

    public void FinishRacing()
    {
        _canLookForNewPos = false;
        _finishingRace = true;
        _speed = 5f;
        _targetPosition = _targetPosition = new Vector3(Random.Range(Mathf.Clamp(transform.position.x - 5f, -11, 11f), Mathf.Clamp(transform.position.x + 5f, -11, 11f)), transform.position.y, -100f);
        _audioSource.Stop();
    }

    private int GetMyCurrentPosition()
    {
        return GameManager.Instance.PositionController.GetCarPosition(this);
    }
}

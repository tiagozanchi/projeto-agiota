using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarsController : MonoBehaviour
{
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
    private float step = 0f;
    private float _shakeAmount;
    private float _luckAmount;
    private float _secondsToTryNewPos;
    private float _timeArrivedTargetPos;
    private bool _canLookForNewPos = true;
    private bool _hasArrivedTarget = false;
    private const float _damageRecoveryTime = 3f;

    // Start is called before the first frame update
    void Awake()
    {
        _shakeAmount = Random.Range(-0.003f, 0.003f);
        _defaultRotation = transform.rotation;
        _rb = GetComponent<Rigidbody>();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.RaceStarted) return;
        
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

    public void StartRacing(float luckAmount, float secondsToTryNewPos)
    {
        _secondsToTryNewPos = 4f;//secondsToTryNewPos;
        _luckAmount = luckAmount;
        _targetPosition = transform.position;
    }

    private void TryNewPos()
    {
        bool forward = Random.Range(0, 10) > 5f;
        GetNewTargetPosition(forward);
        _hasArrivedTarget = false;
    }

    private void GetNewTargetPosition(bool forward) {

        float moveAmount = Random.Range(5, 15);
        float newZ;

        //Clamp para nao sair do mapa
        newZ = forward ? Mathf.Clamp(transform.position.z - moveAmount, -60f, -6f) : Mathf.Clamp(transform.position.z + moveAmount, -60f, -6f);

        _targetPosition = new Vector3(Random.Range(Mathf.Clamp(transform.position.x - 5f, -11, 11f), Mathf.Clamp(transform.position.x + 5f, -11, 11f)), transform.position.y, newZ);
    }

    public void SetColor(CarColors newColor)
    {
        _color = newColor;
        MeshRenderer[] carRenderers = GetComponentsInChildren<MeshRenderer>();
        
        foreach(Renderer r in carRenderers) r.material = _carMaterialsColors[(int)_color];
    }

    public void TakeDamage() 
    {
        GetNewTargetPosition(false);
        _canLookForNewPos = false;
        StartCoroutine(recoverFromDamage());
    }

    private IEnumerator recoverFromDamage() 
    {
        yield return new WaitForSeconds(_damageRecoveryTime);
        _canLookForNewPos = true;
    }
}

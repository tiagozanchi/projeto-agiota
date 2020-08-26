using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _trafficLight;
    [SerializeField]
    private GameObject[] _cars;
    private static GameManager _instance = null;
    public static GameManager Instance
    {
        get => _instance;
    }

    [SerializeField]
    private CarsController[] _carsInTrack;
    public CarsController[] CarsInTrack
    {
        get => _carsInTrack;
    }
    [SerializeField]
    private PositionInTrack _positionController;
    private CameraController _cameraController;
    private int _followingCarIndex = 0;

    private bool _raceStarted = false;
    public bool RaceStarted
    {
        get => _raceStarted;
    }

    private Mission _currentMission;
    void Awake()
    {
        _cameraController = Camera.main.GetComponent<CameraController>();
        if (_instance != null && _instance != this) 
        {
            Destroy(this.gameObject);
        }

        _instance = this;
        DontDestroyOnLoad( this.gameObject );
    }

    void Start()
    {
        _currentMission = new Mission(8, 7500, CarColors.Blue, 3);
        _carsInTrack = new CarsController[_currentMission.NumberOfCars];
        SpawnCars();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PrepareToStart();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _followingCarIndex -= 1;

            if (_followingCarIndex == -1) _followingCarIndex = _carsInTrack.Length-1;
            _cameraController.CarToFollow = _carsInTrack[_followingCarIndex].transform;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _followingCarIndex += 1;

            if (_followingCarIndex == _carsInTrack.Length) _followingCarIndex = 0;
            _cameraController.CarToFollow = _carsInTrack[_followingCarIndex].transform;
        }
    }
    
    private void SpawnCars()
    {
        float spawnZ = -60;
        float spawnX = 15;
        for(int i = 1; i <= _currentMission.NumberOfCars; i++)
        {
            _carsInTrack[i-1] = Instantiate(_cars[Random.Range(0,_cars.Length)], new Vector3(spawnX-i*5, 0.44f, spawnZ), Quaternion.identity).GetComponent<CarsController>();
            CarColors randomColor = (CarColors)Random.Range(0, 5);
            _carsInTrack[i-1].SetColor(randomColor);

            if (i % 5 == 0)
            {
                spawnX += 25; 
                spawnZ += 13f;
            }
        }
    }

    private void PrepareToStart()
    {
        _trafficLight.SetActive(true);
        Invoke("StartRace", 4f);
    }

    private void StartRace()
    {
        if (_raceStarted) return;

        _positionController.RaceTrackLength = _currentMission.TrackLength;
        _cameraController.CarToFollow = _carsInTrack[_followingCarIndex].transform;
        Randomizer.Randomize(_carsInTrack);
        for(int i = 0; i < _carsInTrack.Length; i++)
        {
            _carsInTrack[i].StartRacing(Random.Range(0,10), Random.Range(3,8));
        }
        _raceStarted = true;
    }

    public void FinishRace(CarsController firstPlace)
    {
        _cameraController.CarToFollow = firstPlace.transform;
        _raceStarted = false;
    }

    public Color GetColor(CarColors carColor)
    {
        switch (carColor)
        {
            case CarColors.Black:
                return Color.black;
            case CarColors.Blue:
                return Color.blue;
            case CarColors.Green:
                return Color.green;
            case CarColors.Red:
                return Color.red;
            case CarColors.White:
                return Color.white;
            case CarColors.Yellow:
                return Color.yellow;
            default:
                return Color.magenta;
        }
    }
}

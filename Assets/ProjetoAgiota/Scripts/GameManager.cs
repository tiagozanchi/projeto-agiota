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
    
    private CarsController[] _carsInMission;

    [SerializeField]
    private PositionInTrack _positionController;
    [SerializeField]
    private SoundManager _soundManager;
    public SoundManager SoundManager
    {
        get => _soundManager;
    }
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
        _soundManager.Play(SoundManager.Sounds.BGM_Gameplay,_soundManager.GetComponent<AudioSource>());
        _currentMission = new Mission(15, 1000, CarColors.Blue, 3);
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
        bool missionColorWasSpawned = false;
        int numberOfMissionCars = 0;
        for(int i = 1; i <= _currentMission.NumberOfCars; i++)
        {
            _carsInTrack[i-1] = Instantiate(_cars[Random.Range(0,_cars.Length)], new Vector3(spawnX-i*5, 0.44f, spawnZ), Quaternion.identity).GetComponent<CarsController>();
            CarColors randomColor = (CarColors)Random.Range(0, 5);
            _carsInTrack[i-1].SetColor(randomColor);

            if (randomColor == _currentMission.TrackedCarColor)
            {
                numberOfMissionCars++;
                missionColorWasSpawned = true;
            }

            if (i == _currentMission.NumberOfCars && !missionColorWasSpawned)
            {
                numberOfMissionCars++;
                randomColor = _currentMission.TrackedCarColor;
            }

            _carsInTrack[i-1].SetColor(randomColor);

            if (i % 5 == 0)
            {
                spawnX += 25; 
                spawnZ += 13f;
            }
        }

        _carsInMission = new CarsController[numberOfMissionCars];
        int index = 0;
        for (int i = 0; i < _carsInTrack.Length; i++)
        {
            if (_carsInTrack[i].Color == _currentMission.TrackedCarColor)
            {
                _carsInMission[index] = _carsInTrack[i];
                index++;
            } 
        }
    }

    private void PrepareToStart()
    {
        _soundManager.Play(SoundManager.Sounds.Largada, GetComponent<AudioSource>());
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
        bool missionComplete = false;

        foreach(CarsController car in _carsInMission)
        {
            if (_positionController.GetCarPosition(car) == _currentMission.TrackedCarPosition) missionComplete = true;
        }

        Debug.Log("Mission complete: "+missionComplete);
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

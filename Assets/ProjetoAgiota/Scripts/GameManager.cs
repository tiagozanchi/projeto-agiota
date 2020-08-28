using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _trafficLight;
    [SerializeField]
    private GameObject _finishLine;
    [SerializeField]
    private TMPro.TextMeshProUGUI _missionText;
    [SerializeField]
    private GameObject _endMissionPanel;
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
    public PositionInTrack PositionController
    {
        get => _positionController;
    }
    [SerializeField]
    private SoundManager _soundManager;
    public SoundManager SoundManager
    {
        get => _soundManager;
    }
    [SerializeField]
    private WeaponManager _weaponManager;
    public WeaponManager WeaponManager 
    {
        get => _weaponManager;
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
        //DontDestroyOnLoad( this.gameObject );
    }

    void Start()
    {
        _soundManager.Play(SoundManager.Sounds.BGM_Gameplay,_soundManager.GetComponent<AudioSource>());
        int randomCarAmount = Random.Range(6,10);
        _currentMission = new Mission(randomCarAmount, 500, (CarColors)Random.Range(0,5), Random.Range(1, randomCarAmount+1));
        _carsInTrack = new CarsController[_currentMission.NumberOfCars];
        _missionText.text = string.Format("Um carro <color=#{0}>{1}</color> deve chegar na {2}ª posição!", ColorUtility.ToHtmlStringRGB(GetColor(_currentMission.TrackedCarColor)), _currentMission.TrackedCarColor.ToString(), _currentMission.TrackedCarPosition);
        _missionText.transform.parent.gameObject.SetActive(true);
        SpawnCars();
    }
    
    private void SpawnCars()
    {
        float spawnZ = -60;
        float spawnX = 15;
        bool missionColorWasSpawned = false;
        int numberOfMissionCars = 0;
        for(int i = 1; i <= _currentMission.NumberOfCars; i++)
        {
            CarsController controller = Instantiate(_cars[Random.Range(0, _cars.Length)], new Vector3(spawnX - i * 5, 0.44f, spawnZ), Quaternion.identity).GetComponent<CarsController>();
            controller.gameObject.name = RiderNames.getRandomRiderName();
            _carsInTrack[i - 1] = controller;
            CarColors randomColor = (CarColors)Random.Range(0, 5);
            bool isMissionCar = false;

            if (randomColor == _currentMission.TrackedCarColor)
            {
                numberOfMissionCars++;
                missionColorWasSpawned = true;
                isMissionCar = true;
            }

            if (i == _currentMission.NumberOfCars && !missionColorWasSpawned)
            {
                numberOfMissionCars++;
                randomColor = _currentMission.TrackedCarColor;
                isMissionCar = true;
            }

            _carsInTrack[i-1].SetupCar(randomColor, isMissionCar, _currentMission.TrackedCarPosition);

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

    public void PrepareToStart()
    {
        //kkkkk me desculpa por isso mas nao posso perder tempo
        _missionText.transform.parent.parent.parent.gameObject.SetActive(false);
        _soundManager.Play(SoundManager.Sounds.Largada, GetComponent<AudioSource>());
        _trafficLight.SetActive(true);
        Invoke("StartRace", 4f);
    }

    private void StartRace()
    {
        if (_raceStarted) return;

        _positionController.RaceTrackLength = _currentMission.TrackLength;
        WeaponManager.Init(GetComponents<WeaponController>());
        Randomizer.Randomize(_carsInTrack);
        for(int i = 0; i < _carsInTrack.Length; i++)
        {
            _carsInTrack[i].StartRacing(Random.Range(0,10), Random.Range(4,6));
        }
        _raceStarted = true;
    }

    public void FinishRace(CarsController firstPlace)
    {
        bool missionComplete = false;
        _finishLine.SetActive(true);

        foreach(CarsController car in _carsInMission)
        {
            if (_positionController.GetCarPosition(car) == _currentMission.TrackedCarPosition) missionComplete = true;
        }

        for(int i = 0; i < _carsInTrack.Length; i++)
        {
            _carsInTrack[i].FinishRacing();
        }

        StartCoroutine(SetupEndMissionPanel(missionComplete));
        _raceStarted = false;
    }

    public void ShakeCam(float amount)
    {
        _cameraController.shakeDuration = amount;
    }

    public static Color GetColor(CarColors carColor)
    {
        switch (carColor)
        {
            case CarColors.Preto:
                return Color.black;
            case CarColors.Azul:
                return Color.blue;
            case CarColors.Verde:
                return Color.green;
            case CarColors.Vermelho:
                return Color.red;
            case CarColors.Branco:
                return Color.white;
            case CarColors.Amarelo:
                return Color.yellow;
            default:
                return Color.magenta;
        }
    }

	public int GetNumberOfCars()
    {
        if (_currentMission != null) 
        {
            return _currentMission.NumberOfCars;
        } 
        return -1;
    }

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    private IEnumerator SetupEndMissionPanel(bool missionComplete)
    {   
        yield return new WaitForSeconds(1f);
        int conseqWins = missionComplete ? PlayerPrefs.GetInt("ConseqWins")+1 : PlayerPrefs.GetInt("ConseqWins");
        
        if (missionComplete) PlayerPrefs.SetInt("ConseqWins",conseqWins);

        TMPro.TextMeshProUGUI[] panelTexts = _endMissionPanel.GetComponentsInChildren<TMPro.TextMeshProUGUI>();
        panelTexts[0].text = missionComplete ? "Missão completa!" : "Missão fracassada!";
        panelTexts[1].text = missionComplete ? "Continuar" : "Tentar Novamente";
        panelTexts[2].text = "Sair";
        panelTexts[3].text = "Missões consecutivas sem fracassar: "+conseqWins.ToString();
        
        _endMissionPanel.SetActive(true);
	}

    public int GetCarMissionStats(CarsController car, int currentPosition)
    {
        if (car.Color == _currentMission.TrackedCarColor)
        {
            if (currentPosition > _currentMission.TrackedCarPosition)
            {
                return 0;
            }
            else if (currentPosition == _currentMission.TrackedCarPosition)
            {
                return 1;
            }
            else
            {
                return 2;
            }
        }
        else
        {
            return -1;
        }
    }
}

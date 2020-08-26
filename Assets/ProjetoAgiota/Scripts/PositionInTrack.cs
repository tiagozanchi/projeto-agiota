using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PositionInTrack : MonoBehaviour
{
    [SerializeField]
    private float _timeToCheckPositions = 0.1f;

    private float _lastTimeChecked;
    private TMPro.TextMeshProUGUI _positionsText;
    private Slider _raceSlider;

    private float _raceTrackLength;
    public float RaceTrackLength
    {
        set => _raceTrackLength = value;
    }
    
    private CarsController[] _carsInOrder;
    private float _currentRacersAt = 0f;

    void Awake()
    {
        _positionsText = GetComponentInChildren<TMPro.TextMeshProUGUI>();
        _raceSlider = GetComponentInChildren<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.RaceStarted) return;

        if (Time.time > _lastTimeChecked + _timeToCheckPositions) CheckPositions();   
    }

    void CheckPositions()
    {
        _carsInOrder = GameManager.Instance.CarsInTrack.OrderBy(c => c.transform.position.z).ToArray();
        
        _positionsText.text = "";
        //CarsController[] tempCars = GameManager.Instance.CarsInTrack.OrderBy(c => c.transform.position.z).ToArray();
        
        for (int i = 0; i < _carsInOrder.Length; i++)
        {
            if (i != 0) _positionsText.text += "\n";
            _positionsText.text += string.Format("<color=#{0}> {1} - {2}",
                                                ColorUtility.ToHtmlStringRGB(GameManager.Instance.GetColor(_carsInOrder[i].Color)),
                                                i+1,
                                                _carsInOrder[i].name);
        }

        _currentRacersAt += 1f;
        _raceSlider.value = _currentRacersAt/_raceTrackLength;

        if (_raceSlider.value == 1)
        {
            GameManager.Instance.FinishRace(_carsInOrder[0]);
        }
    }

    public int GetCarPosition(CarsController car)
    {
        return Array.IndexOf(_carsInOrder, car)+1;
    }
}

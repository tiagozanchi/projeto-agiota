using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PositionInTrack : MonoBehaviour
{
    [SerializeField]
    private float _timeToCheckPositions = 0.1f;

    private float _lastTimeChecked;
    private Slider _raceSlider;

    private float _raceTrackLength;
    public float RaceTrackLength
    {
        set => _raceTrackLength = value;
    }
    
    private CarsController[] _carsInOrder;
    private float _currentRacersAt = 0f;

    public GameObject positionUIPrefab;
    public GameObject positionPanel;
    private List<PositionUI> positions = new List<PositionUI>();

    void Awake()
    {
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
        InitializePosUiItems();
        _carsInOrder = GameManager.Instance.CarsInTrack.OrderBy(c => c.transform.position.z).ToArray();
        
        //CarsController[] tempCars = GameManager.Instance.CarsInTrack.OrderBy(c => c.transform.position.z).ToArray();
        
        for (int i = 0; i < _carsInOrder.Length; i++)
        {
            positions[i].setNewDriverInfo(
                _carsInOrder[i].gameObject.name, /* Driver name */
                GameManager.GetColor(_carsInOrder[i].Color)); /* Car color */
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

    private void InitializePosUiItems() 
    {
        if (positions.Count != 0) return;

        int carAmount = GameManager.Instance.GetNumberOfCars();
        if (carAmount != -1) {

            float newY = 0f;
            for (int i = 1; i <= carAmount; i++) 
            {
                GameObject obj = Instantiate(positionUIPrefab, positionPanel.transform);
                obj.GetComponent<RectTransform>().anchoredPosition = Vector3.up * newY;
                newY -= 50;
                PositionUI posUi = obj.GetComponent<PositionUI>();
                posUi.Initialize(i, "Car", Color.white);
                positions.Add(posUi);
            }

        }
    }
}

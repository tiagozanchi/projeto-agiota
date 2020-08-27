using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    [SerializeField]
    private Image _currentWeaponImage;
    [SerializeField]
    private Image _nextWeaponImage;
    [SerializeField]
    private TMPro.TextMeshProUGUI _cooldownText;

    private WeaponController[] _availableWeapons;

    private WeaponController _currentWeapon;
    private WeaponController _nextWeapon;
    
    public void Init(WeaponController[] availableWeapons)
    {
        _availableWeapons = availableWeapons;
        GetFirstWeapon();
    }

    void GetFirstWeapon()
    {
        _currentWeapon = _availableWeapons[Random.Range(0,_availableWeapons.Length)];
        _nextWeapon = _availableWeapons[Random.Range(0,_availableWeapons.Length)];

        _currentWeapon.enabled = true;
        _currentWeapon.OnUse += UsedWeapon;
        UpdateIcons();
    }
    void GetNextWeapon()
    {
        _currentWeapon.OnUse -= UsedWeapon;

        _currentWeapon = _nextWeapon;
        _nextWeapon = _availableWeapons[Random.Range(0,_availableWeapons.Length)];

        _currentWeapon.enabled = true;
        _currentWeapon.OnUse += UsedWeapon;
        UpdateIcons();
    }

    void UpdateIcons()
    {
        _currentWeaponImage.sprite = _currentWeapon.WeaponIcon;
        _nextWeaponImage.sprite = _nextWeapon.WeaponIcon;

        _currentWeaponImage.gameObject.SetActive(true);
        _nextWeaponImage.gameObject.SetActive(true);
    }

    void UsedWeapon(int cooldown)
    {
        _currentWeapon.enabled = false;
        _currentWeaponImage.gameObject.SetActive(false);
        StartCoroutine(WaitCooldown(cooldown));
    }

    IEnumerator WaitCooldown(int cooldown)
    {
        _cooldownText.text = cooldown.ToString();

        for (int i = cooldown; i > 0; i--)
        {
            _cooldownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
        
        _cooldownText.text = "";
        GetNextWeapon();
    }
}

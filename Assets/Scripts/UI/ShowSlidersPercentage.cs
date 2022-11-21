using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowSlidersPercentage : MonoBehaviour
{
    [SerializeField] private Slider masterSlider;
    [SerializeField] private TextMeshProUGUI textMasterSliderValue;

    private void OnEnable()
    {
        masterSlider.value = XMLManager.Instance.savedData.masterSound * 100;
    }

    private void OnDisable()
    {
        XMLManager.Instance.savedData.masterSound = masterSlider.value / 100;
    }
    void Update()
    {
        ShowSliderValue();
        AudioListener.volume = masterSlider.value / 100;
    }
    public void ShowSliderValue()
    {
        textMasterSliderValue.text = masterSlider.value.ToString("F0") + '%';
    }
}
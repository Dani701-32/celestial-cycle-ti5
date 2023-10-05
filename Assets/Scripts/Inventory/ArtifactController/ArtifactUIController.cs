using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArtifactUIController : MonoBehaviour
{
    [SerializeField] private GameObject paintArea; // Parte que sera pintada no slider
    [SerializeField] private GameObject typeArtifactContainer; // Parte que sera pintada no slider
    [SerializeField] private Image image;
    [SerializeField] private Image iconType;
    [SerializeField] private int currentCharge; 
    private Slider _slider;

    public void StartSlider(int maxValue)
    {
        _slider = this.GetComponent<Slider>();
        _slider.maxValue = maxValue;
        image.enabled = false;
        paintArea.SetActive(false);
        typeArtifactContainer.SetActive(false);

    }

    public void StartSlider(int maxValue, int value)
    {
        _slider = this.GetComponent<Slider>();
        _slider.maxValue = maxValue;
        _slider.value = value;
        image.enabled = false;
        paintArea.SetActive(false);
        typeArtifactContainer.SetActive(false);
    }

    public void OpenSlider(Sprite sprite, Sprite type){
        image.sprite = sprite;
        iconType.sprite = type;
        image.enabled = true;
        currentCharge = 100;
        _slider.value = currentCharge;
        paintArea.SetActive(true);
        typeArtifactContainer.SetActive(true);
    }
    public void CloseSlider(){
        image.enabled = false;
        paintArea.SetActive(false); 
        typeArtifactContainer.SetActive(false);
    }


}

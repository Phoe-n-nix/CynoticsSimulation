using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SliderDivision : MonoBehaviour
{
    public Slider slider1;
    public Slider slider2;
    public TextMeshProUGUI slider1ValueText;
    public TextMeshProUGUI slider2ValueText;
    public TextMeshProUGUI resultText;

    void Start()
    {
        // Add listeners to update text when slider values change
        slider1.onValueChanged.AddListener(delegate { UpdateSliderValues(); });
        slider2.onValueChanged.AddListener(delegate { UpdateSliderValues(); });
        
        // Initialize the text fields
        UpdateSliderValues();
    }

    void UpdateSliderValues()
    {
        // Update the text with the current slider values and add 'V' as unit
        slider1ValueText.text = slider1.value.ToString("F2") + " V"; // Format to 2 decimal places
        slider2ValueText.text = slider2.value.ToString("F2") + " V"; // Format to 2 decimal places
        
        // Update the result text with the division of the slider values and add 'cm' as unit
        if (slider2.value != 0)
        {
            float divisionResult = slider1.value / slider2.value;
            resultText.text = divisionResult.ToString("F2") + " cm"; // Format to 2 decimal places
        }
        else
        {
            resultText.text = "Division by Zero!";
        }
    }
}

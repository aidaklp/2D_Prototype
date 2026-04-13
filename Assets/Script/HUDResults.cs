using UnityEngine;
using TMPro;


public class HUDResults : MonoBehaviour
{
    // creates a reffrence to the text mesh pro object
    [SerializeField] private TextMeshProUGUI hudText;

    private void Start()
    {
        // makes sure that an object containing the generatorData script exists
        if (GeneratorData.Instance != null)
        {
            //sets the text of the UI element to display te results from the generation
            hudText.text = $"{GeneratorData.Instance.FinalGenre} {GeneratorData.Instance.FinalBPM} {GeneratorData.Instance.FinalKey}";
        }
    }


}

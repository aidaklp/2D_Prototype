using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class MicrophoneDropdown : MonoBehaviour
{
    //stores the microphone dropdown TMP dropdown
    public TMP_Dropdown microphoneDropdown;
    public int chosenDeviceIndex = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //starts the populate source dropdown method
        PopulateSourceDropDown();
        //restores the saved microphone selection
        RestoreSavedSelection();

        //hooks up the dropdown's value-changed event directly in code,
        //avoiding the Inspector OnValueChanged quirk
        microphoneDropdown.onValueChanged.RemoveListener(ChooseMicrophone);
        microphoneDropdown.onValueChanged.AddListener(ChooseMicrophone);
    }

    //method for filling the dropdown options with the correct devices
    private void PopulateSourceDropDown()
    {
        //creates a new variable which is a list of dropdown options
        var options = new List<TMP_Dropdown.OptionData>();
        //loops through every microphone device unity is detecting on the device
        foreach (var microphone in Microphone.devices)
        {
            //changes the name of the dropdown option to the name of the microphone
            TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData(microphone);
            //adds that microphone as an option
            options.Add(optionData);
        }
        //once the loop is finished it assigns the full list of dropdown
        microphoneDropdown.options = options;
    }

    //method to restore the saved microphone selection
    private void RestoreSavedSelection()
    {
        string saved = GameData.Instance.chosenMicrophoneName;
        if (string.IsNullOrEmpty(saved)) return;

        int index = microphoneDropdown.options.FindIndex(o => o.text == saved);
        if (index >= 0)
        {
            microphoneDropdown.value = index;
            chosenDeviceIndex = index;
        }
    }

    //method for picking the microphone from the list of options
    public void ChooseMicrophone(int optionIndex)
    {
        //saves the index of the selected microphone
        chosenDeviceIndex = optionIndex;
        string deviceName = microphoneDropdown.options[optionIndex].text;
        GameData.Instance.chosenMicrophoneName = deviceName;
    }
}

using NUnit.Framework;
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
    }

    //method for filling the dropdown options with the correct devices
    private void PopulateSourceDropDown()
    {
        //creates a new variable which is a list of dropdown options
        var options = new List<TMP_Dropdown.OptionData>();

        //loops through every microphone device unity is detecting on the device
        foreach (var microphone in Microphone.devices)
        {
            //chancges the name of the options dropdown thingy to the name of te micorphone
            TMP_Dropdown.OptionData optionData = new   TMP_Dropdown.OptionData(microphone);
            //adds that microphone as an option
            options.Add(optionData);


        }

        //once the loop is finished it assigns the full list of dropdown
        microphoneDropdown.options = options;
    }

    //mathod for picking the microphone from the list of options
    public void ChooseMicrophone(int optionIndex)
    {
        //saves the index of the selected microphone 
        chosenDeviceIndex = optionIndex;

    }


}

using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class OutputDeviceDropdown : MonoBehaviour
{
    public TMP_Dropdown outputDropdown;
    public int chosenDeviceIndex = 0;

    void Start()
    {
        PopulateOutputDropDown();
        //restores the saved output device selection
        RestoreSavedSelection();

        //hooks up the dropdown's value-changed event directly in code,
        //avoiding the Inspector OnValueChanged quirk
        outputDropdown.onValueChanged.RemoveListener(ChooseOutputDevice);
        outputDropdown.onValueChanged.AddListener(ChooseOutputDevice);
    }

    //method to show the current available output devices 
    private void PopulateOutputDropDown()
    {
        var options = new List<TMP_Dropdown.OptionData>();
        // calls for the window API TO COUNT HOW many audio sources are available 
        int deviceCount = waveOutGetNumDevs();
        // for each device for each index number
        for (int i = 0; i < deviceCount; i++)
        {
            //creates an empty container that will hold the devices information
            WAVEOUTCAPS caps = new WAVEOUTCAPS();

            waveOutGetDevCaps((uint)i, ref caps, (uint)Marshal.SizeOf(caps));
            options.Add(new TMP_Dropdown.OptionData(caps.szPname));
        }
        outputDropdown.options = options;
    }

    //method to restore the saved output device selection
    private void RestoreSavedSelection()
    {
        string saved = GameData.Instance.chosenOutputDeviceName;
        if (string.IsNullOrEmpty(saved)) return;

        int index = outputDropdown.options.FindIndex(o => o.text == saved);
        if (index >= 0)
        {
            outputDropdown.value = index;
            chosenDeviceIndex = index;
        }
    }

    //method for choosing output device
    public void ChooseOutputDevice(int optionIndex)
    {
        chosenDeviceIndex = optionIndex;
        string deviceName = outputDropdown.options[optionIndex].text;
        GameData.Instance.chosenOutputDeviceName = deviceName;
        Debug.Log("Selected: " + deviceName);
    }

    // imports the waveOutGetNumbers function from windows
    [DllImport("winmm.dll")]
    private static extern int waveOutGetNumDevs();
    // imports the waveOutGetDevCaps function from windows
    [DllImport("winmm.dll", CharSet = CharSet.Ansi)]
    private static extern int waveOutGetDevCaps(uint uDeviceID, ref WAVEOUTCAPS pwoc, uint cbwoc);
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    private struct WAVEOUTCAPS
    {
        public ushort wMid;
        public ushort wPid;
        public uint vDriverVersion;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string szPname;
        public uint dwFormats;
        public ushort wChannels;
        public ushort wReserved1;
        public uint dwSupport;
    }
}
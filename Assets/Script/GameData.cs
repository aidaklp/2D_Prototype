using Unity.Collections;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GameData : MonoBehaviour
{
    // makes usre that there will only be one copy of the variables defined in this script 
   public static GameData Instance;

    // string variables that will store the generation results and are public so other scripts can use them
    public string FinalBPM;
    public string FinalGenre;
    public string FinalKey;

    //seting coins as a variable 
    public float coins = 0;

    [Header("Items")]
    //keeps track if metronomeItem has been bought
    public bool hasMetronome = false;
    //keeps track when redo item has been bought
    public bool hasRedo = false;

    [Header("Rounds")]
    //used to store the current round 
    public int currentRound = 1;

    //sets the first round to be true when the game is started 
    public bool isFirstRound = true;



    //method called for before Start() so very begining of the game
    private void Awake()
    {
        // checks if a object in the game carries this script
       if(Instance == null)
        {
            //stores a refrence to the specific object holding this script so now any script can reach it easily
            Instance = this;
            // keeps the game object across scenes
            DontDestroyOnLoad(gameObject);

            
        }

        else
        {
            // if the game object with this script as its component already exists from other scenes it destroys the new copy immiditely to prevent duplicates.
            Destroy(gameObject);
        }
    }

    public void ResetRoundData()
    {
        FinalBPM = "";
        FinalGenre = "";
        FinalKey = "";
    }
}

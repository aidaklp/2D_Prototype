using Unity.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GeneratorData : MonoBehaviour
{
    // makes usre that there will only be one copy of the variables defined in this script 
   public static GeneratorData Instance;

    // string variables that will store the generation results and are public so other scripts can use them
    public string FinalBPM;
    public string FinalGenre;
    public string FinalKey;

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

}

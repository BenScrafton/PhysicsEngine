using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    FluidSimulator fluidSimulator;

    // Start is called before the first frame update
    void Start()
    {
        fluidSimulator = FindObjectOfType<FluidSimulator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) 
        {
            fluidSimulator.gravity *= -1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) 
        {
            string FilePath = Application.streamingAssetsPath + "/JamParameters" + ".txt";

            fluidSimulator.LoadSimParameters(FilePath);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) 
        {
            string FilePath = Application.streamingAssetsPath + "/LiquidParameters" + ".txt";

            fluidSimulator.LoadSimParameters(FilePath);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            string FilePath = Application.streamingAssetsPath + "/TestParameters" + ".txt";

            fluidSimulator.LoadSimParameters(FilePath);
        }
    }
}

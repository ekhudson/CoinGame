using UnityEngine;
using System.Collections;

public static class UserInputTransceiver
{
    public enum InputCodes
    {
        RightArrow,
        LeftArrow,
        UpArrow,
        DownArrow,
    }

    public static UnityEngine.KeyCode GetInput(InputCodes code)
    {
//        switch(code)
//        {
//            //Input.
//        }

        return KeyCode.End;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CustomSpriteChanger : EditorWindow
{
    Sprite spriteImg;

    [MenuItem("Window/CustomSpriteChanger")]
    // Start is called before the first frame update
    public static void WindowAdd()
    {
        EditorWindow.GetWindow(typeof(CustomSpriteChanger));
    }

    private void OnGUI()
    {
        
    }
}

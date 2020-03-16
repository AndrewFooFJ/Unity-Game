using UnityEditor;
using UnityEngine;
using UnityEditorInternal;

// Tells Unity to use this Editor class with the WaveManager script component.
[CustomEditor(typeof(DialogManager))]
public class DialogueEditor : Editor
{

    // This will contain the <wave> array of the WaveManager. 
    SerializedProperty dialogues;

    // The Reorderable List we will be working with 
    ReorderableList list;

    private void OnEnable()
    {
        // Get the <Dialogue> array from DialogueManager, in SerializedProperty form.
        dialogues = serializedObject.FindProperty("dialogue");

        // Set up the reorderable list
        list = new ReorderableList(serializedObject, dialogues, true, true, true, true);

        list.drawElementCallback = DrawListItems; // Delegate to draw the elements on the list
        list.drawHeaderCallback = DrawHeader; // Skip this line if you set displayHeader to 'false' in your ReorderableList constructor.
    }

    // Draws the elements on the list
    void DrawListItems(Rect rect, int index, bool isActive, bool isFocused)
    {
        SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index); // The element in the list

        //The 'mobs' property. Since the enum is self-evident, I am not making a label field for it. 
        //The property field for mobs (width 100, height of a single line)
        EditorGUI.PropertyField(
        new Rect(new Rect(rect.x, rect.y, 100, EditorGUIUtility.singleLineHeight)),
        element.FindPropertyRelative("toPlay"),
        GUIContent.none
    );
        //The 'Sentence' property
        EditorGUI.LabelField(new Rect(rect.x + 120, rect.y, 100, EditorGUIUtility.singleLineHeight), "Sentence");

        //The property field for level. Since we do not need so much space in an int, width is set to 20, height of a single line.
        EditorGUI.PropertyField(
            new Rect(new Rect(rect.x + 180, rect.y, 210, EditorGUIUtility.singleLineHeight)), //EditorGUIUtility.singleLineHeight)),
            element.FindPropertyRelative("sentence"),
            GUIContent.none
        );
    }

    //Draws the header
    void DrawHeader(Rect rect)
    {
        string name = "Dialogues";
        EditorGUI.LabelField(rect, name);
    }

    //This is the function that makes the custom editor work
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI(); //show the previous GUI
        serializedObject.Update(); // Update the array property's representation in the inspector

        list.DoLayoutList(); // Have the ReorderableList do its work

        // We need to call this so that changes on the Inspector are saved by Unity.
        serializedObject.ApplyModifiedProperties();
    }
}
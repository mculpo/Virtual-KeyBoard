using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(VirtualKeyBoard))]
public class VirtualKeyBoardEditor : Editor
{

    string CurrentStringKeyBoard;
    int CurrentKeyboard;

    public override void OnInspectorGUI()
    {
        VirtualKeyBoard script = (VirtualKeyBoard)target;

        script.loadKeyBoards();

        EditorGUILayout.LabelField("CURRENT KEYBOARD USING");

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(this.CurrentStringKeyBoard);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("KEYBOARD NAME");
        script.nameVirtualKeyBoard = EditorGUILayout.TextField(script.nameVirtualKeyBoard);

        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("SAVE"))
        {
            script.saveVirtualKeyBoard();
        }

        EditorGUILayout.LabelField("ALL KEYBOARDS");

        if (script.listKeyBoard != null )
        {
            if (script.listKeyBoard.Count != 0)
            {
                int count = -1;
                foreach (StructKeyBoard keyBoard in script.listKeyBoard)
                {
                    count++;
                    EditorGUILayout.BeginHorizontal();

                    EditorGUILayout.LabelField(keyBoard.namePainel);

                    if (GUILayout.Button("Use"))
                    {
                        CurrentKeyboard = count;
                        this.CurrentStringKeyBoard = keyBoard.namePainel;
                        script.useVirtualKeyBoard(count);

                    }

                    if (GUILayout.Button("Delete"))
                    {
                        script.deleteKeyBoard(count);
                    }

                    if (GUILayout.Button("Update"))
                    {
                        script.updateVirtualKeyBoard(count);
                    }

                    if (GUILayout.Button("Copy To Using"))
                    {
                        script.copyToUseVirtualKeyBoard(count, this.CurrentKeyboard);
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
        }
    }
}

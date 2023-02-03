using System;
using UnityEditor;
using UnityEngine;

[InitializeOnLoadAttribute]
static class ComponentFilterEditor
{

  static ComponentFilterEditor()
  {
    Editor.finishedDefaultHeaderGUI += DisplayFilterField;
  }

  static void DisplayFilterField(Editor editor)
  {
    EditorGUILayout.BeginHorizontal();

    EditorGUI.BeginChangeCheck();
    string filterString = "";
    filterString = EditorGUILayout.TextField("Filter components", filterString);
    // update inspector only if the filter filed was updated
    if (EditorGUI.EndChangeCheck())
    {
      FilterComponents(editor.target as GameObject, filterString);
      // we have to do this to refresh the inspector. maybe there's a better way idk.
      EditorUtility.SetDirty(editor.target);
    }


    GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
    buttonStyle.fixedWidth = 65;
    // button to clear the search field
    if (GUILayout.Button("Reset", buttonStyle))
    {
      // update inspector only if the filter filed was updated
      FilterComponents(editor.target as GameObject, "");
      // we have to do this to refresh the inspector. maybe there's a better way idk.
      EditorUtility.SetDirty(editor.target);
    }

    EditorGUILayout.EndHorizontal();


  }

  private static void FilterComponents(GameObject obj, string filterString)
  {
    foreach (Component comp in obj.GetComponents(typeof(Component)))
    {
      // if nothing was typed show all components
      if (filterString == "") comp.hideFlags = HideFlags.None;
      else
        // compare filter string with component names (replace this function with your own fuzzy filter or whatever)
        if (IsStringInString(filterString, comp.GetType().Name))
        comp.hideFlags = HideFlags.None;
      else
        comp.hideFlags = HideFlags.HideInInspector;
    }
  }

  public static bool IsStringInString(string stringToCheck, string stringToSearch)
  {
    return stringToSearch.IndexOf(stringToCheck, StringComparison.OrdinalIgnoreCase) >= 0;
  }
}
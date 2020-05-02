using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MassParentorEditor : EditorWindow
{

   public GameObject Parent;
   public bool proMode = false;
   public bool DebugMode = false;
   public bool LogMode = false;
   public bool RenameMode = false;
   public bool NewParent = false;
   public string newName = "";
   public string ParentName = "EmptyParentObject";
   static bool NoUpdateOnOpen;
   static bool FirstBoot = true;
   public GameObject[] CurrentSelection;


    [MenuItem("Window/Parent Tool")]
    static void ShowWindow()
    {
      GetWindow<MassParentorEditor>("MassParentTool");
      if (!NoUpdateOnOpen)
      {
         Debug.Log("Mass Parent Tool V2 by Gregory Miller");
         Debug.Log("Added in V2: Ability to rename objects on parent, added button to create new Empty Gameobject and insert it into the parent slot, added the ability to remove highlighted objects from their parents, added option to disable patch notes at bootup");
         Debug.Log("Added in V1: Pro Mode, Debug Mode, QoL changes");
         Debug.Log("Added in Beta: Unparent Tool, Selection via Mouse");
        if(FirstBoot)
        {
            NoUpdateOnOpen = true;
            FirstBoot = false;

        }
      }
   }


    private void OnGUI()
    {
      EditorGUILayout.Space();
      EditorGUILayout.HelpBox("This tool allows you to parent/unparent mutiple objects with ease.", MessageType.Info);
      EditorGUILayout.Space();
      NewParent = EditorGUILayout.Toggle("Create a new Parent", NewParent);
      
      if(NewParent)
      {
         if (Parent == null)
         {
            ParentName = EditorGUILayout.TextField("Parent's New Name: ", ParentName);
            if (GUILayout.Button("Create Parent"))
            {
               if (Parent == null)
               {
                  ParentBuilder();
               }
            }
         }
         else
         {

            Parent = (GameObject)EditorGUILayout.ObjectField("Parent Object", Parent, typeof(Object), true);

         }
      }
     else
      {
         Parent = (GameObject)EditorGUILayout.ObjectField("Put the parent in this slot", Parent, typeof(Object), true);
      }
      
      EditorGUILayout.Space();
      if (proMode == false)
      {
         if (Parent == null)
         {
            if (NewParent)
            {
               EditorGUILayout.HelpBox("Please enter the name of new object, then press Create Parent.", MessageType.Info);
            }
            else
            {
               EditorGUILayout.HelpBox("Please select a object to become the parent in the inspector.", MessageType.Warning);
            }
         }
         else if (Selection.gameObjects == null)
         {

            EditorGUILayout.HelpBox("Please select all the children in the scene.", MessageType.Warning);

         }
         else
         {

            EditorGUILayout.HelpBox("Press the button below to parent.", MessageType.Info);

         }
      }
        if (GUILayout.Button("Parent Highlighted Objects"))
        {
         if (Parent != null)
         {
            ParentTool();
            NewParent = false;
         }
        }
      if (RenameMode)
      {
         EditorGUILayout.HelpBox("Please enter the new name for the children", MessageType.Info);
      }
      RenameMode = EditorGUILayout.Toggle("Rename Children", RenameMode);
      if (RenameMode)
      {

         newName = EditorGUILayout.TextField("Children's New Name: ", newName);

      }
      EditorGUILayout.Space();
      if (proMode == false)
      {
         if (Parent != null)
         {
            EditorGUILayout.HelpBox("If you want to unparent a object, press this button instead.", MessageType.Info);
         }
         else
         {

            EditorGUILayout.HelpBox("Please select a object to Parent in the inspector.", MessageType.Warning);

         }
      }
      if (GUILayout.Button("Unparent All Children"))
        {
         if (Parent != null)
         {
            UnparentTool();
         }
        }
      if (proMode == false)
      {
         if (CurrentSelection.Length != 0)
         {
            EditorGUILayout.HelpBox("If you want to unparent highlighted objects, press this button instead.", MessageType.Info);
         }
         else
         {

            EditorGUILayout.HelpBox("Highlight GameObjects to use this function.", MessageType.Warning);

         }
      }
      if (GUILayout.Button("Unparent Highlighted Objects"))
      {
         if (CurrentSelection.Length != 0)
         {
            UnparentTargetTool();
         }
      }

      EditorGUILayout.Space();

      if (DebugMode == true)
      {
         EditorGUILayout.HelpBox("Debug Stats", MessageType.Info); ;
         EditorGUILayout.Space();
         if (Parent != null)
         {
            EditorGUILayout.TextField("Current Children Amount", Parent.transform.childCount.ToString());
         }
         else
         {

            EditorGUILayout.TextField("Children of Parent","ERROR: Please insert a parent");

         }
         EditorGUILayout.Space();
         if (Selection.gameObjects != null)
         {

            EditorGUILayout.TextField("Highlighted Objects", CurrentSelection.Length.ToString());
            

         }

        


      }

      EditorGUILayout.Space();
      EditorGUILayout.HelpBox("Options.", MessageType.Info);
      proMode = EditorGUILayout.Toggle("ProMode Enable", proMode);
      DebugMode = EditorGUILayout.Toggle("Debugging Enable", DebugMode);
      LogMode = EditorGUILayout.Toggle("Logging Enable", LogMode);
      NoUpdateOnOpen = EditorGUILayout.Toggle("Disable Update Notes", NoUpdateOnOpen);
      EditorGUILayout.Space();
      EditorGUILayout.HelpBox("Created by Greg Miller", MessageType.None);

    }

    void ParentTool()
    {
        int DebugCount = 0;
        foreach(GameObject children in Selection.gameObjects)
        {
            DebugCount++;
            children.transform.parent = Parent.transform;
         if (RenameMode == true)
         {
            children.name = newName + DebugCount;
         }

      }
      if (LogMode == true)
      {
         Debug.Log("Object " + Parent.name + " had " + DebugCount + " Children added to it via the Mass Parent Tool");

      }
    }

    void UnparentTool()
    {

        Parent.transform.DetachChildren();
    if (LogMode == true)
      {
         Debug.Log("Object " + Parent.name + " had it's children removed via the Mass Parent Tool");
      }


   }
   void UnparentTargetTool()
   {
      string LogChildren = "";
      foreach (GameObject children in Selection.gameObjects)
      {

         children.transform.parent = null;
         LogChildren = LogChildren + children.name + ",";

      }
      if (LogMode == true)
      {
         Debug.Log("Object " + Parent.name + " had " + LogChildren + " removed via the Mass Parent Tool");
      }


   }

   void ParentBuilder()
   {

      Parent = new GameObject();
      Parent.name = ParentName;
      if (LogMode == true)
      {
         Debug.Log("Object " + Parent.name + " was Created");
      }

   }

   private void Update()
   {
      CurrentSelection = Selection.gameObjects;
      Repaint();
   }

}

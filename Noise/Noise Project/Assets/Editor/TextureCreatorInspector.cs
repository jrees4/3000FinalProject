using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(TextureCreator))]
public class TextureCreatorInspector : Editor
{
    private TextureCreator creator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable(){
        creator = target as TextureCreator;
        Undo.undoRedoPerformed += RefreshCreator;
    }

    private void OnDisable(){
        Undo.undoRedoPerformed += RefreshCreator;
    }

    private void RefreshCreator() {
        if(Application.isPlaying) {
            creator.FillTexture();
        }
    }

    public override void OnInspectorGUI () {
        EditorGUI.BeginChangeCheck();
        DrawDefaultInspector();
        if (EditorGUI.EndChangeCheck() ){ //&& Application.isPlaying
            RefreshCreator();
            //(target as TextureCreator).FillTexture();
        }
    }
}

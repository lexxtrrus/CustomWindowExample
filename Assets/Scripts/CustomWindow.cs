using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CustomWindow : EditorWindow
{
    private Color _colorForLeftMouseButton = new Color(0f,0f,0f,1f);
    private Color _colorForRightMouseButton = new Color(0f,0f,0f,1f);
    private Renderer source;
    private Texture2D _texture2D;
    private Color[,] _colors = new Color[8,8];

    [MenuItem("Tools/BadPaint")]
    public static void OpenCustomWindow()
    {
        CustomWindow window = GetWindowWithRect<CustomWindow>(new Rect(0,0, 730f, 450f));
        window.Show();
    }

    private void OnEnable()
    {
        _texture2D = new Texture2D(8, 8);

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                _texture2D.SetPixel(j,i,Color.white);
                _colors[j,i] = Color.white;
            }
        }

        source = GameObject.Find("Cube").GetComponent<Renderer>();
    }

    private void OnGUI()
    {
        var currentEvent = Event.current;
        
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical(GUILayout.Width(250f), GUILayout.MinWidth(250f));
        
        EditorGUILayout.LabelField("Toolbar", GUILayout.Height(25f), GUILayout.ExpandHeight(true));
        
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Left Button", GUILayout.Width(80f));
        EditorGUILayout.Space(20f);
        _colorForLeftMouseButton = EditorGUILayout.ColorField(_colorForLeftMouseButton, GUILayout.ExpandWidth(true));
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Right Button", GUILayout.Width(80f));
        EditorGUILayout.Space(20f);
        _colorForRightMouseButton = EditorGUILayout.ColorField(_colorForRightMouseButton, GUILayout.ExpandWidth(true));
        EditorGUILayout.EndHorizontal();

        if(GUILayout.Button("Fill All", GUILayout.ExpandWidth(true)))
        {
            FillAll();
        }
        
        GUILayout.Space(290f);
        
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Output Renderer", GUILayout.Width(100f));
        EditorGUILayout.Space(20f);
        source = EditorGUILayout.ObjectField(source,typeof(Renderer), true, GUILayout.ExpandWidth(true)) as Renderer;
        EditorGUILayout.EndHorizontal();
        
        if(GUILayout.Button("Set Texture", GUILayout.ExpandWidth(true)))
        {
            SetTextureToObject();
        }

        EditorGUILayout.EndVertical();
        
        
        Texture texture = EditorGUIUtility.whiteTexture;

        var startX = 300f;
        var startY = 35f;
        
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Rect rect = new Rect(startX + 50f * j, startY + 50f * i, 40f, 40f);

                if (currentEvent.isMouse)
                {
                    if (currentEvent.button == 0 && rect.Contains(currentEvent.mousePosition))
                    {
                        _colors[j, i] = _colorForLeftMouseButton;
                        currentEvent.Use();
                    }
                
                    if (currentEvent.button == 1 && rect.Contains(currentEvent.mousePosition))
                    {
                        _colors[j, i] = _colorForRightMouseButton;
                        currentEvent.Use();
                    }

                }

                GUI.color = _colors[j, i];
                GUI.DrawTexture(rect, texture);
            }
        }
        
        EditorGUILayout.EndHorizontal();
    }

    private void FillAll()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                _colors[j,i] = _colorForLeftMouseButton;
            }
        }
    }

    private void SetTextureToObject()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                _texture2D.SetPixel(j,i, _colors[j,i]);
            }
        }
        
        _texture2D.filterMode = FilterMode.Point;
        _texture2D.Apply();

        source.sharedMaterial.mainTexture = _texture2D;
    }
}

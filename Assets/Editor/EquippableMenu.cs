using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EquippableMenu : EditorWindow
{
    public GameObject currentPlayer = null;

    // Paths to equippables
    private const string SLEDGEHAMMER_PATH = "Equippables/Sledgehammer";

    [MenuItem("Tools/Equippables")]
    public static void ShowWindow()
    {
        GetWindow<EquippableMenu>("Equippables");
    }

    private void Awake()
    {
        // Try to pre-populate some fields
        currentPlayer = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnGUI()
    {
        // -------- Player Options GUI --------
        GUILayout.Label("Player Options", EditorStyles.boldLabel);

        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Current Player", EditorStyles.label);
        currentPlayer = (GameObject)EditorGUILayout.ObjectField(currentPlayer, typeof(GameObject), true);
        EditorGUILayout.EndVertical();

        // -------- Space --------
        GUILayout.Space(10);

        // -------- Equippables GUI --------
        GUILayout.Label("Equippables", EditorStyles.boldLabel);
        GUILayout.Label("Weapons", EditorStyles.label);
        if(GUILayout.Button("Sledgehammer"))
        {
            GameObject sledgehammer = (GameObject)Resources.Load(SLEDGEHAMMER_PATH);
            Debug.Log("Loaded : " + sledgehammer);
            EquipItem(sledgehammer);
        }
    }

    private void EquipItem(GameObject obj)
    {
        if (!currentPlayer) return;

        PlayerController pc = currentPlayer.GetComponent<PlayerController>();

        if (!pc) return;

        pc.EquipItem(Instantiate(obj));
    }
}

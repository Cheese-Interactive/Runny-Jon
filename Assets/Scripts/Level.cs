using UnityEngine;

[CreateAssetMenu]
public class Level : ScriptableObject {

    [Header("Settings")]
    public string levelName;
    public int ID;
    public Object scene;
    public Sprite icon;
    [Range(1, 3599)] public int timeLimit;
    public Transform objective;
    public bool isTutorial;

    [Header("Movement")]
    public bool walkEnabled;
    public bool sprintEnabled;
    public bool jumpEnabled;
    public bool crouchEnabled;
    public bool slideEnabled;
    public bool wallRunEnabled;
    public bool swingEnabled;
    public bool ziplineEnabled;

}
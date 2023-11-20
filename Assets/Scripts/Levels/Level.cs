using UnityEngine;

[CreateAssetMenu]
public class Level : ScriptableObject {

    [Header("Settings")]
    [SerializeField] private string levelName;
    [SerializeField] private int ID;
    [SerializeField] private Object scene;
    [SerializeField] private Sprite icon;
    [SerializeField][Range(1, 3599)] private int timeLimit;
    [SerializeField] private bool isTutorial;
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private Color defaultRopeStartColor;
    [SerializeField] private Color defaultRopeEndColor;

    [Header("Movement")]
    [SerializeField] private bool lookEnabled;
    [SerializeField] private bool walkEnabled;
    [SerializeField] private bool sprintEnabled;
    [SerializeField] private bool jumpEnabled;
    [SerializeField] private bool crouchEnabled;
    [SerializeField] private bool slideEnabled;
    [SerializeField] private bool wallRunEnabled;
    [SerializeField] private bool swingEnabled;
    [SerializeField] private bool ziplineEnabled;
    [SerializeField] private bool grabEnabled;

    public string GetLevelName() {

        return levelName;

    }

    public int GetID() {

        return ID;

    }

    public Object GetScene() {

        return scene;

    }

    public Sprite GetIcon() {

        return icon;

    }

    public int GetTimeLimit() {

        return timeLimit;

    }

    public bool GetIsTutorial() {

        return isTutorial;

    }

    public AudioClip GetBackgroundMusic() {

        return backgroundMusic;

    }

    public bool GetLookEnabled() {

        return lookEnabled;

    }

    public void SetLookEnabled(bool lookEnabled) {

        this.lookEnabled = lookEnabled;

    }

    public bool GetWalkEnabled() {

        return walkEnabled;

    }

    public void SetWalkEnabled(bool walkEnabled) {

        this.walkEnabled = walkEnabled;

    }

    public bool GetSprintEnabled() {

        return sprintEnabled;

    }

    public void SetSprintEnabled(bool sprintEnabled) {

        this.sprintEnabled = sprintEnabled;

    }

    public bool GetJumpEnabled() {

        return jumpEnabled;

    }

    public void SetJumpEnabled(bool jumpEnabled) {

        this.jumpEnabled = jumpEnabled;

    }

    public bool GetCrouchEnabled() {

        return crouchEnabled;

    }

    public void SetCrouchEnabled(bool crouchEnabled) {

        this.crouchEnabled = crouchEnabled;

    }

    public bool GetSlideEnabled() {

        return slideEnabled;

    }

    public void SetSlideEnabled(bool slideEnabled) {

        this.slideEnabled = slideEnabled;

    }

    public bool GetWallRunEnabled() {

        return wallRunEnabled;

    }

    public void SetWallRunEnabled(bool wallRunEnabled) {

        this.wallRunEnabled = wallRunEnabled;

    }

    public bool GetSwingEnabled() {

        return swingEnabled;

    }

    public void SetSwingEnabled(bool swingEnabled) {

        this.swingEnabled = swingEnabled;

    }

    public bool GetZiplineEnabled() {

        return ziplineEnabled;

    }

    public void SetZiplineEnabled(bool ziplineEnabled) {

        this.ziplineEnabled = ziplineEnabled;

    }

    public bool GetGrabEnabled() {

        return grabEnabled;

    }

    public void SetGrabEnabled(bool grabEnabled) {

        this.grabEnabled = grabEnabled;

    }
}
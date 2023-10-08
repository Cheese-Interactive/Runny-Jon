using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public abstract class GameManager : MonoBehaviour {

    public abstract void CompleteLevel();

    public abstract Level GetCurrentLevel();

    public abstract int GetLevelTimeLimit();

    public abstract void KillPlayer();

}

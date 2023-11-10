using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class LevelSection : ScriptableObject {

    [Header("Information")]
    [SerializeField] private string sectionName;
    [SerializeField] private Image icon;
    [SerializeField] private List<Level> levels;

    public string GetSectionName() {

        return sectionName;

    }

    public Image GetIcon() {

        return icon;

    }

    public List<Level> GetLevels() {

        return levels;

    }
}

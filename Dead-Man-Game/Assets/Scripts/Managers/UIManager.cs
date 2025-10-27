using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[DefaultExecutionOrder(-50)]
public class UIManager : MonoBehaviour
{
    public static UIManager Instance {get; private set;}

    // List of level sprites
    [SerializeField] private List<Sprite> levelIcons = new List<Sprite>();
    [SerializeField] private List<Image> levelIconSlots = new List<Image>();

    private void Awake(){
        if (Instance != null && Instance != this) {
            DestroyImmediate(gameObject);
        } else {
            Instance = this;
        }
    }

    private void Start(){
    }

    public void UpdateLevelUI(int currentLevel){
        // Determine the range to populate the sprite list based on the level
        // Loop through each lvl icon slot and populate that slot with the proper index of the range
        Debug.Log($"Current Level: {currentLevel}");

        switch (currentLevel){
            case < 8:
                UpdateLevelIcons(0, currentLevel-1);
                break;
            case > 7 and < 19:
                UpdateLevelIcons(currentLevel-7, currentLevel);
                break;
            case > 18:
            UpdateLevelIcons(13,19);
                break;
        }
    }

    private void UpdateLevelIcons(int startIndex, int endIndex)
    {
        // Loop through each level icon slot one at a time
        for (int slotIndex = 0; slotIndex < levelIconSlots.Count; slotIndex++)
        {
            if  (slotIndex > endIndex){
                levelIconSlots[slotIndex].sprite = levelIcons[20];
            }else{
                // Place the corresponding icon in the icon slot
                levelIconSlots[slotIndex].sprite = levelIcons[slotIndex + startIndex];
            }
        }
    }
}

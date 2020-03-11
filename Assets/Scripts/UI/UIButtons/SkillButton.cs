using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public enum SkillState
{
    Unlockable,
    Pressed,
    Disabled,
    Unlocked
}

public class SkillButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public SkillType type;
    public int lvl;

    public Image unlockable;
    public Image pressed;
    public Image disabled;
    public Image unlocked;

    public Image textZone;

    public TextMeshProUGUI skillName;
    public TextMeshProUGUI skillDescription;

    public Skill skillDetails;

    public bool alwaysUnlocked;

    // Start is called before the first frame update
    void Start()
    {
        TriggerText(false);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();
    }

    public void UpdateState()
    {
        if (GameManager.SkillTree != null)
        {
            if (alwaysUnlocked || GameManager.SkillTree.tree[(int)type + lvl])
            {
                Show(SkillState.Unlocked);
            }
            else if (GameManager.SkillTree.CheckDependencies(type, lvl) && GameManager.ShapeMud >= GameManager.SkillTree.CheckCost(type, lvl))
            {
                Show(SkillState.Unlockable);
            }
            else
            {
                Show(SkillState.Disabled);
            }
        }
    }

    public void Show(SkillState state)
    {
        switch (state)
        {
            case SkillState.Unlocked:
                unlockable.enabled = false;
                pressed.enabled = false;
                disabled.enabled = false;
                unlocked.enabled = true;
                break;
            case SkillState.Disabled:
                unlockable.enabled = false;
                pressed.enabled = false;
                disabled.enabled = true;
                unlocked.enabled = false;
                break;
            case SkillState.Pressed:
                unlockable.enabled = false;
                pressed.enabled = true;
                disabled.enabled = false;
                unlocked.enabled = false;
                break;
            case SkillState.Unlockable:
                unlockable.enabled = true;
                pressed.enabled = false;
                disabled.enabled = false;
                unlocked.enabled = false;
                break;
        }
    }

    public void OnClick()
    {
        if (GameManager.SkillTree.CheckDependencies(type, lvl) && GameManager.ShapeMud >= GameManager.SkillTree.CheckCost(type, lvl))
        {
            GameManager.SkillTree.Unlock(type, lvl);
            SaveManager.Instance.SaveGame();
        }
    }

    public void TriggerText(bool show)
    {
        textZone.enabled = show;
        skillName.enabled = show;
        skillDescription.enabled = show;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TriggerText(true);
        skillName.text = skillDetails.skillName;
        skillDescription.text = skillDetails.description;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TriggerText(false);
    }

}

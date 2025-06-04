using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_LevelUp : BaseUI
{
    private TMP_Text currentLevel;
    private TMP_Text nextLevel;
    private Animator anim;

    private bool isFinish = false;

    protected override void Awake()
    {
        base.Awake();

        currentLevel = GetUI<TextMeshProUGUI>("Level");
        nextLevel = GetUI<TextMeshProUGUI>("NextLevel");
        anim = GetComponent<Animator>();

        currentLevel.text = (DataManager.Instance.level.Value - 1).ToString();        
        nextLevel.text = DataManager.Instance.level.Value.ToString();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return) && isFinish)
            anim.SetTrigger("Out");
    }

    public void isFinishTrue() => isFinish = true;

    public void ActivateFalse() => gameObject.SetActive(false);
}

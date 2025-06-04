using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Rank : BaseUI
{
    [SerializeField] private VertexGradient[] rankGradients;
    private Animator crownAnim;
    private Animator rankAnim;

    private TMP_Text rankText;

    protected override void Awake()
    {
        base.Awake();

        rankText = GetUI<TextMeshProUGUI>("Rank");
        crownAnim = GetUI<Animator>("Crown");
        rankAnim = GetUI<Animator>("Rank");        
    }    

    public void PlayRankRoutine(Rank rank)
    {
        rankText.colorGradient = rankGradients[(int)rank];
        rankText.text = rank.ToString();
        
        if (rank == Rank.S)
            StartCoroutine(SRankRoutine());
        else
            StartCoroutine(RankRoutine());
    }

    private IEnumerator SRankRoutine()
    {
        Debug.Log("Start SRankRoutine");

        yield return new WaitForSeconds(1f);
        Debug.Log("Before Rank In");
        rankAnim.SetTrigger("In");

        yield return new WaitForSeconds(.5f);
        Debug.Log("Before Crown In");
        crownAnim.SetTrigger("In");

        yield return new WaitForSeconds(.5f);
        Debug.Log("Before Rotate");
        crownAnim.SetTrigger("Rotate");

        Debug.Log("End SRankRoutine");
    }

    private IEnumerator RankRoutine()
    {
        yield return new WaitForSeconds(1f);

        rankAnim.SetTrigger("In");        
    }
}

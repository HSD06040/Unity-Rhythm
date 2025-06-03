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
        yield return new WaitForSeconds(1f);
        rankAnim.SetTrigger("In");

        yield return new WaitForSeconds(.5f);
        crownAnim.SetTrigger("In");

        yield return new WaitForSeconds(.5f);

        crownAnim.SetTrigger("Rotate");
    }

    private IEnumerator RankRoutine()
    {
        yield return new WaitForSeconds(1f);

        rankAnim.SetTrigger("In");        
    }
}

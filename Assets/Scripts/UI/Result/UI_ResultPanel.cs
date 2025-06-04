using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_ResultPanel : BaseUI
{
    [SerializeField] private RankParticle rankParticle;
    [SerializeField] private UI_Rank rank;

    [Header("Speed")]
    [SerializeField] private float speed;
    [SerializeField] private float lerpDuration;

    [Header("Delay")]
    [SerializeField] private float delay;
    private YieldInstruction _delay;
    [Space]

    [SerializeField] private PlayData playData;

    [SerializeField] private Slider rateSlider;
    [SerializeField] private GameObject levelUp;

    private TMP_Text bestComboText;
    private TMP_Text perfectText;
    private TMP_Text greatText;
    private TMP_Text goodText;
    private TMP_Text missText;
    private TMP_Text scoreText;
    private TMP_Text rateText;

    private GameObject bestComboObj;
    private GameObject perfectObj;
    private GameObject greatObj;
    private GameObject goodObj;
    private GameObject missObj;
    private GameObject scoreObj;

    [Header("TextEndTransform")]
    [SerializeField] private Transform bestComboTextEndTransform;
    [SerializeField] private Transform perfectTextEndTransform;
    [SerializeField] private Transform greatTextEndTransform;
    [SerializeField] private Transform goodTextEndTransform;
    [SerializeField] private Transform missTextEndTransform; 
    [SerializeField] private Transform rateTextEndTransform;

    [Header("EndTransform")]
    [SerializeField] private Transform bestComboEndTransform;
    [SerializeField] private Transform perfectEndTransform;
    [SerializeField] private Transform greatEndTransform;
    [SerializeField] private Transform goodEndTransform;
    [SerializeField] private Transform missEndTransform;
    [SerializeField] private Transform scoreEndTransform;

    private Coroutine lobbyRoutine;

    protected override void Awake()
    {
        base.Awake();

        bestComboText = GetUI<TextMeshProUGUI>("BestCombo");
        perfectText = GetUI<TextMeshProUGUI>("Perfect");
        greatText = GetUI<TextMeshProUGUI>("Great");
        goodText = GetUI<TextMeshProUGUI>("Good");
        missText = GetUI<TextMeshProUGUI>("Miss");
        scoreText = GetUI<TextMeshProUGUI>("Score");
        rateText = GetUI<TextMeshProUGUI>("Rate");

        bestComboObj = GetUI("BestComboCount");
        perfectObj = GetUI("PerfectCount");
        greatObj = GetUI("GreatCount");
        goodObj = GetUI("GoodCount");
        missObj = GetUI("MissCount");
        scoreObj = GetUI("ScoreCount");
    }

    private void Start()
    {
        playData = GameManager.Instance.currnetPlayData;
        StartCoroutine(ResultRoutine());
    }

    private void Update()
    {
        if (levelUp.activeSelf) return;

        if(Input.GetKeyUp(KeyCode.Return))
        {
            if(lobbyRoutine == null)
                lobbyRoutine = StartCoroutine(LoadLobby());
        }
    }

    private IEnumerator LoadLobby()
    {
        UI_Manager.Instance.fadeScreen.EnterFade(FadeType.Defualt);

        yield return new WaitForSeconds(1);
        UI_Manager.Instance.mvPlayer.StopVideo();
        AudioManager.Instance.StopBGM();

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Lobby");
        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < 0.9f)
            yield return null;

        yield return new WaitForSeconds(.5f);

        asyncLoad.allowSceneActivation = true;

        UI_Manager.Instance.fadeScreen.ExitFade(FadeType.Defualt);
        lobbyRoutine = null;
    }

    private IEnumerator UpdateTextRoutine(TMP_Text text, int targetIdx)
    {
        float amount = 0;
        float delta = 0;

        while (delta <= lerpDuration)
        {
            float t = delta / lerpDuration;

            float currentValue = Mathf.Lerp(amount, targetIdx, t);
            text.text = currentValue.ToString("F0");

            delta += Time.deltaTime;
            yield return null;  
        }
    }
    private IEnumerator UpdateTextRoutine(TMP_Text text, float targetIdx)
    {
        float amount = 0;
        float delta = 0;
        float sliderAmount = 0;

        while (delta <= lerpDuration)
        {            
            float t = delta / lerpDuration;

            float currentValue = Mathf.Lerp(amount, targetIdx, t);
            string value = currentValue.ToString("F2");
            text.text = $"{value}%";

            float sliderValue = Mathf.Lerp(sliderAmount, targetIdx, t);
            rateSlider.value = sliderValue;

            delta += Time.deltaTime;
            yield return null;
        }

        rateSlider.value = targetIdx;
        text.text = $"{targetIdx.ToString("F2")}%";
    }

    private IEnumerator ResultRoutine()
    {
         _delay = new WaitForSeconds(delay);

        yield return new WaitForSeconds(1.3f);

        if (playData.rank == Rank.S)
            rankParticle.PlayParticle();

        StartCoroutine(MoveRoutine(rateText.gameObject, rateTextEndTransform));
        StartCoroutine(MoveRoutine(scoreObj.gameObject, scoreEndTransform));
        StartCoroutine(UpdateTextRoutine(rateText, playData.rate));
        StartCoroutine(UpdateTextRoutine(scoreText, playData.score));

        yield return _delay;
                
        StartCoroutine(MoveRoutine(missObj, missEndTransform));        

        yield return _delay;

        StartCoroutine(MoveRoutine(missText.gameObject, missTextEndTransform));
        StartCoroutine(MoveRoutine(goodObj, goodEndTransform));
        StartCoroutine(UpdateTextRoutine(missText, playData.m70));

        yield return _delay;

        StartCoroutine(MoveRoutine(goodText.gameObject, goodTextEndTransform));
        StartCoroutine(MoveRoutine(greatObj, greatEndTransform));
        StartCoroutine(UpdateTextRoutine(goodText, playData.m80));

        yield return _delay;

        StartCoroutine(MoveRoutine(greatText.gameObject, greatTextEndTransform));
        StartCoroutine(MoveRoutine(perfectObj, perfectEndTransform));
        StartCoroutine(UpdateTextRoutine(greatText, playData.m90));

        yield return _delay;

        StartCoroutine(MoveRoutine(perfectText.gameObject, perfectTextEndTransform));
        StartCoroutine(MoveRoutine(bestComboObj, bestComboEndTransform));
        StartCoroutine(UpdateTextRoutine(perfectText, playData.m100));

        yield return _delay;

        StartCoroutine(MoveRoutine(bestComboText.gameObject, bestComboTextEndTransform));
        StartCoroutine(UpdateTextRoutine(bestComboText, playData.maxCombo));

        rank.PlayRankRoutine(playData.rank);
    }

    private IEnumerator MoveRoutine(GameObject target, Transform endTransform)
    {
        while (Vector2.Distance(target.transform.position, endTransform.position) > .1f)
        {
            target.transform.position = Vector2.Lerp(target.transform.position, endTransform.position, speed * Time.deltaTime);

            yield return null;
        }        
    }
}
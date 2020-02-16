using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TestimonyScene : Event
{
    private TestimonyManager theTM;
    protected int Count => theTM.Count; //어떤 장면인가?
    private bool IsCorrect => testimony.logCount == Count;
    [SerializeField]
    protected Testimony testimony;
    private bool IsTestimony() => theTM.state == TestimonyManager.State.testimony;
    private bool IsCoroutine;
    [SerializeField]
    protected Dialog start_dialog;
    [SerializeField]
    protected Dialog backToZero;

    //protected abstract IEnumerator StartTestimony();
    protected abstract IEnumerator InterrogationCoroutine(int _i);
    protected abstract IEnumerator WrongAnswerCoroutine();
    protected void ExitCoroutine() => IsCoroutine = false;
    protected void SetTestimony(int _c, string _s) => theTM.SetTestimony(_c, _s);

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        theTM = FindObjectOfType<TestimonyManager>();
    }
    protected override IEnumerator EventCoroutine()
    {
        FadeIn(); //화면이 밝아진 후       
        StartTestimony(testimony);//할당, 증언 개시 글자가 전개되고 진입
        yield return new WaitUntil(() => IsTestimony()); //최초 증언 종료 후
        FadeOut(); //화면이 어두워 진 후
        yield return waitTime;
        FadeIn(); //다시 밝아지고
        StartInterrogation(start_dialog);
        yield return waitExit; //개략적인 추궁의 방향을 요약한 뒤 심문 개시      
        FadeOut();
        yield return waitTime;
        
        while (flag)
        {
            FadeIn();
            ShowTestimony(Count);

            yield return new WaitUntil(() => !IsTestimony()); //입력이 있어 상태가 전이될 때까지 대기.

            switch (theTM.state)
            {
                case TestimonyManager.State.interrogate:
                    HoldIt();
                    IsCoroutine = true;
                    StartCoroutine(InterrogationCoroutine(Count));
                    yield return new WaitUntil(() => !IsCoroutine);
                    theTM.NextCount(); 
                    break;
                case TestimonyManager.State.objection:
                    if (theTM.Answer && IsCorrect)
                    {
                        Objection();
                        flag = false;
                    }                       
                    else
                    {
                        Objection();
                        IsCoroutine = true;
                        StartCoroutine(WrongAnswerCoroutine());
                        yield return new WaitUntil(() => !IsCoroutine);
                        yield return waitTime;
                    }
                    break;
                case TestimonyManager.State.back_to_zero:
                    StartInterrogation(backToZero);
                    yield return waitExit;
                    FadeOut();
                    theTM.NextCount();
                    yield return waitTime;
                    break;
                default:
                    break;
            }//return to testimony
        }
        NextEvent(nextEvent);
    } //접근 금지
}

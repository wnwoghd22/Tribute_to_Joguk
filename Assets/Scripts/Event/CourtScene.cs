using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CourtScene : Event
{
    private TestimonyManager theTM;
    protected int Count => theTM.Count; //어떤 장면인가?
    private bool IsCorrect => testimony.logCount == Count;
    [SerializeField]
    protected Testimony testimony;
    private bool IsTestimony() => theTM.state == TestimonyManager.State.testimony;
    private bool IsCoroutine;
    [SerializeField]
    protected Dialog backToZero;

    protected abstract IEnumerator InterrogationCoroutine(int _i);
    protected abstract IEnumerator WrongAnswerCoroutine();
    protected void ExitCoroutine() => IsCoroutine = false;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        theTM = FindObjectOfType<TestimonyManager>();
    }
    protected override IEnumerator EventCoroutine()
    {
        FadeIn();
        StartTestimony(testimony);//할당, 심문개시 글자가 전개되고 진입

        while (flag)
        {
            ShowTestimony(Count);

            yield return new WaitUntil(() => !IsTestimony()); //입력이 있어 상태가 전이될 때까지 대기.

            switch (theTM.state)
            {
                case TestimonyManager.State.interrogate:
                    HoldIt();
                    IsCoroutine = true;
                    StartCoroutine(InterrogationCoroutine(Count));
                    yield return new WaitUntil(() => !IsCoroutine);
                    break;
                case TestimonyManager.State.objection:
                    if (theTM.Answer && IsCorrect)
                        flag = false;
                    else
                    {
                        Objection();
                        IsCoroutine = true;
                        StartCoroutine(WrongAnswerCoroutine());
                        yield return new WaitUntil(() => !IsCoroutine);
                    }
                    break;
                case TestimonyManager.State.back_to_zero:
                    StartInterrogation(backToZero);
                    yield return new WaitUntil(() => !IsExcuting);
                    break;
                default:
                    break;
            }
            theTM.NextCount(); //return to testimony
        }
        NextEvent(nextEvent);
    } //접근 금지
}

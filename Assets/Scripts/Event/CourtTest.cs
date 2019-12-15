using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourtTest : CourtScene
{
    protected override IEnumerator InterrogationCoroutine(int _i)
    {
        switch (_i)
        {
            case 0:
                yield return new WaitForSeconds(1f);
                break;
        }
    }

    protected override IEnumerator ObjectionCoroutine(int _i, int _ID)
    {
        throw new System.NotImplementedException();
    }
}

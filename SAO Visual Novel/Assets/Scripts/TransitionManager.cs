using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager ins;
    public float time;
    //public Button nextBtn;


    public bool isPlaying;

    Coroutine routine;
    private void Awake()
    {
        ins = this;
        
    }

    public static void Fade(string kind, SpriteRenderer img, Action complete)
    {
        ins.StartCoroutine(ins.FadeEnum(kind, img, complete));
    }
    public static void Fade(string kind, TextMeshPro text, Action complete)
    {
        ins.StartCoroutine(ins.FadeEnum(kind, text, complete));
    }
    public static void FadeTransition(string type, SpriteRenderer img, Color color, Action complete)
    {
        //ins.FadeTransitionNonStatic(img, complete);
        img.color = color;
        ins.StartCoroutine(ins.FadeEnum(type,img, complete));
        //complete();
    }
    public static void TransitionWithText(SpriteRenderer img, TextMeshPro text, string animated, Action complete)
    {
        ins.TransitionWithTextNonStatic(img, text, animated, complete);
    }
    public static void TransitionWithText(SpriteRenderer img, TextMeshPro text, string animated,Action halfComplete, Action complete)
    {
        ins.TransitionWithTextNonStatic(img, text, animated,halfComplete, complete);
    }


    public IEnumerator FadeEnum(string kind, SpriteRenderer img, Action complete)
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / time;
            Color imgColor = img.color;
            if (kind == "in") imgColor.a = Mathf.Lerp(1, 0, t);
            if(kind == "out") imgColor.a = Mathf.Lerp(0, 1, t);
            img.color = imgColor;
            yield return null;
        }
        complete();
    }
    public IEnumerator FadeEnum(string kind, TextMeshPro img, Action complete)
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / time;
            Color imgColor = img.color;
            if (kind == "in") imgColor.a = Mathf.Lerp(1, 0, t);
            if (kind == "out") imgColor.a = Mathf.Lerp(0, 1, t);
            img.color = imgColor;
            yield return null;
        }
        complete();
    }
    void TransitionWithTextNonStatic(SpriteRenderer img, TextMeshPro text,string animated, Action complete)
    {
        StartCoroutine(FadeEnum("out", img, () => { }));
        StartCoroutine(FadeEnum("out", text, () => {
            StartCoroutine(Wait(1, () =>
            {
                StartCoroutine(FadeEnum("in", img, complete));
                StartCoroutine(FadeEnum("in", text, () => { }));
            }));
        }));
    }
    void TransitionWithTextNonStatic(SpriteRenderer img, TextMeshPro text, string animated, Action halfComplete, Action complete)
    {
        StartCoroutine(FadeEnum("out", img, () => { }));
        StartCoroutine(FadeEnum("out", text, () => {
            StartCoroutine(Wait(1, () =>
            {
                halfComplete();
                StartCoroutine(FadeEnum("in", img, complete));
                StartCoroutine(FadeEnum("in", text, () => { }));
            }));
        }));
    }
    IEnumerator Wait(float time, Action complete)
    {
        yield return new WaitForSeconds(time);
        complete();
    }
}

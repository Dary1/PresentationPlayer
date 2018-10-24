using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum FaderState { Init, FadingIn, Pause, FadingOut, Stopped }

public class AutoFaderExecution : MonoBehaviour
{    
    public FaderState FaderState = FaderState.Init;
    public AutoFaderController Controller;
    public Type type;
    Text text;
    Image image;
    AudioSource audioSource;

    public void Initialize()
    {
        text = this.GetComponent<Text>();
        image = this.GetComponent<Image>();
        audioSource = this.GetComponent<AudioSource>();

        if (this.transform.childCount == 0)
        {
            int calculated = 0;
            if (text != null)
            {
                calculated = (int)(PresentationPlayer.Instance.WaitPerText * text.text.Length / Time.fixedDeltaTime);
                calculated += (int)(PresentationPlayer.Instance.WaitPerImage / Time.fixedDeltaTime);
            }
            RecursivelyAddCount(this, calculated);
        }
    }

    private void RecursivelyAddCount(AutoFaderExecution afe, int calculated)
    {
        afe.counter = 0;
        if (afe.countForNextAction != 0)
        {
            afe.countForNextAction += 200;
        }
        afe.countForNextAction += calculated;
        afe.FaderState = FaderState.FadingIn;

        if (afe.Controller.Parent != null)
        {
            RecursivelyAddCount(afe.Controller.Parent.Execution, calculated);
        }

    }

    int counter = 0;
    public int countForNextAction;

    int FadingInCounter = 100;
    int FadingOutCounter = 100;


    private void FixedUpdate()
    {
        if (FaderState == FaderState.FadingIn)
        {
            if (audioSource != null && !audioSource.isPlaying)
                audioSource.Play();
            if (counter++ > FadingInCounter)
            {
                counter = 0;
                FaderState = FaderState.Pause;
            }
            FadeIn();
        }
        else if (FaderState == FaderState.Pause)
        {
            if (counter++ > countForNextAction)
            {
                counter = 0;
                FaderState = FaderState.FadingOut;
            }
        }
        else if (FaderState == FaderState.FadingOut)
        {
            if (counter++ > FadingOutCounter)
            {
                counter = 0;
                FaderState = FaderState.Stopped;
            }
            FadeOut();
        }
        else if (FaderState == FaderState.Stopped)
        {
            this.enabled = false;
            if (this.Controller.Parent.HasMoreToExecute())
                this.Controller.Parent.Begin();
        }
    }

    private void FadeOut()
    {
        if (text != null)
            text.color -= new Color(0, 0, 0, 0.01f);
        else if (image != null)
            image.color -= new Color(0, 0, 0, 0.01f);
        else if (audioSource != null)
            audioSource.volume -= 0.01f;
    }

    private void FadeIn()
    {
        if (text != null)
            text.color += new Color(0, 0, 0, 0.01f);
        else if (image != null)
            image.color += new Color(0, 0, 0, 0.01f);
        else if (audioSource != null)
            audioSource.volume = 1;
    }
}

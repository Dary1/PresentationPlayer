using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PresentationPlayer : MonoBehaviour {

    public float WaitPerText = 0.3f;
    public float WaitPerImage = 2f;
    public float FadingVolumn = 1f;
    public AutoFaderController FirstController;
    public static PresentationPlayer Instance;

    // Use this for initialization
    void Awake () {
        Instance = this;
        foreach (Image image in GetComponentsInChildren<Image>()) {
            image.color -= new Color(0, 0, 0, 1);
        }
        foreach (Text text in GetComponentsInChildren<Text>())
        {
            text.color -= new Color(0, 0, 0, 1);
        }
        foreach (AudioSource ac in GetComponentsInChildren<AudioSource>())
        {
            ac.volume = 0;
        }
        FirstController = this.gameObject.AddComponent<AutoFaderController>();
        AutoFaderExecution afe = this.gameObject.AddComponent<AutoFaderExecution>();
        FirstController.Execution = afe;
        
        afe.Controller = FirstController;
        FirstController.MakeKids();
        FirstController.enabled = false;
    }

    private void FixedUpdate()
    {
        if (!FirstController.enabled) {
            FirstController.Begin();            
            this.enabled = false;
        }
    }

}

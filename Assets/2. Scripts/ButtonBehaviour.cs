using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonBehaviour : MonoBehaviour
{
    #region Public Variables
    
    public enum State { normal, hover, pressed }
    public State myState;
    [Header("Colour")]
    public Color normalClr;
    public Color hoverClr;
    public Color pressedClr;
    [Header("Scale")]
    public float normalScale;
    public float hoverScale;
    public float pressedScale;
    [Header("Actions")]
    public TMP_Text text;
    public UnityEvent whenPressed;
    
    #endregion
    #region Private Variables

    private Color _myColor;
    
    #endregion
    #region Unity Methods

    private void Start()
    {
        myState = State.normal;
        _myColor =  normalClr;
    }

    #endregion
    #region Public Methods
    
    public State GetState() => myState;
    public void BTN_Normal() => BTN_Pressed(State.normal);
    public void BTN_Hover() => BTN_Pressed(State.hover);
    public void BTN_Pressed() => BTN_Pressed(State.pressed);
    
    public void SCN_LoadScene(string s) => SceneManager.LoadScene(s);
    public void SCN_Quit() => Application.Quit();
    
    #endregion
    #region Private Methods

    private void BTN_Pressed(State s)
    {
        myState = s;
        
        switch (myState)
        {
            case State.normal:
                SetColour(normalClr);
                SetScale(normalScale);
                break;
            case State.hover:
                SetColour(hoverClr);
                SetScale(hoverScale);
                break;
            case State.pressed:
                SetColour(pressedClr);
                SetScale(pressedScale);
                whenPressed.Invoke();
                break;
        }
    }

    private void SetColour(Color c) => text.color = c;
    private void SetScale(float scale) => text.transform.localScale = new Vector3(scale, scale, scale);
    
    
    #endregion Private Methods
}

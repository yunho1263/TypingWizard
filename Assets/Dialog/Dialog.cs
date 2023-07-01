using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog
{
    public enum DialogMode
    {
        Default,
        Cinematic,
        Auto
    }
    public DialogMode dialogMode;

    public string dialogName;

    public List<Dialog_Node> dialog_Nodes;
    public Dialog_Node root;
}

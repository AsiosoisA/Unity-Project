using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OptionMenu : InterfaceMenu
{
    protected override void Awake(){
        IgnoreActiveInput();
    }

}

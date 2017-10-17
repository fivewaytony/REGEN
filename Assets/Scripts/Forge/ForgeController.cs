using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class ForgeController : GameController
{
    public static ForgeController forgeInstance;

    // Use this for initialization
    void Start () {
        forgeInstance = this;
        PlayerStatLoad();
        PlayerPssItemLoadALL(); //전체 소유 아이템 로드
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

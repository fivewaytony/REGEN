using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class ShopController : GameController
{
    public static ShopController ishopInstance;

    private void Awake()
    {
     PlayerStatLoad();          //Player 상태 로드
     PlayerPssItemLoadALL(); //전체 소유 아이템 로드
    }

    // Use this for initialization
    void Start()
    {
        ishopInstance = this;
        Instance = this;
        // Retrieve the name of this scene.
        Scene currentScene = SceneManager.GetActiveScene();
        SceneName = currentScene.name;
    }

    // Update is called once per frame
    void Update () {
		
	}

    //      Debug.Log("광고보여주기");
    //ShowRewardedVideo();
}

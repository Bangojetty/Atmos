using System;
using UnityEngine;

public class BulletContainer : MonoBehaviour {
    private ChampPage champPage;
    public int blockId;

    private void Start() {
        champPage = GameObject.Find("ChampPage").GetComponent<ChampPage>();
    }


    public void AttemptToAddBullet() {
        champPage.AttemptToAddBullet(gameObject, blockId);
    }
}

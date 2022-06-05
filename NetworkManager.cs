using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks
{
   public InputField NickNameInput;
   public GameObject DisconnectPanel;
   public GameObject GameoverPanel;
   public GameObject InGameOptionPanel;
   public GameObject QuitPanel;

// NetworkManger에서 최초로 한번 시작
   void Awake() {
       // 960 540으로 화면 빌드
       Screen.SetResolution(960, 540, false);
       PhotonNetwork.SendRate =  60;
       PhotonNetwork.SerializationRate = 30;
   }
    // 연결
   public void Connect() => PhotonNetwork.ConnectUsingSettings();
   
   public override void OnConnectedToMaster(){
       PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;
       PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 2 }, null);
   }

//Room Join callback
   public override void OnJoinedRoom(){
       DisconnectPanel.SetActive(false);
       InGameOptionPanel.SetActive(true);
       StartCoroutine("DestoryBullet");
       Spawn();
   }

   public override void OnDisconnected(DisconnectCause cause){
       DisconnectPanel.SetActive(true);
       GameoverPanel.SetActive(false);
       InGameOptionPanel.SetActive(false);
       Debug.Log("photonnetwork: disconnected");
   }
  


    IEnumerator DestoryBullet() {
        yield return new WaitForSeconds(0.2f);
        foreach (GameObject GO in GameObject.FindGameObjectsWithTag("Bullet")) GO.GetComponent<PhotonView>().RPC("DestroyRPC", RpcTarget.All);
    }


  public void Spawn() {
      Vector3 pos = (PhotonNetwork.CurrentRoom.PlayerCount == 1) ? new Vector3(-7 , -1, 0) : new Vector3(7, -1, 0); 
      PhotonNetwork.Instantiate("Player", pos, Quaternion.identity);
      GameoverPanel.SetActive(false);
  }
  


  public void QuitHandler(){
       QuitPanel.SetActive(true);
  } 

  public void QuitPanel_Cancel() {
        QuitPanel.SetActive(false);
  }

  public void QuitPanel_DisConnect(){
      if(PhotonNetwork.IsConnected) {
          PhotonNetwork.Disconnect();
      }
      QuitPanel.SetActive(false);
  }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviourPunCallbacks
{
    public InputField UserName;
    public Text StatusText;
    public Button LoginButton;

    private readonly string gameVersion = "1";

    void Awake()
    {
        Screen.SetResolution(960, 540, false);
    }

    private void Start()
    {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();

        StatusText.text = "연결중...";
    }


    public override void OnConnectedToMaster()
    {
        LoginButton.interactable = true;
        StatusText.text = "연결성공";
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        LoginButton.interactable = false;
        StatusText.text = $"연결실패 {cause.ToString()}";

        PhotonNetwork.ConnectUsingSettings();
    }

    public void Connect()
    {
        LoginButton.interactable = false;

        PhotonNetwork.LocalPlayer.NickName = UserName.text;

        if (PhotonNetwork.IsConnected)
        {
            StatusText.text = "룸으로 접속시도";
            PhotonNetwork.JoinRandomRoom();
        }
        else 
        {
            StatusText.text = "연결실패 - 다시 연결중...";
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        StatusText.text = "새로운 룸 생성";
        PhotonNetwork.CreateRoom(roomName: null, new RoomOptions { MaxPlayers = 20 });
    }

    public override void OnJoinedRoom()
    {
       StatusText.text = "룸으로 접속완료";
        //SceneManager.LoadScene("main");
        PhotonNetwork.LoadLevel("Main");
    }

}

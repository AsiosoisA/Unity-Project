using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Data.Common;

public class PlayerDataManager : MonoBehaviour
{
    

    /*
        Save 함수를 실행하면 playerData.json 이라는 파일이 뚝딱 생기고, 현재 담고 있는 playerData 의 정보가 저장된다.

        Load 함수를 실행하면 playerData.json 파일에서 데이터를 읽어와서 playerData 에 저장한다.

        간단하다!

        어떤 데이터를 저장할지 조정하고 싶다면 여기 말고 PlayerData 클래스쪽을 건들여라.



        !!! TODO !!!
        현재 세이브 데이터는 playerData 라는 이름으로 딱 하나만 생성된다.
        그러나 실제 게임 플레이에서 유저가 여러 세이브 파일을 분기마다 만들 수 있다.
        따라서 나중에는 세이브 파일의 경로를 좀 복잡하게 설정할 필요가 있다!

        세부 로직은 그냥 open() read() write() 계열 함수들 쓰고 인코딩 디코딩하고 뭐시기하는 내용. 안 봐도 된다.
    */


    string saveDataPath; // 이 경로에 세이브 파일 (Json) 이 생성된다.

    [SerializeField]
    private PlayerData playerData;
    bool isDataAccessable;

    void Start(){
        Begin();
    }

    public void Begin(){
        isDataAccessable = false;

        /*
            !!!TODO!!!
            세이브파일의 이름을 인자로 받던가 해서 세이브파일 경로를 잘 설정해주어야 한다!
            지금은 일단 그냥 임시값으로 해놓겠다.
        */
        saveDataPath = Application.dataPath + "/playerData.json";


        if(File.Exists(saveDataPath)) { // 물론 세이브데이터가 존재할 때만!
            Debug.Log("데이터를 불러옵니다!");
            Load(); 
        }

        isDataAccessable = true;
    }   

    public void Save(){
        isDataAccessable = false;

        PlayerDataForJSON data = playerData.Simplificate(); // 꼭 저장해야 하는 데이터만 간단하게 저장하기 위해 필요한 값만 추린다.
        string jsonData = JsonConvert.SerializeObject(data); // json 파일에 넣을 수 있도록 데이터를 직렬화한다.
        
        FileStream stream = new FileStream(saveDataPath, FileMode.Create); // 파일 덮어쓰기 모드로 playerData.json 파일 생성.
        byte[] byteData = Encoding.UTF8.GetBytes(jsonData); // 인코딩.
        stream.Write(byteData, 0, byteData.Length); // 파일에 쓰기.
        stream.Close(); // 파일 닫기.

        isDataAccessable = true;
    } 

    public void Load(){
        isDataAccessable = false;
        
        FileStream stream = new FileStream(saveDataPath, FileMode.Open); // 파일을 그냥 오픈 모드로 연다.
        byte[] byteData = new byte[stream.Length]; // 걍 바이트 데이터 담을 배열 하나 선언한다.
        stream.Read(byteData, 0, byteData.Length); // stream.Read 함수를 통해 파일에서 바이트 데이터를 읽어온다.
        stream.Close(); // 파일을 닫는다.

        string jsonData = Encoding.UTF8.GetString(byteData); // 그 바이트 데이터를 string 으로 인코딩한다.
        PlayerDataForJSON data = JsonConvert.DeserializeObject<PlayerDataForJSON>(jsonData); // 그 string 을 객체로 Deserialize 한다.

        playerData = new PlayerData(data); // 그 데이터를 기반으로 현재 들고 있는 playerData를 초기화한다!

        isDataAccessable = true;
    }

    public void SyncThisData(PlayerData yourData){
        StartCoroutine(WaitData(yourData));
    }

    IEnumerator WaitData(PlayerData yourData){
        while(!isDataAccessable){
            yield return null;
        }
        yourData = this.playerData;
        Debug.Log("Sync data success!");
    }

    public PlayerData getPlayerData(){
        return this.playerData;
    }
}

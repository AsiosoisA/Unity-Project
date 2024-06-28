using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveLoadSlot : MonoBehaviour
{
    public int slotNumber = -1;
    private PlayerDataPreview info;

    public GameObject emptySlotView;
    public FilledSlotView filledSlotView;
    private bool isEmpty;

    // ============================   filledSlotView 에서 쓸 정보    ================================
    public Text date;
    public Text content;
    string dateText;
    string contentText;
    // ==============================================================================================

    public void LoadData(){
        
        /*
            이 함수는 외부에서 slotNumber 를 해당 슬롯에 할당했다는 전제 하에 이루어짐.
        */
        if(slotNumber == -1){
            Debug.Log("SaveLoadSlot Error : 슬롯 번호가 초기화되지 않은 상태에서 LoadData가 호출됨!");
            return;
        } 

        info = PlayerDataManager.Instance.LoadPreview(slotNumber);

        if(info == null) // 빈 칸일 때
        {
            isEmpty = true;
            emptySlotView.SetActive(true);
        }

        else // 정보를 받아오는 것에 성공했을 때
        {
            InitSlot();
            isEmpty = false;
            filledSlotView.gameObject.SetActive(true);
        }
    }

    private void InitSlot(){
        // 이게 호출됐다는건 info 는 이미 채워져있다는 뜻.
        dateText = "" + info.timestamp_year + "." + info.timestamp_month + "." + info.timestamp_day + " " + 
        info.timestamp_hour + ":" + info.timestamp_minute + ":" + info.timestamp_second;

        // TODO 이 부분 어떻게 출력할지 나중 가면 정해야 함!
        contentText = "소지금 : " + info.money + ", 식당 레벨 : " + info.restaurantLevel;

        date.text = dateText;
        content.text = contentText;
    }

    public void OnSlotClickForSave(){
        if(isEmpty){
            SaveDataInSlot();
        }
        else{
            AlertManager.Instance.MakeAlert(
            "세이브 파일을 덮어쓰시겠습니까?",
            "기존 세이브 파일은 사라질 것입니다.",
            SaveDataInSlot,
            null
            );
        }
    }

    public void OnSlotClickForLoad(){
        /*
            데이터 불러온 뒤 해당 데이터를 토대로 적절한 씬으로 넘어감.
        */
        
        if(isEmpty){
            // 빈칸을 로드한답시고 클릭했을 때
            return; // 걍 패스
        }

        PlayerDataManager.Instance.Load(slotNumber);
        // 이러면 Instance의 PlayerData 가 초기화됐을거임.

        Debug.Log("로드 성공! PlayerData의 정보 중 소지금 : " + PlayerDataManager.Instance.data.money);
        Debug.Log("이제 적절한 씬으로 넘어가면 끝!");
    }

    public void SaveDataInSlot(){
        /*
            데이터를 저장한 뒤 성공적으로 저장이 됐다고 알려줘야 함.

            이 때, 저장된 것을 눈으로 확인할 수 있도록 해당 슬롯의 prev 데이터는 즉시 초기화할 것.
        */

        if(isEmpty) emptySlotView.SetActive(false);

        PlayerDataManager.Instance.Save(slotNumber);

        PopupManager.Instance.MakePopup("성공적으로 저장했습니다!");
        
        LoadData();
    }
}

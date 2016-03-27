using UnityEngine;
using System.Collections;

public class BtnPowerUp : MonoBehaviour {

	CardUpEvent mCardUpEvent;
	int mLevelBefore;
	GetCardInvenEvent mInvenEvent;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnClick(){
		if(name.Equals("BtnPowerUp")){
			mLevelBefore = transform.root.FindChild("CardPowerUp").GetComponent<CardPowerUp>().mTargetCard.cardLevel;
			mCardUpEvent = new CardUpEvent(ReceivedLevelUp);
			NetMgr.CardLevelUp(
				transform.root.FindChild("CardPowerUp").GetComponent<CardPowerUp>().mTargetCard,
				transform.root.FindChild("CardPowerUp").GetComponent<CardPowerUp>().mCardFeedList, mCardUpEvent);
		} else{
			mCardUpEvent = new CardUpEvent(ReceivedRankUp);
			NetMgr.CardRankUp(
				transform.root.FindChild("CardPowerUp").GetComponent<CardPowerUp>().mTargetCard,
				transform.root.FindChild("CardPowerUp").GetComponent<CardPowerUp>().mCardFeedList[0], mCardUpEvent);
		}
	}

	void ReceivedLevelUp(){
		int levelAfter = 0;
		int success = 0;
		foreach(CardUpInfo info in mCardUpEvent.Response.data){
			if(info.resultValue > 0)
				success = 1;
			if(info.cardLevel > levelAfter)
				levelAfter = info.cardLevel;

		}

		if(success > 0){
			DialogueMgr.ShowDialogue("Success", "Card Level Up!\nLevel "
			                         + mLevelBefore + " -> " + levelAfter, DialogueMgr.DIALOGUE_TYPE.Alert, ReloadInven);
		} else{
			DialogueMgr.ShowDialogue("Fail", "Level Up failed", DialogueMgr.DIALOGUE_TYPE.Alert, ReloadInven);
		}
		
	}

	void ReceivedRankUp(){
		DialogueMgr.ShowDialogue("Success", "Card Rank Up!", DialogueMgr.DIALOGUE_TYPE.Alert, ReloadInven);
	}

	void ReloadInven(DialogueMgr.BTNS btn){
		mInvenEvent = new GetCardInvenEvent(ReceivedInven);
		NetMgr.GetCardInven(mInvenEvent);
	}

	void ReceivedInven(){
		transform.root.FindChild("MyCards").GetComponent<MyCards>().Init(mInvenEvent,
             transform.root.FindChild("MyCards").GetComponent<MyCards>().GetMailEvent());
		UtilMgr.OnBackPressed();
	}
}
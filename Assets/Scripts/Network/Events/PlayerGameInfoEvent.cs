﻿using UnityEngine;
using System.Collections;

public class PlayerGameInfoEvent : BaseEvent {

	public PlayerGameInfoEvent(EventDelegate.Callback callback)
	{
		base.eventDelegate = new EventDelegate(callback);

		InitEvent += InitResponse;
	}

	public void InitResponse(string data)
	{
		response = Newtonsoft.Json.JsonConvert.DeserializeObject<PlayerGameInfoResponse>(data);

		if (checkError ())
			return;

		eventDelegate.Execute ();
	}

	public PlayerGameInfoResponse Response
	{
		get{ return response as PlayerGameInfoResponse;}
	}

}
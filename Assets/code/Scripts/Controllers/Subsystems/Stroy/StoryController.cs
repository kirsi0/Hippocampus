using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Config;
using Evidence;
using GamePlay.Global;
using GamePlay.Stage;
using StarPlatinum;
using StarPlatinum.Manager;
using StarPlatinum.StoryCompile;
using StarPlatinum.StoryReader;
using UI.Panels.StaticBoard;
using UnityEngine;

namespace Controllers.Subsystems.Story
{
	public enum StoryActionType
	{
		WaitClick,
		Name,
		Content,
		Color,
		FontSize,
		TypewriterInterval,
		Jump,
		Font,
		Waiting,
		Picture,
		PictureMove,
		Bold,
		ChangeBGM,
		ChangeEffectMusic,
		ShowEvidence,
		LoadGameScene,
		LoadMission,
		TriggerEvent,
		PlayAnimation,
		ChangeBackground,
		Wrap,
		ChangeSoundVolume,
	}

	public class StoryController : ControllerBase
	{
		public readonly string STORY_FOLDER = "Storys_Export/";
		public override void Initialize (IControllerProvider args)
		{
			base.Initialize (args);
			State = SubsystemState.Initialization;
			StartCoroutine (LoadStoryInfo ());
		}

		//public bool LoadStoryByItem (string itemId)
		//{
		//	if (m_storys == null) {
		//		return false;
		//	}
		//	bool result = true;
		//	result = m_storys.RequestLabel (itemId);
		//	if (!result) {
		//		return false;
		//	}

		//	result = IsCorrectChapter ();

		//	result = m_storys.JumpToWordAfterLabel (itemId);

		//	return result;
		//}



		private IEnumerator LoadStoryInfo ()
		{
			while (Data.ConfigProvider.StoryConfig == null) {
				yield return null;
			}
			m_config = Data.ConfigProvider.StoryConfig;
			LoadStoryFileByName (m_config.StoryPath);
			State = SubsystemState.Initialized;
		}

		public void LoadStoryFileByName (string storyFileName)
		{
			if (m_storyFileName == storyFileName) {
				return;
			}

			//Assets/Resources/STORY_FOLDER + storyFileName
			
				StoryReader storyReader = new StoryReader (STORY_FOLDER + storyFileName);
			if (storyReader != null) {
				m_storys = storyReader;
			}
			bool result = m_storys.GetLoadResult ();
			if (result == true) {
				m_storyFileName = storyFileName;
			}

		}

		public bool IsLabelExist (string label)
		{
			return m_storys.RequestLabel (label);
		}

		//private bool IsCorrectChapter ()
		//{
		//	bool result = true;
		//	result = SingletonGlobalDataContainer.Instance.Chapter == m_storys.Chapter;
		//	if (result == false) {
		//		return false;
		//	}

		//	result = SingletonGlobalDataContainer.Instance.Scene == m_storys.Scene;
		//	if (result == false) {
		//		return false;
		//	}

		//	return result;
		//}
		//private string GenerateStoryID (string itemId)
		//{
		//	return ChapterManager.Instance.GetCurrentSceneName () +
		//		GameSceneManager.Instance.GetCurrentSceneEnum ().ToString() +
		//		itemId;
		//}
		//Temp
		int left = 28;
		int right = 72;
		string hero = "Hero";
		string heroine = "Heroine";
		string jailerMan = "JailerMan";
		string jailerWoman = "JailerWoman";

		private void PushPicture (StoryActionContainer container, string leftName, string rightName)
		{
			container.PushPicture (hero, 0);
			container.PushPicture (heroine, 0);

			container.PushPicture (jailerMan, 0);
			container.PushPicture (jailerWoman, 0);


			if (leftName.Length != 0) {
				container.PushPicture (leftName, left);
			}
			if (rightName.Length != 0) {
				container.PushPicture (rightName, right);

			}
		}

		//Temp
		public StoryActionContainer GetStory (string labelId)
		{
			StoryActionContainer container = new StoryActionContainer ();

			StoryVirtualMachine.Instance.SetStoryActionContainer (container);

//			container.PushPicture("Role_Heroine",150,150);
//			container.PushWaiting(1);
//			container.PushPicture("Role_Heroine",180,150);
//			container.PushWaiting(1);
//			container.PushPicMove("Role_Heroine",180,150,120,170,Ease.Linear,3);
//			container.PushWaiting(2);
//			container.PushPicMove("Role_Heroine",150,170,190,170,Ease.Linear,2);
//			container.PushWaiting(1);
//			container.PushPicMove("Characters_Vera_Confusion",190,150,130,150,Ease.Linear,3);
//			container.PushWaiting(1);
//			container.PushPicture("Role_Heroine",100,100);
//			container.PushWaiting(100);
//
//			return container;
			//List<StoryBasicData> datas = m_storys.GetSotry();

			//for(int i = 0; i < datas.Count(); i++)
			//{
			//    if(datas[i].typename == StoryReader.NodeType.word.ToString())
			//    {
			//        StoryWordData data = datas[i] as StoryWordData;
			//        container.PushName(data.name);
			//        container.PushContent(data.content);
			//    }
			//    else if (datas[i].typename == StoryReader.NodeType.jump.ToString())
			//    {
			//        StoryJumpData data = datas[i] as StoryJumpData;
			//        container.PushJump(data.jump, );

			//    }
			//}
			if (m_storys == null) {
				Debug.LogError ("Story doesn`t exist.");
				return null;
			}

			if (!m_storys.RequestLabel (labelId)) {
				Debug.LogError ($"Label {labelId} doesn`t exist");
			} else {
				m_storys.JumpToWordAfterLabel (labelId);
			}

			//if (m_storys.Chapter == "Ep2" &&
			//	m_storys.Scene == "Pier") {
			//	PushPicture (container, hero, heroine);

			//}

			//if (labelId == "Ep2_Jeep_PoliceQuestion_0") {
			//	PushPicture (container, hero, "");
			//}

			//if (labelId == "Ep2_Jeep_PoliceQuestion_1") {
			//	PushPicture (container, hero, "");

			//}
			//if (labelId == "Ep2_Jeep_PoliceQuestion_2") {
			//	PushPicture (container, hero, "");

			//}
			//if (labelId == "Ep2_Jeep_PoliceQuestion_4") {
			//	PushPicture (container, hero, "");
			//}

			while (!m_storys.IsDone ()) {
				switch (m_storys.GetNodeType ()) {

				case StoryReader.NodeType.word:
					container.PushName (m_storys.GetName ());
					StoryVirtualMachine.Instance.Run (m_storys.GetContent ());
					m_storys.NextStory ();
					break;

				case StoryReader.NodeType.jump:
					container.PushJump (m_storys.GetJump ());
					//						m_storys.NextStory ();
					//Test
					return container;
					break;

				case StoryReader.NodeType.label:
					//m_storys.NextStory ();
					m_storys.NextStory ();
					break;
					break;

				case StoryReader.NodeType.end:
					m_storys.NextStory ();
					return container;
					break;

				case StoryReader.NodeType.exhibit:
					container.PushShowEvidence (m_storys.GetExhibit (),m_storys.GetExhibitPrefix());
					m_storys.NextStory ();
					//return container;
					break;

				case StoryReader.NodeType.raiseEvent:
					string eventName =m_storys.GetEventName ();

					switch (m_storys.GetEventType ()) {
					case StoryReader.EventType.loadScene:
						container.PushLoadGameScene (eventName);
						break;
					case StoryReader.EventType.loadMission:
						MissionEnum needLoadMission =MissionSceneManager.Instance.GetMissionEnumBy (eventName,false);
						if (needLoadMission == MissionEnum.None) {
							Debug.LogError (eventName + " is not exist.");
						} else {
							container.LoadMission (needLoadMission);
						}
						break;
					case StoryReader.EventType.invokeEvent:
						container.TriggerEvent (new StarPlatinum.EventManager.RaiseEvent (
							StoryReader.EventType.invokeEvent,
							eventName));
						break;
					case StoryReader.EventType.playAnimation:
						container.PlayAnimation (eventName);
						break;

					case StoryReader.EventType.LoadBackground:
						container.ChangeBackground (eventName);
						break;

					default:
						break;
					}
					m_storys.NextStory ();
					break;

				default:
					Debug.LogError ("Unknown Node Type");
					break;
				}


			}


			return container;
		}

		private StoryReader m_storys;
		private StoryConfig m_config;
		private string m_storyFileName;

	}
}
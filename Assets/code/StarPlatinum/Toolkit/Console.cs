﻿using System.Collections;
using System.Collections.Generic;
using StarPlatinum.Base;
using UnityEngine;
using UnityEngine.UI;

namespace StarPlatinum
{
	public class Console : Singleton<Console>
	{
		Text m_console;
		InputField m_inputField;
		Text m_inputFiledText;

		public Console ()
		{
			Transform root = GameObject.Find ("GameRoot").transform;
			if (root == null) return;

			GameObject consoleUI;
			consoleUI = new GameObject ("ConsoleUI");
			consoleUI.transform.SetParent (root);
			Canvas canvas = consoleUI.AddComponent<Canvas> ();
			canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			consoleUI.AddComponent<CanvasScaler> ();
			consoleUI.AddComponent<GraphicRaycaster> ();

			GameObject panel;
			panel = new GameObject ("Panel");
			panel.transform.SetParent (consoleUI.transform);
			panel.AddComponent<Image> ();
			panel.AddComponent<ScrollRect> ();
			Mask mask = panel.AddComponent<Mask> ();
			mask.showMaskGraphic = true;

			GameObject console;
			console = new GameObject ("Console");
			console.transform.SetParent (panel.transform);
			m_console = console.AddComponent<Text> ();
			ContentSizeFitter fitter = console.AddComponent<ContentSizeFitter> ();
			fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
			fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

			GameObject inputFiled;
			inputFiled = new GameObject ("Input_Filed");
			inputFiled.transform.SetParent (panel.transform);
			m_inputField = inputFiled.AddComponent<InputField> ();


			GameObject inputFiledText;
			inputFiledText = new GameObject ("Input_Field_Text");
			inputFiledText.transform.SetParent (inputFiled.transform);
			m_inputFiledText = inputFiledText.AddComponent<Text> ();
		}

		public void Debug (GameObject go)
		{
			Print (go + "\n");
		}

		public void Debug (string text)
		{
			Print (text + "\n");
		}

		public void Print (string text)
		{
			m_console.text += text + "\n";
		}
	}

}

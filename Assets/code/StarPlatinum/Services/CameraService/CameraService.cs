﻿using Config.GameRoot;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarPlatinum.Base;
using StarPlatinum.Manager;
using GamePlay.Stage;
using UnityEngine.Assertions;

namespace StarPlatinum.Service
{
	/// <summary>
	/// 如果没有摄像机，那么实例化一个摄像机并把固定脚本挂上去
	/// 如果有摄像机，没有固定脚本，那么把脚本挂上去
	/// 如果有摄像机，也有固定脚本，那么什么都不做
	/// 如果有多台摄像机
	/// </summary>
	public class CameraService : Singleton<CameraService>
	{
		public enum SceneCameraType
		{
			None,
			Fixed,
			Moveable
		}

		[SerializeField]
		private GameObject m_mainCamera;

		[SerializeField]
		private CameraController m_cameraController;

		public CameraService ()
		{

		}

        public void SetMainCamera(GameObject mainCamera)
        {
            if (m_mainCamera != null)
            {
                GameObject.Destroy(m_mainCamera);
            }
            m_mainCamera = mainCamera;
        }
		public GameObject GetMainCamera ()
		{
			if (m_mainCamera == null || m_mainCamera.activeInHierarchy == false) {
				Debug.LogError ("Camera is expired");
			}

			bool result = UpdateCurrentCamera ();
			if (!result) {
				return null;
			}

			return m_mainCamera;
		}

		public bool UpdateCurrentCamera ()
		{
			m_cameraController = null;

            CloseAllCamera();

			SceneLookupEnum sceneEnum = GameSceneManager.Instance.GetCurrentSceneEnum ();
			SceneCameraType cameraType = ConfigRoot.Instance.GetCameraTypeBySceneName (sceneEnum.ToString ());
			//The camera type is now only movable

			cameraType = SceneCameraType.Moveable;

			if (cameraType == SceneCameraType.Moveable) {
				m_cameraController = m_mainCamera.GetComponent<CameraController> ();
				if (m_cameraController == null) {
					m_cameraController = m_mainCamera.AddComponent<CameraController> ();

				}

				m_cameraController.Refresh ();
			}

			if (cameraType == SceneCameraType.Fixed) {
				m_cameraController = m_mainCamera.GetComponent<CameraController> ();
				if (m_cameraController != null) {
					GameObject.Destroy (m_cameraController);
				}
			}

			return true;
		}

		public void SetTarget (GameObject target)
		{
			if (m_cameraController != null) {
				m_cameraController.SetTarget (target);
			}
		}

		private void CloseAllCamera ()
		{
			Camera [] cameras = Camera.allCameras;

            Assert.IsTrue(cameras.Length > 0, "Camera length is wrong");

			for (int i = 0; i < cameras.Length; i++) {
                if (cameras[i].gameObject != m_mainCamera)
                {
                    cameras[i].gameObject.SetActive(false);
                }
			}
		}

		//public override void SingletonInit ()
		//{

		//      }

		private GameObject GetCamera ()
		{

			Camera [] cameras = Camera.allCameras;
			Debug.Log ("Camera Length:" + cameras.Length);

			if (Camera.allCamerasCount == 0) {
				Debug.Log ("This scene does`t contain a camera!");

				return null;
			}

			if (cameras.Length > 1) {
				for (int i = 1; i < cameras.Length; i++) {
					cameras [i].gameObject.SetActive (false);
					GameObject.Destroy (cameras [i].gameObject);
				}
			}

			return cameras [0].gameObject;
		}

		private GameObject CreateCamera ()
		{
			GameObject root = GameObject.Find ("GameRoot");
			if (root == null) return null;

			PrefabManager.Instance.InstantiateAsync<GameObject> ("MainCamera", (result) => {
				GameObject cameraGameObject = result.result as GameObject;

				Camera camera = cameraGameObject.GetComponent<Camera> ();
				if (camera == null) {
					cameraGameObject.AddComponent<Camera> ();
					cameraGameObject.transform.SetParent (root.transform);
				}
			});
			return camera;
		}

		private GameObject camera;
	}
}
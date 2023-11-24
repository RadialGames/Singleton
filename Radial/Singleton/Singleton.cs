using UnityEngine;

// From the Unity wiki:
// https://wiki.unity3d.com/index.php/Singleton

namespace Radial.Singleton
{
	/// <summary>
	/// Inherit from this base class to create a singleton. eg:
	/// public class MyClassName : Singleton<MyClassName> {}
	/// 
	/// Be aware this will not prevent a non singleton constructor, such as:
	/// MyClassName badActor = new MyClassName();
	/// To prevent that, you can override the constructor in your class with this:
	/// protected MyClassName() { }
	/// </summary>
	public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		// Check to see if we're about to be destroyed.
		private static bool m_ShuttingDown = false;
		private static object m_Lock = new object();
		private static T m_Instance;

		/// <summary>
		/// Access singleton instance through this propriety.
		/// </summary>
		public static T Instance
		{
			get
			{
				if (m_ShuttingDown)
				{
					Debug.LogWarning($"[{nameof(Singleton)}] Instance '{typeof(T)}' already destroyed. Returning null.");
					return null;
				}

				lock (m_Lock)
				{
					if (m_Instance == null)
					{
						// Search for existing instance.
#if UNITY_2023_1_OR_NEWER                        
						m_Instance = FindFirstObjectByType<T>();
#else
						m_Instance = FindObjectOfType<T>();
#endif

						// Create new instance if one doesn't already exist.
						if (m_Instance == null)
						{
							// Need to create a new GameObject to attach the singleton to.
							var singletonObject = new GameObject();
							m_Instance = singletonObject.AddComponent<T>();
							singletonObject.name = $"{typeof(T)} ({nameof(Singleton)})";

							// Make instance persistent.
							DontDestroyOnLoad(singletonObject);
						}
					}

					return m_Instance;
				}
			}
		}

		private void OnApplicationQuit()
		{
			m_ShuttingDown = true;
		}

		private void OnDestroy()
		{
			m_ShuttingDown = true;
		}
	}
}
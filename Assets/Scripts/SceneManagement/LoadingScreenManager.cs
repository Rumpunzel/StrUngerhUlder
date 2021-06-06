using UnityEngine;

public class LoadingScreenManager : MonoBehaviour
{
    [Header("Loading screen ")]
    public GameObject loadingInterface;

    //The loading screen event we are listening to
    [Header("Loading screen Event")]
	[SerializeField] private BoolEventChannelSO m_ToggleLoadingScreen = default;


	private void OnEnable()
	{
		if (m_ToggleLoadingScreen != null)
			m_ToggleLoadingScreen.onEventRaised += ToggleLoadingScreen;
	}

	private void OnDisable()
	{
		if (m_ToggleLoadingScreen != null)
			m_ToggleLoadingScreen.onEventRaised -= ToggleLoadingScreen;
	}


	private void ToggleLoadingScreen(bool state)
	{
		loadingInterface.SetActive(state);
	}
}

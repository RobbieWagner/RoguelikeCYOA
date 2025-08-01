using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace RobbieWagnerGames.UI
{
    public class SettingsMenu : Menu
    {
        [SerializeField] private AudioMixer mixer;
        [SerializeField] [SerializedDictionary("slider","mixer")] private SerializedDictionary<Slider, string> volumeSettings;

		[SerializeField] private Color normalBackgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.5f);
		[SerializeField] private Color selectedBackgroundColor = new Color(0.3f, 0.3f, 0.3f, 0.7f);
		[SerializeField] private Color normalFillColor = new Color(0.9f, 0.9f, 1f);
		[SerializeField] private Color selectedFillColor =  Color.white;

		protected override void Awake()
		{
			InitializeMenu();

			base.Awake();
		}

		private void InitializeMenu()
		{
			foreach (KeyValuePair<Slider, string> setting in volumeSettings)
			{
				setting.Key.maxValue = 0;
				setting.Key.minValue = -80;
				setting.Key.value = 0;

				float volume = PlayerPrefs.GetFloat(setting.Value, setting.Key.value);
				setting.Key.value = volume;
				mixer.SetFloat(setting.Value, volume);

				// Add event triggers for selection
				var trigger = setting.Key.gameObject.GetComponent<EventTrigger>() ??
							 setting.Key.gameObject.AddComponent<EventTrigger>();

				var selectEntry = new EventTrigger.Entry();
				selectEntry.eventID = EventTriggerType.Select;
				selectEntry.callback.AddListener((data) => { UpdateSliderColors(setting.Key); });
				trigger.triggers.Add(selectEntry);

				var deselectEntry = new EventTrigger.Entry();
				deselectEntry.eventID = EventTriggerType.Deselect;
				deselectEntry.callback.AddListener((data) => { ResetSliderColors(setting.Key); });
				trigger.triggers.Add(deselectEntry);

				// Set initial colors
				ResetSliderColors(setting.Key);
			}
		}

		protected override void OnEnable()
        {
            base.OnEnable();

            backButton.onClick.AddListener(SaveSettings);

			foreach (KeyValuePair<Slider, string> setting in volumeSettings)
				setting.Key.onValueChanged.AddListener((value) => SetMixerVolume(value, setting.Value));
		}

        protected override void OnDisable()
        {
            base.OnDisable();

            backButton.onClick.RemoveListener(SaveSettings);

			foreach (KeyValuePair<Slider, string> setting in volumeSettings)
				setting.Key.onValueChanged.RemoveListener((value) => SetMixerVolume(value, setting.Value));
		}

        public void SaveSettings()
        {
            foreach (KeyValuePair<Slider, string> setting in volumeSettings)
                PlayerPrefs.SetFloat(setting.Value, setting.Key.value);
		}

        private void SetMixerVolume(float value, string parameterName)
        {
			mixer.SetFloat(parameterName, value);
		}

		private void UpdateSliderColors(Slider slider)
		{
			if (EventSystem.current.currentSelectedGameObject == slider.gameObject)
			{
				SetSliderSelectedColors(slider);
			}
			else
			{
				ResetSliderColors(slider);
			}
		}

		private void SetSliderSelectedColors(Slider slider)
		{
			// Change background color (assuming the background is the first child image)
			Image background = slider.GetComponentInChildren<Image>();
			if (background != null)
			{
				background.color = selectedBackgroundColor;
			}

			// Change fill color
			Image fill = slider.fillRect?.GetComponent<Image>();
			if (fill != null)
			{
				fill.color = selectedFillColor;
			}
		}

		private void ResetSliderColors(Slider slider)
		{
			// Reset background color
			Image background = slider.GetComponentInChildren<Image>();
			if (background != null)
			{
				background.color = normalBackgroundColor;
			}

			// Reset fill color
			Image fill = slider.fillRect?.GetComponent<Image>();
			if (fill != null)
			{
				fill.color = normalFillColor;
			}
		}
	}
}
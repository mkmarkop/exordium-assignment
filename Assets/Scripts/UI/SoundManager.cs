using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : Singleton<SoundManager> {

	public Toggle MuteToggle;
	public Slider MusicSlider;
	public Slider SFXSlider;

	public AudioSource MusicSource;
	public AudioSource SFXSource;

	protected SoundManager() {}

	private static SoundManager _instance;
	private static object _lock = new object();

	private float _sfxVolume = 1f;
	private float _musicVolume = 1f;

	private bool _isMuted = false;

	void Awake() {
		_isMuted = PlayerPrefs.GetInt ("IsMuted") == 1;
		MuteToggle.isOn = _isMuted;
		if (_isMuted) {
			SFXSource.volume = 0;
			MusicSource.volume = 0;
		}

		_sfxVolume = PlayerPrefs.GetFloat ("SFXVolume", 1f);
		SFXSlider.value = _sfxVolume;
		if (!_isMuted)
			SFXSource.volume = _sfxVolume;

		_musicVolume = PlayerPrefs.GetFloat ("MusicVolume", 1f);
		MusicSlider.value = _musicVolume;
		if (!_isMuted)
			MusicSource.volume = _musicVolume;
	}

	public void PlaySFX() {
		SFXSource.Play ();
	}

	public void SetSFXVolume() {
		float volume = SFXSlider.value;
		_sfxVolume = volume;
		PlayerPrefs.SetFloat ("SFXVolume", volume);
		if (!_isMuted)
			SFXSource.volume = volume;
	}

	public void SetMusicVolume() {
		float volume = MusicSlider.value;
		PlayerPrefs.SetFloat ("MusicVolume", volume);
		_musicVolume = volume;
		if (!_isMuted)
			MusicSource.volume = volume;
	}

	public void Mute() {
		SFXSource.volume = 0f;
		MusicSource.volume = 0f;
	}

	public void ToggleMute() {
		if (MuteToggle.isOn) {
			Mute ();
		} else {
			Unmute ();
		}

		_isMuted = MuteToggle.isOn;
		PlayerPrefs.SetInt ("IsMuted", _isMuted ? 1 : 0);
	}

	public void Unmute() {
		SFXSource.volume = _sfxVolume;
		MusicSource.volume = _musicVolume;
	}

}

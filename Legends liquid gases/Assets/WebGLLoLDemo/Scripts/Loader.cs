using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using LoLSDK;
using SimpleJSON;
using System.IO;

public class SpaceData
{
	public int CURRENT_PROGRESS = 0;
	public int PLAYER_SCORE = 0;
	public int GAME1_CURRENT_TIME = 5;
	public int GAME1_NEXT_CURRENT_TIME = 5;
	public int GAME2_STATUS = 0;
	public int GAME2_DAY_TIME_MESSAGE = 0;
	public int GAME2_BG_POS = 0;
	public int GAME2_LAST_BG_POS = 0;
}

public class Loader : MonoBehaviour
{

	// Relative to Assets /StreamingAssets/
	private const string languageJSONFilePath = "language.json";
	private const string questionsJSONFilePath = "questions.json";
	private const string startGameJSONFilePath = "startGame.json";

	public static int MAX_STARS_EARNED = 0;
	public static int CURRENT_STARS_EARNED = 0;
	public static int CURRENT_LEVELS_UNLOCKED = 0;

	private int _loaderCounter = 0;
	public static void SaveData()
	{
		SpaceData spaceData = new SpaceData();
		spaceData.CURRENT_PROGRESS = Loader.MAX_STARS_EARNED;
		spaceData.PLAYER_SCORE = Loader.CURRENT_STARS_EARNED;
		spaceData.GAME1_CURRENT_TIME = Loader.CURRENT_LEVELS_UNLOCKED;
		LOLSDK.Instance.SaveState(spaceData);
	}

	void Awake()
	{
		// Create the WebGL (or mock) object
#if UNITY_EDITOR
		ILOLSDK webGL = new LoLSDK.MockWebGL();
#elif UNITY_WEBGL
			ILOLSDK webGL = new LoLSDK.WebGL();
#endif

		// Initialize the object, passing in the WebGL
		LOLSDK.Init(webGL, "com.legends-of-learning.unity.sdk.v5.1.potionworkshop");

		// Register event handlers
		LOLSDK.Instance.StartGameReceived += new StartGameReceivedHandler(this.HandleStartGame);
		LOLSDK.Instance.LanguageDefsReceived += new LanguageDefsReceivedHandler(this.HandleLanguageDefs);

		// Mock the platform-to-game messages when in the Unity editor.
#if UNITY_EDITOR
		LoadMockData();
#endif

		// Then, tell the platform the game is ready.
		LOLSDK.Instance.GameIsReady();
	}

	private void Update()
	{
		if (_loaderCounter == 2)
		{
			_loaderCounter = 3;
			SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
		}
	}

	// Start the game here
	void HandleStartGame(string json)
	{
		SharedState.StartGameData = JSON.Parse(json);
		_loaderCounter++;
	}

	// Store the questions and show them in order based on your game flow.
	void HandleQuestions(MultipleChoiceQuestionList questionList)
	{
		Debug.Log("HandleQuestions");
		SharedState.QuestionList = questionList;
	}

	// Use language to populate UI
	void HandleLanguageDefs(string json)
	{
		JSONNode langDefs = JSON.Parse(json);

		// Example of accessing language strings
		// Debug.Log(langDefs);
		// Debug.Log(langDefs["welcome"]);

		SharedState.LanguageDefs = langDefs;

		_loaderCounter++;
	}
	private void LoadMockData()
	{
#if UNITY_EDITOR
		// Load Dev Language File from StreamingAssets

		string startDataFilePath = Path.Combine(Application.streamingAssetsPath, startGameJSONFilePath);
		string langCode = "en";

		Debug.Log(File.Exists(startDataFilePath));

		if (File.Exists(startDataFilePath))
		{
			string startDataAsJSON = File.ReadAllText(startDataFilePath);
			JSONNode startGamePayload = JSON.Parse(startDataAsJSON);
			// Capture the language code from the start payload. Use this to switch fontss
			langCode = startGamePayload["languageCode"];
			HandleStartGame(startDataAsJSON);
		}

		// Load Dev Language File from StreamingAssets
		string langFilePath = Path.Combine(Application.streamingAssetsPath, languageJSONFilePath);
		if (File.Exists(langFilePath))
		{
			string langDataAsJson = File.ReadAllText(langFilePath);
			// The dev payload in language.json includes all languages.
			// Parse this file as JSON, encode, and stringify to mock
			// the platform payload, which includes only a single language.
			JSONNode langDefs = JSON.Parse(langDataAsJson);
			// use the languageCode from startGame.json captured above
			HandleLanguageDefs(langDefs[langCode].ToString());
		}

		// Load Dev Questions from StreamingAssets
		string questionsFilePath = Path.Combine(Application.streamingAssetsPath, questionsJSONFilePath);
		if (File.Exists(questionsFilePath))
		{
			string questionsDataAsJson = File.ReadAllText(questionsFilePath);
			MultipleChoiceQuestionList qs =
				MultipleChoiceQuestionList.CreateFromJSON(questionsDataAsJson);
			HandleQuestions(qs);
		}
#endif
	}
}

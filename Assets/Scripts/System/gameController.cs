using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class gameController : MonoBehaviour {

    [Tooltip("Difficulty increases when the game progresses to the next stage, starts from 1 ends at 5")]
	[Header("Difficulty")]
	[Range(1,5)]
    public int difficultyLevel = 1;
    public float secondsBeforeDifficultyIncrease = 15.0f;
	public int difficultyScale = 1;
	public float speedModifier = 1.2f;
	public float speedMaxModifier = 2.0f;
	[Range(1.0f, 3.0f)]
	public float speedModifierRateChange = 0.12f;
	public bool adminMode = false;


	[Header("Tile Spawner")]
    public GameObject floorTile;
    public GameObject ceilingTile;
    public int startingXForFutureChunks = 40;
    public int piecesToSpawnEachChunk;
    public float waitToSpawnNextChunk = 5.0f;
	public float waitToSpawnNextObstacles = 5.0f;
    public int xLengthOfTile;


	[Header("Obstacle Spawn Settings")]
	[Tooltip("Lower value = more clustered enemies")]
	[Range(0.001f, 1.0f)]
	public float frequency = .5f;
	[Range(0.0f, 1.0f)]
	public float frequencyMultiplier = 0.9f;
	public float frequencyHardcap = 0.2f;
	public int totalObjectsPerChunk = 20;
	public float distTillNextSpawn = 0.0f;
	public float bossItemDropSpawnRate = 500;
	private float timeToWaitForNext = 0.0f;
	private float currentXTiles = 0.0f;
	public float enemySpawnTime = 4.0f;
	private int enemyNum = 0;



	[Header("# objects. Make sure these add to 1")]
	[Range(0.0f, 1.0f)]
	public float percentEnemy = 0.3f;
    [Range(0.0f, 1.0f)]
    public float percentBird = 0.1f;
    [Range(0.0f, 1.0f)]
	public float percentWall = 0.45f;
	[Range(0.0f, 1.0f)]
	public float percentPowerUp = 0.1f;
	[Range(0.0f, 1.0f)]
	public float percentInvincibility = 0.05f;



	[Header("RNG objects")]
	public GameObject enemy;
    public GameObject bird;
	public GameObject powerUp;
	public GameObject invincibility;
	public GameObject wall;
	public Sprite lanternSprite;
	public Sprite barrelSprite;
	public GameObject spawner;
	//main camera for changing the color of the background
	public Camera mainCamera;
	public cameraMover mainCameraMove;
	public backgroundSpawner bgSpawner;



    private float totalEnemy;                       //30% of total objects
    private float totalBird;                        //10% of total objects
	private float totalPowerUp;						//10% of total objects
	private float totalInvincibility;				//5% of total objects	
	private float totalWall;						//45% of total objects

	private Vector3 vel;
	private Vector3 startingXPos;

	[Header("Boss Mechanics")]
	public bool bossIsActive = false;
	public bool firstBoss = true;
	public int currentBoss = 0;
	public List<GameObject> boss;
	public int bossGauge = 50;
    public GameObject startingBoss1Pos;
    public GameObject endingBoss1Pos;
	public int currentBossGauge = 0;
    public GameObject clearboard;
	[Tooltip("set the next spawn time of obstacles after boss dies.")]
	public float delayToSpawnObstacles = 4.0f;
	public float bossEmergenceTime = 3.0f;

	public clearBoard clearAllObstacles;
	[HideInInspector]
	public int boss1Shot = 0;

	public Score score;

	public Slider progressBar;
	public GameObject boss1SliderImage;
	public GameObject boss2SliderImage;


	[Header("Audio")]
	public AudioClip bossHitSFX;
	public AudioClip bosslaughSFX;
	public AudioClip enemygruntSFX;
	AudioSource _audio; 


	List<GameObject> randomObjects = new List<GameObject> ();

	void Start () {
		startingXPos = mainCamera.GetComponent<Transform>().position;

		//temp thing, delete after boss gauge
		score = score.GetComponent<Score>();

		//Debug.Log ("Aspect Ratio: " + mainCamera.);

		spawner.transform.position = new Vector3 (startingXPos.x * mainCamera.aspect * 2.0f, spawner.transform.position.y, 0);
		currentXTiles = spawner.transform.position.x;
        //StartCoroutine(SpawnNextChunk());
		StartCoroutine(EnemyObjectSpawner());
        //StartCoroutine(BirdObjectSpawner());
        StartCoroutine(IncreaseDifficulty());
		StartCoroutine (SetVelocity ());

		progressBar.minValue = 0;
		progressBar.maxValue = 20;

		//make sure lantern is the first sprite
		wall.gameObject.GetComponent<SpriteRenderer> ().sprite = lanternSprite;

		_audio = GetComponent<AudioSource> ();
		if (_audio == null) {
			Debug.LogWarning ("AudioSource component missing from this game object. Adding one.");
			_audio = gameObject.AddComponent <AudioSource> ();
		}

		enemyNum = 0;
	}
	void PlaySound(AudioClip clip, float volume){
		_audio.PlayOneShot (clip, volume);
	}
	
	void Update () {
		if (mainCamera == null) {
			mainCamera = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera>();
		}
		//check if score required for boss is met
		//if ((currentBossGauge > bossGauge ||  GetComponent<Score>().PlayerScore >  1000) && bossIsActive == false) {
		if ((progressBar.value >=  progressBar.maxValue) && bossIsActive == false) {
			Debug.Log (score.PlayerScore);
			StartCoroutine(SetActiveBoss ());

			clearAllObstacles.Sweet ();
			//set the next spawn time of obstacles after boss dies.
			timeToWaitForNext = delayToSpawnObstacles;
		}

	}
		
	//Difficulty settings
	private IEnumerator IncreaseDifficulty()
	{
		while (true)
		{
			yield return new WaitForSeconds(secondsBeforeDifficultyIncrease);
			if (!bossIsActive) {
				difficultyLevel += difficultyScale;
				totalObjectsPerChunk += difficultyLevel;
				if (frequency > frequencyHardcap) {
					frequency *= frequencyMultiplier;
				}
				if (speedModifier < speedMaxModifier) {
					speedModifier += speedModifierRateChange;
				}
			}
			UpdateAllSpeed ();
			Debug.Log ("Difficulty has increased: "+ totalObjectsPerChunk + "\n the time is: " + Time.time);
		}
	}
	public void UpdateAllSpeed()
	{
		//mainCameraMove.GetComponent<cameraMover> ().UpdateSpeed (speedModifier);
		mainCameraMove.UpdateSpeed (speedModifier);
		enemy.GetComponent<enemyMove> ().UpdateEnemySpeed (speedModifier);
        bird.GetComponent<enemyMove>().UpdateEnemySpeed(speedModifier);
		bgSpawner.UpdateBGSpawnTime (speedModifier);
	}
	//Tile Spawning
	/*
    private IEnumerator SpawnNextChunk()
    {
		yield return new WaitForSeconds (5.0f);

        while (true)
        {
            for (int i = 0; i < piecesToSpawnEachChunk; i++)
            {
                SpawnCeilingPiece(startingXForFutureChunks + (i * xLengthOfTile));
                SpawnFloorPiece(startingXForFutureChunks + (i * xLengthOfTile));
            }
            startingXForFutureChunks += piecesToSpawnEachChunk * xLengthOfTile; 
			//Debug.Log("pieces length per speed: " + (piecesToSpawnEachChunk * xLengthOfTile));
            yield return new WaitForSeconds(waitToSpawnNextChunk);


        }
    }

	*/
	/*
	private void SpawnCeilingPiece(int xValueOfNextCeiling)
	{
		GameObject newCeilingPiece = Instantiate(floorTile);
		newCeilingPiece.transform.position = new Vector3(xValueOfNextCeiling, 9.5f, 0);
	}

	private void SpawnFloorPiece(int xValueOfNextFloor)
	{
		GameObject newFloorPiece = Instantiate(floorTile);
		newFloorPiece.transform.position = new Vector3(xValueOfNextFloor, 0.5f, 0);
	}
	*/

	private IEnumerator SetVelocity()
	{
		yield return new WaitForSeconds (1.0f);
		vel = mainCamera.velocity;
		currentXTiles = spawner.transform.position.x;
//		Debug.Log (vel);

		StartCoroutine (MasterObjectSpawner (randomObjects));
	}

	//Obstacle Spawner
	private IEnumerator MasterObjectSpawner(List<GameObject> objList)
	{
		while (!bossIsActive) {
			Debug.Log ("Master Spawning");

			yield return new WaitForSeconds (timeToWaitForNext);
			vel = mainCamera.velocity;
			PopulateList (objList);

			for (int i = 0; i < objList.Count (); i++) {
				GameObject newObject = Instantiate (objList [i].gameObject);
				//Debug.Log("Starting X when: " + newObject + " was made: " + (currentXTiles + (100* frequency * (i+1)) + 
				//	"\nFrequency: " + frequency + " and i is: " + (i+ 1)));
				newObject.name = objList [i].name + i;
				newObject.transform.position = new Vector3 ((currentXTiles ) + (100 * frequency * (i + 1)), Random.Range (1.5f, 8.5f), 0.0f);
			}
			//CheckList (objList);
			float distance = (100 * frequency * objList.Count ());
			float rate = vel.x;
			currentXTiles += distance;

			timeToWaitForNext = distance / rate;
			Debug.Log ("Time to wait until next deployment of obstacles: " + timeToWaitForNext);

			ClearList (ref objList);
		}
	}

	private IEnumerator EnemyObjectSpawner()
	{
		while (!bossIsActive) {
			Debug.Log ("Enemy SPawning");

			yield return new WaitForSeconds (enemySpawnTime);
            float rand = Random.Range(0.0f, 1.0f);
            if (rand > 0.3f)
            {
                GameObject newObject = Instantiate(bird);
                newObject.name = newObject.name + enemyNum;
                enemyNum++;
                newObject.transform.position = new Vector3((currentXTiles + 50) + (100 * frequency * (0 + 1)), Random.Range(1.5f, 8.5f), 0.0f);
            }
            else
            {
                GameObject newObject = Instantiate(enemy);
                newObject.name = newObject.name + enemyNum;
                enemyNum++;
                newObject.transform.position = new Vector3((currentXTiles + 50) + (100 * frequency * (0 + 1)), Random.Range(1.5f, 8.5f), 0.0f);
            }
			
			
	
		}
	}

   

    private void PopulateList(List<GameObject> objList)
	{
		//generates number between minimum and maximum-1
		totalEnemy = Mathf.Floor(totalObjectsPerChunk * percentEnemy);
        totalBird = Mathf.Floor(totalObjectsPerChunk * percentBird);
		//at least one power up
		totalPowerUp = Mathf.Ceil(totalObjectsPerChunk * percentPowerUp); 
		totalInvincibility = Mathf.Floor(totalObjectsPerChunk * percentInvincibility);
		totalWall = totalObjectsPerChunk - totalInvincibility - totalPowerUp - totalEnemy;

		Debug.Log("Enemies: " + totalEnemy);
        Debug.Log("Birds: " + totalBird);
		Debug.Log("Power Ups: " + totalPowerUp);
		Debug.Log("Invincility: " + totalInvincibility);
		Debug.Log("Walls: " + totalWall);
		Debug.Log ("Total Objects: " + totalObjectsPerChunk);

		for (int i = 0; i < totalObjectsPerChunk; i++) {
			int rand = Random.Range (0, 5);
            if (rand == 0 && totalEnemy > 0)
            {
                if (!bossIsActive)
                    objList.Add(enemy);
                totalEnemy--;
                //Debug.Log("Added " + i + " enemy");
            }
            else if (rand == 1 && totalPowerUp > 0)
            {
                objList.Add(powerUp);
                totalPowerUp--;
                //Debug.Log("Added " + i + " power up");
            }
            else if (rand == 2 && totalWall > 0)
            {
                if (!bossIsActive)
                    objList.Add(wall);
                totalWall--;
                //Debug.Log("Added " + i + " wall");
            }
            else if (rand == 3 && totalInvincibility > 0)
            {
                objList.Add(invincibility);
                totalInvincibility--;
            }
            else if (rand == 3 && totalInvincibility > 0)
            {
                objList.Add(bird);
                totalBird--;
            }
            else
            {
                //catches any excess and forces system to "try again" in 
                //allocating an object to index
                i--;
            }

		}
	}

	private void ClearList(ref List<GameObject> objList)
	{
		objList.RemoveRange(0, objList.Count);
	}

	private void CheckList(List<GameObject> objList)
	{
		if (objList.Count () > 0) {
			for (int i = 0; i < objList.Count (); i++) {
				Debug.Log ("For object (" + (i + 1) + "), there is a: " + objList [i].gameObject + "\n it is Spawning at: " + objList[i].transform.position);
			}
		} else {
			Debug.Log ("Nothing here");
		}
	}

	private IEnumerator BossObjectSpawner(List<GameObject> objList)
	{
		while (bossIsActive) {
			Debug.Log ("Boss Spawning Objects");
			yield return new WaitForSeconds (timeToWaitForNext);
			int j = 0;
			float newFreq = frequency * 0.8f;
			vel = mainCamera.velocity;
			PopulateList (objList);
			j = objList.Count ();
			for (int i = 0; i < j; i++) {
				GameObject newObject = Instantiate (objList [i].gameObject);
				//Debug.Log("Starting X when: " + newObject + " was made: " + (currentXTiles + (100* frequency * (i+1)) + 
				//	"\nFrequency: " + frequency + " and i is: " + (i+ 1)));
				newObject.name = objList [i].name + i;
				newObject.transform.position = new Vector3 (currentXTiles + (bossItemDropSpawnRate * newFreq * (i + 1)), Random.Range (1.5f, 8.5f), 0.0f);
				Debug.Log ("Spawned a powerup somewhere");
			}
			//CheckList (objList);
			float distance = (bossItemDropSpawnRate * newFreq * objList.Count ());
			float rate = vel.x;
			currentXTiles += distance;

			timeToWaitForNext = distance / rate;
			Debug.Log ("Time to wait until next deployment of obstacles: " + timeToWaitForNext);

			ClearList (ref objList);
		}
	}

	//Boss
	private IEnumerator SetActiveBoss(){
		int rand = Random.Range (0, boss.Count);
		currentBoss = rand;
        bossIsActive = true;
		clearboard.GetComponent<clearBoard>().Sweet();
		yield return new WaitForSeconds (bossEmergenceTime);
		if (firstBoss) {
			firstBoss = false;
			currentBoss = 0;
			BossSpawn (currentBoss);
		} else {
			BossSpawn (currentBoss);
		}
    }

	private void BossSpawn(int randNum)
	{
		bool remainingBosses = false;
		Debug.Log ("Spawned a boss");
		//moved these from the Update
		//{
		StopCoroutine (MasterObjectSpawner (randomObjects));
		StartCoroutine (BossObjectSpawner (randomObjects));
		//}
		//Checks if there are any remaining bosses that hasn't
		//been seen yet
		for (int i = 0; i < boss.Count; i++) {
			if (boss [i].GetComponent<bossHealth> ().spawned == false) {
				remainingBosses = true;
				PlaySound (bosslaughSFX, 0.4f);
			} 
		}

		//after a full cycle of bosses, set all the spawns to false
		//to spawn all the types of bosses again.
		if (remainingBosses == false) {
			for (int i = 0; i < boss.Count; i++) {
				boss [i].GetComponent<bossHealth> ().spawned = false;
			}
		}


		if (boss [randNum].GetComponent<bossHealth> ().spawned == false) {
            StartCoroutine(lerptheBoss());
            boss [randNum].SetActive (true);
			boss[randNum].GetComponent<BoxCollider2D> ().enabled = true;
			if (randNum == 0)
				boss [randNum].GetComponent<bossAttacks> ().StartAttackCycle ();
			
			boss [randNum].GetComponent<bossHealth> ().spawned = true;
		} else {
			randNum = Random.Range (0, boss.Count);
			//recursive it if the current boss hasn't been spawned yet. WIP-----------
			BossSpawn (randNum);
            
        }
	}

    private IEnumerator lerptheBoss()
    {
        boss[0].transform.position = startingBoss1Pos.transform.position;

        for (int i = 0; i < 500; i++)
        {
            boss[0].transform.position = Vector3.Lerp(boss[0].transform.position, endingBoss1Pos.transform.position, Time.deltaTime * 1.0f);
            yield return new WaitForSeconds(.01f);
        }
       
    }

	public void BossDeath()
	{
		Debug.Log ("BOSS IZ DEAD. EZ");
		timeToWaitForNext = delayToSpawnObstacles;
		bossIsActive = false;
		clearAllObstacles.Sweet ();
		currentXTiles = spawner.transform.position.x;
		StopCoroutine (BossObjectSpawner (randomObjects));

		StartCoroutine (MasterObjectSpawner (randomObjects));
		StartCoroutine (EnemyObjectSpawner ());

		currentBossGauge = 0;
		bossGauge += 10;
		progressBar.maxValue += 10;
		progressBar.value = 0;
		progressBar.value += 5;
	}
	public void SwitchObstacleSprite(){
		SpriteRenderer sr = wall.gameObject.GetComponent<SpriteRenderer> ();
		if (sr.sprite == lanternSprite) {
			sr.sprite = barrelSprite;
		} else {
			sr.sprite = lanternSprite;
		}
	}	
	public void switchBossSliderImage(){
		if (boss1SliderImage.activeSelf == true) {
			boss1SliderImage.SetActive (false);
			boss2SliderImage.SetActive (true);
		} else {
			boss1SliderImage.SetActive (true);
			boss2SliderImage.SetActive (false);
		}
	}
}

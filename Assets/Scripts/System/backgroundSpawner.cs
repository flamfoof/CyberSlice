using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backgroundSpawner : MonoBehaviour {

    
    public List<GameObject> foregrounds = new List<GameObject>();
    public List<GameObject> backgrounds = new List<GameObject>();

	public GameObject reactorForeground;
	public GameObject reactorMidGround;
	public GameObject reactorBackground1;
	public GameObject reactorBackground2;
	public GameObject reactorBackground3;

    private GameObject foreground;
    private GameObject background;
	private GameObject reactorBackground;
    public int numOfBackgroundObjectsBeforePlay;
    public float timeTillNextSpawn;
	public float origTimeTillNextSpawn;
    private float nextXPosOfSpawn;
    private float lengthOfBackground;

	private List<GameObject> activeBackgrounds = new List<GameObject> ();
	public GameObject player;
	private GameObject farthestBackground;

	private bool pauseSpawning = false;
	public bool spawnSecondLevel = false;

	// Use this for initialization
	void Start () {
        foreground = foregrounds[0];
        background = backgrounds[0];
        //lengthOfBackground = background.gameObject.transform.lossyScale.x * (18.0f/18.75f) ;
        lengthOfBackground = 18.75f;
        nextXPosOfSpawn = numOfBackgroundObjectsBeforePlay * lengthOfBackground;
//        Debug.Log(nextXPosOfSpawn);
        StartCoroutine(SpawnBackground());
		origTimeTillNextSpawn = timeTillNextSpawn;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private IEnumerator SpawnBackground()
    {
        while (true) {
			if (!pauseSpawning) {
				if (!spawnSecondLevel) {
					//spawn first level backgrounds
					float randomForeground = Random.Range (0.0f, 1.0f);
					float randomBackground = Random.Range (0.0f, 1.0f);

					if (randomForeground < 0.5f) {
						foreground = foregrounds [0];
					} else {
						foreground = foregrounds [1];
					}
					if (randomBackground < 0.5f) {
						background = backgrounds [0];
					} else {
						background = backgrounds [1];
					}
					GameObject nextForegroundPiece = Instantiate (foreground);
					GameObject nextBackgroundPiece = Instantiate (background);
					nextForegroundPiece.transform.position = new Vector3 (nextXPosOfSpawn, 5.0f, 1.2f);
					nextBackgroundPiece.transform.position = new Vector3 (nextXPosOfSpawn, 5.0f, 2f);
					nextXPosOfSpawn += lengthOfBackground;
				} else {
					// spawn second level backgrounds
					float randomNumDetermineBackgound = Random.Range(0,3);

					if (randomNumDetermineBackgound < 1.0f) {
						reactorBackground = reactorBackground1;
					} else if (randomNumDetermineBackgound < 2.0f) {
						reactorBackground = reactorBackground2;
					} else {
						reactorBackground = reactorBackground3;
					}
					GameObject nextBackgroundPiece = Instantiate (reactorBackground);
					GameObject nextMidgroundPiece = Instantiate (reactorMidGround);
					GameObject nextForegroundPiece = Instantiate (reactorForeground);
					nextForegroundPiece.transform.position = new Vector3 (nextXPosOfSpawn, 5.0f, 1.2f);
					nextMidgroundPiece.transform.position = new Vector3 (nextXPosOfSpawn, 5.0f, 1.4f);
					nextBackgroundPiece.transform.position = new Vector3 (nextXPosOfSpawn, 5.0f, 2.0f);
					nextXPosOfSpawn += lengthOfBackground;
				}
			}
            
            yield return new WaitForSeconds(timeTillNextSpawn);
        }
        
    }

	public void UpdateBGSpawnTime(float num)
	{
		timeTillNextSpawn = origTimeTillNextSpawn / num;
	}

	public void clearBackgrounds4AwayFromPlayer(){
		pauseSpawning = true;
		List<GameObject> activeBackgrounds = new List<GameObject>(GameObject.FindGameObjectsWithTag ("Background"));
		Debug.Log ("active background" + activeBackgrounds.Count);
		foreach (GameObject background in activeBackgrounds) {
			if (Vector2.Distance(player.transform.position, background.transform.position) < 70.0f){
			}
			else {
				Destroy (background);
			}



		}
		activeBackgrounds.Clear ();
		activeBackgrounds = new List<GameObject>(GameObject.FindGameObjectsWithTag ("Background"));
		foreach (GameObject background in activeBackgrounds) {
			if (farthestBackground == null & Vector2.Distance(player.transform.position, background.transform.position) < 70.0f) {
				farthestBackground = background;
			}
		
			if ((background.transform.position.x > farthestBackground.transform.position.x) && Vector2.Distance(player.transform.position, background.transform.position) < 70.0f) {
			farthestBackground = background;
		}
		}
		//farthestBackground.GetComponent<SpriteRenderer> ().color = Color.red;
		Debug.Log (farthestBackground.name + " name and position " + farthestBackground.gameObject.transform.position.x);
		nextXPosOfSpawn = farthestBackground.transform.position.x;
		spawnSecondLevel = !spawnSecondLevel;
		pauseSpawning = false;

	}
}

using UnityEngine;
using System;
using System.Collections.Generic; 		//Allows us to use Lists.
using Random = UnityEngine.Random; 		//Tells Random to use the Unity Engine random number generator.

namespace Completed
	
{
	
	public class BoardManager : MonoBehaviour
	{
		// Using Serializable allows us to embed a class with sub properties in the inspector.
		[Serializable]
		public class Count
		{
			public int minimum; 			//Minimum value for our Count class.
			public int maximum; 			//Maximum value for our Count class.
			
			
			//Assignment constructor.
			public Count (int min, int max)
			{
				minimum = min;
				maximum = max;
			}
		}
		
		
		public int columns = 15; 										//Number of columns in our game board.
		public int rows = 15;											//Number of rows in our game board.
		//public Count wallCount = new Count (5, 9);						//Lower and upper limit for our random number of walls per level.
		//public Count foodCount = new Count (1, 5);						//Lower and upper limit for our random number of food items per level.
		public GameObject exit;											//Prefab to spawn for exit.
		public GameObject[] floorTiles;									//Array of floor prefabs.
		public GameObject[] wallTiles;									//Array of wall prefabs.
		public GameObject[] foodTiles;									//Array of food prefabs.
		public GameObject[] enemyTiles;									//Array of enemy prefabs.
		public GameObject[] outerWallTiles;								//Array of outer tile prefabs.
        public GameObject spawnPointTile;
		
		private Transform boardHolder;									//A variable to store a reference to the transform of our Board object.
		private List <Vector3> gridPositions = new List <Vector3> ();	//A list of possible locations to place tiles.


        public Vector3 exitPosition = Vector3.zero;
        private bool isExitSetup = false;
        private Vector3 spawnPoint;
        private LevelStrategy levelStrategy;
        private int startingEnemyCount;
        private RangeInt wallCountRange;
         

        void initLevelStrategy(int level) 
        {
            this.levelStrategy = new LevelStrategy(level);

            // map size
            this.columns = (int)this.levelStrategy.MapSize.x;
            this.rows = (int)this.levelStrategy.MapSize.y;

            // enemy spawn rate
            GameManager.instance.enemySpawnRate = this.levelStrategy.EnemySpawnRate;

            // boxes amount
            this.wallCountRange = this.levelStrategy.WallCountRange;


            // starting enemy count
            this.startingEnemyCount = this.levelStrategy.StartEnemyAmount;
        }

        bool isSafeArea(int x, int y) {

            // player first position
            if (x >= 0 && x <= 1 && y >= 0 && y <= 1) return false;
            // spawn point
            if (x >= columns-2 && x <= columns-1 && y >= rows-2 && y <= rows-1) return false;
            // outer wall inside
            if ((x + 1) % 2 == 0 && (y + 1) % 2 == 0) return false;

            return true;
        }

		//Clears our list gridPositions and prepares it to generate a new board.
		void InitialiseList ()
		{
			//Clear our list gridPositions.
			gridPositions.Clear ();
			
			//Loop through x axis (columns).
			for(int x = 0; x < columns; x++)
			{
				//Within each column, loop through y axis (rows).
				for(int y = 0; y < rows; y++)
				{
                    //At each index add a new Vector3 to our list with the x and y coordinates of that position.
                    if (isSafeArea(x,y))
                        gridPositions.Add (new Vector3(x, y, 0f));
				}
			}
		}

        void SpawnPointSetup() 
        {

            spawnPoint = Vector3.right * (columns - 1) + Vector3.up * (rows - 1);
            Instantiate(spawnPointTile, spawnPoint, Quaternion.identity);
        }
		
		
		//Sets up the outer walls and floor (background) of the game board.
		void BoardSetup ()
		{
			//Instantiate Board and set boardHolder to its transform.
			boardHolder = new GameObject ("Board").transform;
			
			//Loop along x axis, starting from -1 (to fill corner) with floor or outerwall edge tiles.
			for(int x = -1; x < columns + 1; x++)
			{
				//Loop along y axis, starting from -1 to place floor or outerwall tiles.
				for(int y = -1; y < rows + 1; y++)
				{
					//Choose a random tile from our array of floor tile prefabs and prepare to instantiate it.
					GameObject toInstantiate = floorTiles[Random.Range (0,floorTiles.Length)];

                    //Check if we current position is at board edge, if so choose a random outer wall prefab from our array of outer wall tiles.
                    if(x == -1 || x == columns || y == -1 || y == rows || ((x+1) % 2 == 0 && (y + 1) % 2 == 0))
                        toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
					
					//Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
					GameObject instance =
						Instantiate (toInstantiate, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;

					//Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
					instance.transform.SetParent (boardHolder);
				}
			}
		}
		
		
		//RandomPosition returns a random position from our list gridPositions.
		Vector3 RandomPosition ()
		{
            return RandomPosition(true);

        }
        Vector3 RandomPosition(bool needRemoveFromGridPositions) 
        {
            //Declare an integer randomIndex, set it's value to a random number between 0 and the count of items in our List gridPositions.
            int randomIndex = Random.Range(0, gridPositions.Count);

            //Declare a variable of type Vector3 called randomPosition, set it's value to the entry at randomIndex from our List gridPositions.
            Vector3 randomPosition = gridPositions[randomIndex];

            //Remove the entry at randomIndex from the list so that it can't be re-used.
            if (needRemoveFromGridPositions)
                gridPositions.RemoveAt(randomIndex);

            //Return the randomly selected Vector3 position.
            return randomPosition;

        }

        //LayoutObjectAtRandom accepts an array of game objects to choose from along with a minimum and maximum range for the number of objects to create.
        void LayoutObjectAtRandom (GameObject[] tileArray, int minimum, int maximum, bool needSetupExit  )
		{
			//Choose a random number of objects to instantiate within the minimum and maximum limits
			int objectCount = Random.Range (minimum, maximum+1);
			
			//Instantiate objects until the randomly chosen limit objectCount is reached
			for(int i = 0; i < objectCount; i++)
			{
				//Choose a position for randomPosition by getting a random position from our list of available Vector3s stored in gridPosition
				Vector3 randomPosition = RandomPosition();

                // do not layout any objects at outwall positions
                //if ((int)(randomPosition.x + 1) % 2 == 0 &&  (int)(randomPosition.y + 1) % 2 == 0)
                    //continue;
				
				//Choose a random tile from tileArray and assign it to tileChoice
				GameObject tileChoice = tileArray[Random.Range (0, tileArray.Length)];
				
				//Instantiate tileChoice at the position returned by RandomPosition with no change in rotation
				Instantiate(tileChoice, randomPosition, Quaternion.identity);


                if (needSetupExit && exitPosition==Vector3.zero) {
                    exitPosition = randomPosition;
                }
			}
		}
		
		
		//SetupScene initializes our level and calls the previous functions to lay out the game board
		public void SetupScene (int level)
		{
            // the first steup to do
            initLevelStrategy(level);

            //Creates the outer walls and floor.
            BoardSetup ();
            SpawnPointSetup();

            // clear exit settings
            isExitSetup = false;
            exitPosition = Vector3.zero;

            //Reset our list of gridpositions.
            InitialiseList ();

            //Instantiate a random number of wall tiles based on minimum and maximum, at randomized positions.
            LayoutObjectAtRandom (wallTiles, this.wallCountRange.start, this.wallCountRange.end, true);

            //Instantiate a random number of food tiles based on minimum and maximum, at randomized positions.
            //LayoutObjectAtRandom (foodTiles, foodCount.minimum, foodCount.maximum);

            //Determine number of enemies based on current level number, based on a logarithmic progression
            int enemyCount = this.startingEnemyCount; //2 + (int)Mathf.Log(level, 2f);
			
			//Instantiate a random number of enemies based on minimum and maximum, at randomized positions.
			LayoutObjectAtRandom (enemyTiles, enemyCount, enemyCount, false);


            //Instantiate the exit tile in the upper right hand corner of our game board
            //Instantiate (exit, new Vector3 (columns - 1, rows - 1, 0f), Quaternion.identity);
        }

        public void setupExitIfNecessary(Vector3 position) {
            if (!isExitSetup && exitPosition == position){
                Instantiate(exit, exitPosition, Quaternion.identity);
                isExitSetup = true;
            }
        }
        void logVector3(Vector3 v, string p)
        {
            Debug.Log(string.Format("{0}:x={1},y={2},z={3}", p, v.x, v.y, v.z));
        }

        public void playParticleOnSpawnPoint() {
            // workaround to solve the problem of particle system doesn't work if get component form spawn point directly.
            var holder = GameObject.Find("ParticleSpawn");
            var particle = holder.GetComponent<ParticleSystem>();
            particle.Play();           

        }
        public void SpawnEnemy() {
            playParticleOnSpawnPoint();
            this.StartCoroutine(() =>
            {
                GameObject tileChoice = enemyTiles[Random.Range(0, enemyTiles.Length)];
                Instantiate(tileChoice, spawnPoint, Quaternion.identity);
            }, 1f);
        }

        public void TeleportEnemy(Enemy enemy) {
            var newPosition = RandomPosition(false);
            enemy.Teleport(newPosition);
        }
	}


    public class LevelStrategy
    {

        public RangeInt WallCountRange;
        public Vector2 MapSize = Vector2.zero;
        public int StartEnemyAmount = 2;
        public float EnemySpawnRate = 10f;

       
        private float enemySpawnRateMax = 60.0f;
        private float enemySpawnRateMin = 10.0f;
        private float baseRows = 7f;
        private float baseColumns = 11f;
        public LevelStrategy (int level) {

            var weight = (int)Mathf.Log(level, 2f);
            var oddWeight = weight % 2 == 1 ? weight + 1 : weight;

            // starting enemy amount
            this.StartEnemyAmount = 2 + weight;

            // map size
            this.MapSize = new Vector2(baseColumns + oddWeight, baseRows + oddWeight);

            // enemy spawing rate
            var enemyRate = enemySpawnRateMax - (level - 1) * 2;
            this.EnemySpawnRate = Mathf.Clamp(enemyRate, enemySpawnRateMin, enemySpawnRateMax);

            // wall count
            int emptyFloorEstimation = (int)MapSize.x * (int)MapSize.y * 3 / 4;
            if (level <= 5) {
                this.WallCountRange = new RangeInt((int)(0.10 * emptyFloorEstimation), (int)(0.25 * emptyFloorEstimation));
            } else {
                this.WallCountRange = new RangeInt((int)(0.25 * emptyFloorEstimation), (int)(0.75 * emptyFloorEstimation));
            }

        }

    }


}

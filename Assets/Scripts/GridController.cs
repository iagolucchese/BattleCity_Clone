using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GridController : MonoBehaviour {

	public GameController gameController;
	public GameObject spawnerPrefab;

	public int numberOfLines, numberOfColumns; //how big you want the map to be, x lines by y columns
	public float squareSizePixels; //meant to be how big in pixels you want each square sprite to be, no matter it's original size
	public GameObject prefabBlankQuad,boundaries; //references: one to the prefab for each tile, the other to the boundaries of the game
	public TextAsset mapTextFile;
	public List<GameObject> terrainPrefabsList;

	//private GameObject[,] gridMatrix; //matrix to hold all the grid elements, useful for map making later on
	private float scaleFactorForX, scaleFactorForY; //these are basically padding for the grid's squares, because grid positions should start from the top left corner, not the unity default, center
	private float squareSizeInUnityScale;

	void Start () 
	{
		//gridMatrix = new GameObject[numberOfLines,numberOfColumns];
		squareSizeInUnityScale = prefabBlankQuad.transform.localScale.x*squareSizePixels; //converts the pixel size to unity scale size
		scaleFactorForX = boundaries.transform.lossyScale.x/-2 + squareSizeInUnityScale/2;
		scaleFactorForY = boundaries.transform.lossyScale.y/2 - squareSizeInUnityScale/2;

		newParseMap();
	}

	void newParseMap(){
		string[] mapTextLines = mapTextFile.text.Split('\n'); //split the text file by line break, each line is an array element
		int mapfileLines = int.Parse(mapTextLines[1].Split(',')[0]);
		int	mapfileColumns = int.Parse(mapTextLines[1].Split(',')[1]);
		string[,] mapfileToSpritesMatrix = new string[mapfileLines,mapfileColumns]; //creates a matrix of integers that is the size that the mapfile's second line determines

		/*some more complex code here to try and fill the mapfileToSprites matrix, by turning the data from the 
		mapfile into something the game understands better; its also here that the mapfile's specific 
		sintax comes into play; if the sintax ever changes, this stops working */
		for (int i=2; i < mapTextLines.Length; i++){
			if (mapTextLines[i] != "") //ignores any empty lines
			{
				int j=0,k=0;
				foreach (string block in mapTextLines[i].Split(',')) { //does the splitting by chars
					string[] blockSplit = block.Split('@'); //here too
					for (k = j; k < (j + int.Parse(blockSplit[0])); k++)
					{
						mapfileToSpritesMatrix[k,i-2] = blockSplit[1]; //and finally adds one string with the block's name, one for each block
					}
					j=k;
				}
			}
		}

		//iterates through the matrix that we just filled, and instantiates the prefab named in it, in its respective position
		for(int i = 0; i < numberOfLines; i++)
		{
			for(int j = 0; j < numberOfColumns; j++)
			{
				string tileName = mapfileToSpritesMatrix[i,j];
				Vector3 position = new Vector3((squareSizePixels/100)*i+scaleFactorForX,(squareSizePixels/100)*(numberOfColumns-j-1)-scaleFactorForY,this.transform.position.z);

				GameObject prefab = prefabBlankQuad;

				if (tileName == "enemySpawner") { //if its a spawner, there's specific code for that
					GameObject newSpawner = Instantiate(spawnerPrefab,position,Quaternion.identity) as GameObject;
					gameController.enemySpawnerScript.listOfEnemySpawners.Add(newSpawner);
					newSpawner.name = "EnemySpawner(" + i + "," + j + ")";
				} 
				else if (tileName == "playerSpawner") { //same here, but for player spawner instead
					GameObject newSpawner = Instantiate(spawnerPrefab,position,Quaternion.identity) as GameObject;
					gameController.playerSpawner = newSpawner;
					newSpawner.name = "PlayerSpawner(" + i + "," + j + ")";
				} 
				else if (tileName != "blank") { //if it is anything but a spawner or a blank tile
					prefab = terrainPrefabsList.Find(p => p.name == tileName); //Find the first object in the list that has the same name as the string in the mapfile matrix, in other words, finds the prefab with that name
					if (!prefab) {
						throw new UnityException("An element in the mapfile (" + tileName + ") has no match in the terrain prefab list!");
					} else {
						GameObject newTile = Instantiate(prefab,position,this.transform.rotation) as GameObject; //finally instantiates the new tile
						newTile.transform.parent = this.transform; //parented to this grid
						newTile.name = tileName + " (" + i + "," + j + ")"; //fancy name for it
					}
				}
			}
		}
		//finished parsing succesfully, therefore tell game to spawn player
		gameController.SpawnPlayer();
	}
	/* old code, safe to ignore

	void parseMapFile()
	{
		string[] mapTextLines = mapTextFile.text.Split('\n'); //split the text file by line break, each line is an array element
		Sprite[] terrainSpritesArray = Resources.LoadAll<Sprite>("Sprites/" + mapTextLines[0]); //load the file with all the sprites for the terrain, based on the first line on the mapfile; each element of the array is one sprite of the file

		int mapfileLines = int.Parse(mapTextLines[1].Split(',')[0]);
		int	mapfileColumns = int.Parse(mapTextLines[1].Split(',')[1]);

		string[,] mapfileToSpritesMatrix = new string[mapfileLines,mapfileColumns]; //creates a matrix of integers that is the size that the mapfile's second line determines

		for (int i=2; i < mapTextLines.Length; i++){ //some more complex code here to try and fill the mapfileToSprites matrix, by turning the data from the mapfile into something 
													//the game understands better; its also here that the mapfile's specific sintax comes into play; if the sintax ever changes, this stops working
			if (mapTextLines[i] != "")
			{
				int j=0,k=0;
				foreach (string block in mapTextLines[i].Split(',')) {
					string[] blockSplit = block.Split('@');
					for (k = j; k < (j + int.Parse(blockSplit[0])); k++)
					{
						mapfileToSpritesMatrix[k,i-2] = blockSplit[1];
					}
					j=k;
				}
			}
		}

		if (terrainSpritesArray != null) // checks to see if the terrain sprites are loaded, if that's ok, starts to fill the map
		{
			Sprite spriteFromMapfile;
			for(int i = 0; i < numberOfLines; i++)
			{
				for(int j = 0; j < numberOfColumns; j++)
				{
					int indexOfSprite = Array.FindIndex(terrainSpritesArray, s => s.name == mapfileToSpritesMatrix[i,j]); //does some LINQ to find the sprite based on name provided by the mapfile

					if (mapfileToSpritesMatrix[i,j] == "spawner") {
						GameObject newSpawner = Instantiate(spawnerPrefab,gridMatrix[i,j].transform.position,Quaternion.Euler(Vector3.zero)) as GameObject;
						gameController.enemySpawner.listOfEnemySpawners.Add(newSpawner);
						newSpawner.name = "EnemySpawner" + newSpawner.GetInstanceID();
					}
					if (mapfileToSpritesMatrix[i,j] == "playerSpawn") {
						GameObject newSpawner = Instantiate(spawnerPrefab,gridMatrix[i,j].transform.position,Quaternion.Euler(Vector3.zero)) as GameObject;
						gameController.playerSpawn = newSpawner;
						newSpawner.name = "PlayerSpawner";
					}

					if (indexOfSprite < 0)
						spriteFromMapfile = null;
					else
						spriteFromMapfile = terrainSpritesArray.ElementAt(indexOfSprite);

					foreach(SpriteRenderer spriteRenderer in gridMatrix[i,j].GetComponentsInChildren<SpriteRenderer>())
					{
						spriteRenderer.sprite = spriteFromMapfile;
					}
					foreach(Collider2D collider in gridMatrix[i,j].GetComponentsInChildren<Collider2D>())
					{
						if (spriteFromMapfile && spriteFromMapfile.name.Contains("Full"))
							collider.enabled = true;
						else
							collider.enabled = false;
					}
				}
			}
			//finished parsing succesfully, therefore tell game to spawn player
			gameController.SpawnPlayer();
		} else 	{
			throw new ArgumentNullException("Something happened to the terrain! Maybe it doesn't exist, or maybe it's mispelled in the mapfile!");
		}
	}

	void initiateGrid()
	{
		for (int j = 0; j > (-numberOfColumns); j--)
		{
			for (int i = 0; i < numberOfLines; i++)
			{
				Vector3 position = new Vector3((squareSizePixels/100)*i+scaleFactorForX,(squareSizePixels/100)*j+scaleFactorForY,this.transform.position.z);
				
				GameObject tile = (GameObject)Instantiate(prefabBlankQuad, position, Quaternion.identity);
				tile.transform.parent = transform;
				tile.name = "SpriteQuad" + (i+1) + "," + (-j+1);
				tile.transform.localScale = new Vector3(squareSizeInUnityScale,squareSizeInUnityScale,1.0f);
				
				gridMatrix[i,-j] = tile;
			}
		}
	}
	
	*/
}

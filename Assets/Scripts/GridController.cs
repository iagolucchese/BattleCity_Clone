using UnityEngine;
using System.Collections;
using System;
using System.Linq;

public class GridController : MonoBehaviour {

	public GameController gameController;
	public GameObject enemySpawnerPrefab;

	public int numberOfLines, numberOfColumns; //how big you want the map to be, x lines by y columns
	public float squareSizePixels; //meant to be how big in pixels you want each square sprite to be, no matter it's original size
	public GameObject prefabSpriteQuad, boundaries; //references: one to the prefab for each tile, the other to the boundaries of the game

	public TextAsset mapTextFile;

	private GameObject[,] gridMatrix; //matrix to hold all the grid elements, useful for map making later on
	private float scaleFactorForX, scaleFactorForY; //these are basically padding for the grid's squares, because grid positions should start from the top left corner, not the unity default, center
	private float squareSizeInUnityScale;

	void Start () 
	{
		gridMatrix = new GameObject[numberOfLines,numberOfColumns];

		squareSizeInUnityScale = prefabSpriteQuad.transform.localScale.x*squareSizePixels; //converts the pixel size to unity scale size

		scaleFactorForX = boundaries.transform.lossyScale.x/-2 + squareSizeInUnityScale/2;
		scaleFactorForY = boundaries.transform.lossyScale.y/2 - squareSizeInUnityScale/2;

		initiateGrid(); //creates each quad in the grid, and stores them into the gridMatrix
		parseMapFile(); //parses through the mapfile.txt, and sets the sprites for the quads
	}

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
						GameObject newSpawner = Instantiate(enemySpawnerPrefab,gridMatrix[i,j].transform.position,Quaternion.Euler(Vector3.zero)) as GameObject;
						gameController.enemySpawner.listOfEnemySpawners.Add(newSpawner);
						newSpawner.name = "EnemySpawner" + newSpawner.GetInstanceID();
					}
					if (mapfileToSpritesMatrix[i,j] == "playerSpawn") {
						GameObject newSpawner = Instantiate(enemySpawnerPrefab,gridMatrix[i,j].transform.position,Quaternion.Euler(Vector3.zero)) as GameObject;
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
			/* finished parsing succesfully, therefore tell game to spawn player */
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
				
				GameObject tile = (GameObject)Instantiate(prefabSpriteQuad, position, Quaternion.identity);
				tile.transform.parent = transform;
				tile.name = "SpriteQuad" + (i+1) + "," + (-j+1);
				tile.transform.localScale = new Vector3(squareSizeInUnityScale,squareSizeInUnityScale,1.0f);
				
				gridMatrix[i,-j] = tile;
			}
		}
	}
}

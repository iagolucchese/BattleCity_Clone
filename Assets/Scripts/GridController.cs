using UnityEngine;
using System.Collections;
using System;
using System.Linq;

public class GridController : MonoBehaviour {

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
						mapfileToSpritesMatrix[i-2,k] = blockSplit[1];
						//Debug.Log((i-2) + " " + k + "; " + mapfileToSpritesMatrix[i-2,j]);
					}
					j=k;
				}
			}
		}

		if (terrainSpritesArray != null) // checks to see if the terrain sprites are loaded
		{
			Sprite spriteFromMapfile;
			for(int i = 0; i < numberOfLines; i++)
			{
				for(int j = 0; j < numberOfColumns; j++)
				{
					int indexOfSprite = Array.FindIndex(terrainSpritesArray, s => s.name == mapfileToSpritesMatrix[i,j]); //does some LINQ to find the sprite based on name provided by the mapfile
					Debug.Log(i + " " + j + "; " + mapfileToSpritesMatrix[i,j]);

					if (indexOfSprite < 0)
						spriteFromMapfile = null;
					else
						spriteFromMapfile = terrainSpritesArray.ElementAt(indexOfSprite);
					//Debug.Log(spriteFromMapfile);

					foreach(SpriteRenderer spriteRenderer in gridMatrix[i,j].GetComponentsInChildren<SpriteRenderer>())
					{
						spriteRenderer.sprite = spriteFromMapfile;
					}
					foreach(Collider2D collider in gridMatrix[i,j].GetComponentsInChildren<Collider2D>())
					{
						if (spriteFromMapfile)
							collider.enabled = true;
						else
							collider.enabled = false;
					}
				}
			}
		} else 	{
			Debug.Log("Something happened to the terrain! Maybe it doesn't exist, maybe it's misspelled in the mapfile!");
			throw new ArgumentNullException();
		}
	}

	void initiateGrid()
	{
		for (int j = 0; j > (-numberOfColumns); j--)
		{
			for (int i = 0; i < numberOfLines; i++)
			{
				Vector3 position = new Vector3((squareSizePixels/100)*i+scaleFactorForX,(squareSizePixels/100)*j+scaleFactorForY,2.0f);
				
				GameObject tile = (GameObject)Instantiate(prefabSpriteQuad, position, Quaternion.identity);
				tile.transform.parent = transform;
				tile.name = "SpriteQuad" + (i+1) + "," + (-j+1);
				tile.transform.localScale = new Vector3(squareSizeInUnityScale,squareSizeInUnityScale,1.0f);
				
				gridMatrix[i,-j] = tile;
			}
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    // map stats panel

    public GenerationMaster generationMaster;                       
    public CameraControl camControl;

    public Slider sizeXSlider;
    public Slider sizeYSlider;

    public Slider noiseScaleSlider;

    public Slider xSeedSlider;
    public Slider ySeedSlider;


    public Slider chestSlider;
    public Slider biomeSlider;
    public Slider waterSlider;

    public Slider waterHeight;


    public TMP_InputField randomSeedInputField;


    public TextMeshProUGUI textMeshXsize;
    public TextMeshProUGUI textMeshYsize;
    public TextMeshProUGUI textMeshScale;
    public TextMeshProUGUI textMeshSeedX;
    public TextMeshProUGUI textMeshSeedY;

    public TextMeshProUGUI chestFrequency;
    public TextMeshProUGUI chestTotalTiles;

    public TextMeshProUGUI waterFrequency;
    public TextMeshProUGUI waterTotalTiles;

    public TextMeshProUGUI biomeTotalValue;
    public TextMeshProUGUI altnernativeBiomeTotalTiles;

    public TextMeshProUGUI waterHeightValueTmp;

    // asset stats panel

    public Slider assetSizeSlider;
    public Slider assetDensitySlider;

    public TextMeshProUGUI assetSizeText;
    public TextMeshProUGUI assetDensityText;



    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void UpdateUI()
    {
               // extract from UI sliders
            
               // map stats panel

        int xSize = (int)sizeXSlider.value;                                                      
        int ySize = (int)sizeYSlider.value;                                                     
        int noiseScale = (int)noiseScaleSlider.value;                                         
        int seedX = (int)xSeedSlider.value;                                                       
        int seedY = (int)ySeedSlider.value;                                                    
        int chestValue = (int)chestSlider.value;
        int biomeValue = (int)biomeSlider.value;
        int randomSeedValue = int.Parse(randomSeedInputField.text);    
        int waterValue = (int)waterSlider.value;
        int waterHeightValue = (int)waterHeight.value;

                // assets stats panel

        int assetSize = (int)assetSizeSlider.value;
        int assetDensity = (int)assetDensitySlider.value;

                // execute generation 
        generationMaster.Generate(xSize, ySize, noiseScale, seedX, seedY, chestValue, biomeValue, randomSeedValue, waterValue, waterHeightValue, assetSize, assetDensity);
        UIValues uiVals = generationMaster.RequestIUValues();
        camControl.CalculateCameraTarget(generationMaster.GetTilesList());


        // update UI 
        // map stats panel
        textMeshXsize.text = xSize.ToString();                                  
        textMeshYsize.text = ySize.ToString();
        textMeshScale.text = noiseScale.ToString();
        textMeshSeedX.text = seedX.ToString();
        textMeshSeedY.text = seedY.ToString();

        chestFrequency.text = chestValue.ToString();
        waterFrequency.text = waterValue.ToString();
        biomeTotalValue.text = biomeValue.ToString();
        waterHeightValueTmp.text = waterHeightValue.ToString();

        chestTotalTiles.text = uiVals.chest.ToString();
        waterTotalTiles.text = uiVals.water.ToString();
        altnernativeBiomeTotalTiles.text = uiVals.biome.ToString();

        // asset stats panel
        assetSizeText.text = assetSize.ToString();
        assetDensityText.text = assetDensity.ToString();




    }




}






//  Debug.Log(Time.time + " " + chestValue);
//   chestFrequency.text = chestValue.ToString();

//     chestFrequency.text = chestValue.ToString();
//    chestTotalTiles.text = generationMaster.GetChestList().Count.ToString();

//    waterFrequency.text = waterValue.ToString();
//    waterTotalTiles.text = generationMaster.GetWaterList().Count.ToString();

//     biomeTotalValue.text = biomeValue.ToString();
//   altnernativeBiomeTotalTiles.text = generationMaster.GetAlternativeBiomeList().Count.ToString();







//    textMeshScale.text = noiseScale.ToString();
//   textMeshSeedX.text = seedX.ToString();
//   textMeshSeedY.text = seedY.ToString();
//   biomeTotalValue.text = biomeValue.ToString();
//   waterTotalValue.text = waterValue.ToString();
//   waterHeightValueTMP.text = waterHeightValue.ToString();

//    randomSeedText.text = randomSeedInt.ToString();

/*
string chestText = chestValue + "%";
chestFrequency.text = chestText;


int totalTiles = (int)xSize * (int)ySize;
float totalChestTiles = totalTiles * chestValue /100;

float calculateChest = Mathf.Ceil(totalChestTiles);
chestTotalTiles.text = calculateChest.ToString();


noiseScale = noiseScale * 0.01f;

seedX = seedX * 0.1f;
seedY = seedY * 0.1f;

*/


//   generationMaster.Generate(xSize, ySize, noiseScale, seedX, seedY, chestValue, biomeValue, randomSeedValue, waterValue, waterHeightValue);
//  camControl.CalculateCameraTarget(generationMaster.GetTilesList());

//      int randomSeed = randomSeedText.text.;




//     generationMaster.mapWidth = (int)xSize;
//   generationMaster.mapHeight = (int)(ySize);
//   generationMaster.noiseScale = (int)(noiseScale);
// generationMaster.seedX = (int)seedX;
//  generationMaster.seedY = (int)seedY;
//generationMaster.chestPercentage = (int)chestValue;

//     generationMaster.Generate(xSize, ySize, noiseScale, seedX, seedY, chestValue, biomeValue, randomSeedValue, waterValue, waterHeightValue);
//    camControl.CalculateCameraTarget(generationMaster.GetTilesList());

//   camControl.TilesNumber(generationMaster.CalculateCameraTarget());



// generationMaster.GenerateMap();
// camControl.InitialiseNewState( everything thats needed);

//GenerateMap((int)xSize, (int)ySize, noiseScale, seedX, seedY, calculateChest);

//  generationMaster.RequestValues(this);

/*

Screen Grabber

poop

by Matt Cabanag

27 Aug 2017

based on below:
*/

/**
 * Robot Screenshots (v2.22)
 * by  Amnon (2014/Nov/11)
 * mod GoToLoop
 *
 * forum.processing.org/two/discussion/8025/
 * take-a-screen-shot-of-the-screen
 */
 
import java.awt.Rectangle;
import java.awt.Robot;
import java.awt.AWTException;

static int capWidth;
static int capHeight;
static int capX = 0;
static int capY = 0;

//detection params
float distThreshold = 30;
float colThreshold = 30;
int blobMin = 200;
int blobMax = 10000;
int brightCountCutoff = 100000;
color brightCutoff = color(100,100,100);

boolean blobChecking = false;

int blobCountMax = 120;

//diagnostics
int blobCount = 0;
int [] colAvg = new int[3];
int [] colSum = new int[3];
int brightCount = 0;


//"system things"
String imageFile = "hello";
String blobDataFile = "blobs";
 
PImage screenshot;
Rectangle dimension;
Robot robot;

ArrayList <Blob> blobs = new ArrayList<Blob>();

int fileIndex = 0;
int maxFileIndex = 20;
 
void setup() 
{  
  capWidth = 706;//displayWidth;
  capHeight = 506;//displayHeight;
  capX = 300;
  capY = 200;
  
  size(706, 506, JAVA2D);
 
  smooth(4);
  frameRate(25);
 
  imageMode(CORNER);
  background((color) random(#000000));
 
  screenshot = createImage(capWidth, capHeight, ARGB);
  dimension  = new Rectangle(capX, capY, capWidth, capHeight);
 
  try 
  {
    robot = new Robot();
  }
  catch (AWTException cause) 
  {
    println(cause);
    exit();
  }  
}
 
void draw() 
{
  image(grabScreenshot(screenshot, dimension, robot)
    , 0, 0, width, height);
  
  blobCount = 0;
  for (Blob b : blobs) 
  {
    if (b.size() > blobMin && b.size() < blobMax) 
    {
      b.show();
      blobCount++;
    }
  }
  
  WriteBlobFile(blobCount);
  
  fill(color(255,0,0));
  text("Color threshold: " + colThreshold,10,20);
  text("Distance threshold: " + distThreshold,10,40);
  text("Blob Count: " + blobCount, 10, 60);
  text("Blob Min: " + blobMin, 10, 80);
  text("Blob Max: " + blobMax, 10, 100);
  text("Blob Checking: " + blobChecking, 10, 120);  
  
  int avgSum = colAvg[0] + colAvg[1] + colAvg[2];
  int sumSum = colSum[0] + colSum[1] + colSum[2];
  
  text(colAvg[0] + ", " + colAvg[1] + ", " + colAvg[2] + ": " + avgSum, 10, 140);
  text(colSum[0] + ", " + colSum[1] + ", " + colSum[2] + ": " + sumSum, 10, 160);
  text("Bright Pixels: " + brightCount, 10, 180);
  
  screenshot.save(imageFile+"_"+fileIndex+"_.jpg");  
    //println("blob count: " + blobs.size());
  blobs.clear();
  
  if(fileIndex < maxFileIndex) 
    fileIndex++;
  else
    fileIndex = 0;
    
  if (fileIndex < 0)
    fileIndex = maxFileIndex;
}

void WriteBlobFile(int count)
{
  String [] validBlobs = new String[count];
  
  int i = 0;
  for (Blob b : blobs) 
  {
    if (b.size() > blobMin && b.size() < blobMax) 
    {
      validBlobs[i] = b.getString();
      i++;
    }
  }
  
  
  try
  {
    saveStrings(blobDataFile+"_"+fileIndex+"_.txt", validBlobs);
  }
  catch(Exception e)
  {
    println("File contention!! fileIndex:" + fileIndex );
    fileIndex--;
  }
}


PImage grabScreenshot(PImage img, Rectangle dim, Robot bot) 
{
  //return new PImage(bot.createScreenCapture(dim));
 
  bot.createScreenCapture(dim).getRGB(0, 0
    , dim.width, dim.height
    , img.pixels, 0, dim.width);
  
  img = flipImage(img);
  
  colSum[0] = 0;
  colSum[1] = 0;
  colSum[2] = 0;
  brightCount = 0;
  
  
  //stat gathering
  for (int x = 0; x < img.width && blobs.size() < blobCountMax; x++ ) 
  {
    for (int y = 0; y < img.height && blobs.size() < blobCountMax; y++) 
    {      
      int loc = x + y * img.width;      
      color currentColor = img.pixels[loc];
      
      float r1 = red(currentColor);
      float g1 = green(currentColor);
      float b1 = blue(currentColor);
      
      colSum[0] += r1;
      colSum[1] += g1;
      colSum[2] += b1;
      
      if(currentColor >= brightCutoff)
        brightCount++;
    }
  }
  
  colAvg[0] = colSum[0] / (img.width * img.height);
  colAvg[1] = colSum[1] / (img.width * img.height);
  colAvg[2] = colSum[2] / (img.width * img.height);
  
  //blob detection
  if(blobChecking)
  {
    for (int x = 0; x < img.width && blobs.size() < blobCountMax; x++ ) 
    {
      for (int y = 0; y < img.height && blobs.size() < blobCountMax; y++) 
      {
        
        int loc = x + y * img.width;      
        
        color currentColor = img.pixels[loc];
        
        //if(y > 100)
        //img.pixels[loc] = color(0,255,0);
        
        float r1 = red(currentColor);
        float g1 = green(currentColor);
        float b1 = blue(currentColor);
        
        colSum[0] += r1;
        colSum[1] += g1;
        colSum[2] += b1;
        
        boolean colourAccept = false;
        
        //skin tone
        if(!colourAccept)
        { 
          colourAccept = r1 >= 140 && g1 >=99 && b1 >= 110;  
        }
        
        //"bright"
        if(!colourAccept)
        { 
          colourAccept = r1 >= 190 && g1 >= 190 && b1 >= 190;  
        }
        
        //dark clothing
        if(!colourAccept)
        {
          if(brightCount < brightCountCutoff)
            colourAccept = r1 <= 60 && g1 <= 60 && b1 <= 60;
        }

        if(colourAccept)
        {
          //println("ACCEPTED :- r1: " + r1 + " g1: " + g1 + " b1: " + b1);
          
          boolean found = false;

          for(Blob b : blobs)
          {
            if(b.isNear(x,y))
            {
              b.add(x,y);
              found = true;
              break;
            }
          }
          
          if(!found && blobs.size() < blobCountMax)
          {
            //println("BLOB FOUND");
            Blob b = new BlobFast(x,y);
            blobs.add(b);
          }

        }        
      }
    }
  }
  
  img.updatePixels();
  return img;
}

void keyPressed() 
{
  if (key == 'a') 
  {
    distThreshold+=5;
  } 
  else if (key == 'z') 
  {
    distThreshold-=5;
  }
  
  if (key == 's') 
  {
    colThreshold+=5;
  } 
  else if (key == 'x') 
  {
    colThreshold-=5;
  }
  
  if (key == '1') 
  {
    blobMin+=5;
  } 
  else if (key == 'q') 
  {
    blobMin-=5;
  }
  
  if (key == '2') 
  {
    blobMax+=5;
  } 
  else if (key == 'w') 
  {
    blobMax-=5;
  }
  
  
  if(key == 'b')
  {
    blobChecking = !blobChecking;
  }
}

PImage flipImage(PImage src)
{ 
  for (int x = 0; x < src.width/2; x++ ) 
  {
    for (int y = 0; y < src.height - 1; y++) 
    {
      int loc = x + (y * src.width);
      int op = (src.width-x)+ (y * src.width);
      
      color tmp = src.pixels[loc];
      src.pixels[loc] = src.pixels[op];
      src.pixels[op] = tmp;
    }
  }
  
  src.updatePixels();
  
  return src;
}



// Daniel Shiffman
// http://codingtra.in
// http://patreon.com/codingtrain
// Code for: https://youtu.be/1scFcY-xMrI

// Custom distance functions w/ no square root for optimization
float distSq(float x1, float y1, float x2, float y2) 
{
  float d = (x2-x1)*(x2-x1) + (y2-y1)*(y2-y1);
  return d;
}


float distSq(float x1, float y1, float z1, float x2, float y2, float z2) 
{
  float d = (x2-x1)*(x2-x1) + (y2-y1)*(y2-y1) +(z2-z1)*(z2-z1);
  return d;
}
 
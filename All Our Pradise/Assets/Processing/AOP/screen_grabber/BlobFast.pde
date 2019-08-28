//Matt Cabanag
//
//adapted from:
// Daniel Shiffman
// http://codingtra.in
// http://patreon.com/codingtrain
// Code for: https://youtu.be/1scFcY-xMrI

class BlobFast extends Blob 
{ 
  float minx;
  float miny;
  float maxx;
  float maxy;

  BlobFast(int x, int y) 
  {
    super();
    
    minx = x;
    miny = y;
    maxx = x;
    maxy = y;

    centreX = x;
    centreY = y;
  }

  void show() 
  {
    stroke(0);
    fill(255);
    strokeWeight(2);
    rectMode(CORNERS);
    rect(minx, miny, maxx, maxy);

  }


  void add(float x, float y) 
  {
    if(isNear(x,y))
    {
      if(x > maxx)
        maxx = x;
        
      if(x < minx)
        minx = x;
        
      if(y > maxy)
        maxy = y;
        
      if(y < miny)
        miny = y;
        
      centreX = (int)((float)(maxx+minx)/2);
      centreY = (int)((float)(maxy+miny)/2);
    }
  }

  float size() 
  {
    return (maxx-minx)*(maxy-miny);
  }
  
  boolean isNear(float x, float y) 
  {
    centreX = (int)((float)(maxx+minx)/2);
    centreY = (int)((float)(maxy+miny)/2);
    
    float d = distSq(centreX,centreY,x,y);
    
    return d < distThreshold * distThreshold;
  }
  
  public String getString()
  {
    return centreX + ", " + centreY;  
  }
}
## Overview
This package is provides functionality for a [CAVE (Cave automatic virtual environment)](https://en.wikipedia.org/wiki/Cave_automatic_virtual_environment) space with appropriate camera projection and interaction abilities. This project is specifically configured for the MMU Brooks CAVE (BR0.89), a 5m by 5m floorspace with ~2.8125m high walls, running on three walls and the floor. 


<br/>
<p align="center">
  <img width="460" height="300" src="https://upload.wikimedia.org/wikipedia/commons/6/6d/CAVE_Crayoland.jpg"><br/>
  Image of a CAVE space. Credit to Wiki user Davepape.
</p>
<br/>



### The Problem - Warped Camera Projection
Camera projection within a CAVE space has unique challenges due to image wapring effects where content is stretched at wall edges. This is due to how virtual cameras typically render 3D scnenes in computer graphics. The image below is a [View Frustum](https://en.wikipedia.org/wiki/Viewing_frustum) and in simplified terms, everything behind the yellow 'Near' plane is what gets displayed to screen. 

<br/>
<p align="center">
  <img width="460" height="300" src="https://upload.wikimedia.org/wikipedia/commons/0/02/ViewFrustum.svg"><br/>
  Image of a View Frustrum. Credit to Wiki user MithrandirMage.
</p>
<br/>


For this context, the above yellow near plane can be understood as a wall within a CAVE facility where digital content will be projected onto. Given that, some considerations arrise such as:

1. How will we project to multiple walls, simply more cameras?
2. How should digital content infront of the near plane, that inside the physical bounds of the CAVE, be handled?
3. Where should the camera point, or 'eye' be position


<br/>


### Solution - Off-Axis Projection
My solution is to use [Off-axis projection in Unity](https://github.com/aptas/off-axis-projection-unity) as a base. With this technique, a camera's near plane and camera point of origin (or eye) can be unaligned. 

To ensure that only content outside the CAVE is rendered, each real surface has a camera with a corresponding near render plane positioned to replicate the real-world CAVE space.

To ensure the images displayed via each camera align as expected, the origin for each camera is the centre of the CAVE but set to head hight. This also is the physical place within the CAVE to view content where projection will be most accurate. 


<br/>


## How to use

1. Download this repository, extract the contents, and within Unity's Package Manager add "install package from disk".
2. Include the "CAVE_3W1F" prefab in your scene or a variation for you specific number of walls desired.
3. Include any desired intractability such as for touch, Canvas UI interaction or movement via the "CaveController" component on the prefab.


<br/>


## Features
- [x] Correctly projecting scenes on all walls within a build
- [x] Wall-agnostic Raycast
- [x] Wall-agnostic world space UI interacting 
- [x] Screen space demo UI (main menu)
- [x] Movement: Teleportation hotspots
- [x] Movement: Continuous
- [x] 360 degree video player (correctly aligned)


<br/>


## Roadmap 
- [ ] Polish up MVP (Main menu, Good example, Bad examples, 360 video player, Flashy interactive scene)
- [ ] Documentation
- [ ] Kinect Integration (with head-tracked perspective-based projection)
- [ ] AI Avatar Integration

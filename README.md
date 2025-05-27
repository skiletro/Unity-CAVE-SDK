## Overview
This package provides easy to use functionality within [Unity](https://unity.com/) for customaisable [CAVE (Cave automatic virtual environment)](https://en.wikipedia.org/wiki/Cave_automatic_virtual_environment) space configurations, with appropriate camera projection, interaction abilities and extension features such as incorporating Microsoft Kinect.

This project is specifically configured for the [Manchester Metropolitan University](https://www.mmu.ac.uk/) Brooks Building CAVE (BR0.89), a 5m by 5m floorspace with ~2.8125m high walls, running on three walls and the floor. 


<br/>
<p align="center">
  <img width="460" height="300" src="https://upload.wikimedia.org/wikipedia/commons/6/6d/CAVE_Crayoland.jpg"><br/>
  Image of a CAVE space. Credit to Wiki user Davepape.
</p>
<br/>
<br/>


## Features
<br/>

### Projection
- [x] Correctly projecting scenes on all walls within a build
<br/>

### Interaction
- [x] Wall-agnostic Raycast
- [x] Wall-agnostic world space UI interacting 
- [x] Movement: Teleportation hotspots
- [x] Movement: Continuous
- [ ] Touchables
<br/>

### Demonstrations
- [ ] Lightweight outdoor nature scene
- [ ] Lightweight indoor scene 
- [x] Screen space demo UI (main menu)
<br/>

### Kinect Functionality
- [ ] Kinect Integration (with head-tracked perspective-based projection)
<br/>

### Video Player
- [x] 360 degree video player (correctly aligned)
<br/>

### SDK Package
- [ ] Unity Package
- [ ] Documentation
- [ ] Polish
<br/>

<br/>
<br/>



## The CAVE Challenge
Camera projection within a CAVE space has unique challenges due to **image wapring effects** where content is stretched at wall edges. This is due to how virtual cameras typically render 3D scnenes in computer graphics. The image below is a [View Frustum](https://en.wikipedia.org/wiki/Viewing_frustum) and in simplified terms, everything behind the yellow 'Near' plane (from the cameras perspective) is what gets displayed to screen. 

<br/>
<p align="center">
  <img width="460" height="300" src="https://upload.wikimedia.org/wikipedia/commons/0/02/ViewFrustum.svg"><br/>
  Image of a View Frustrum. Credit to Wiki user MithrandirMage.
</p>
<br/>


For this context, the above yellow near plane can be understood as a wall within a CAVE facility where digital content will be projected onto. Given that, some considerations arise such as:

1. How will we project to multiple walls, simply more cameras?
2. Where do we position near planes and decide what will and wont be rendered?
3. Where should the camera point, or 'eye' be positioned


### Our Solution
Our solution is to use [Off-axis projection in Unity](https://github.com/aptas/off-axis-projection-unity) as a base. With this technique, a camera's near plane and camera point of origin (or eye) can be unaligned. 

To ensure that only content outside the CAVE is rendered, each real surface (e.g. wall) has a camera with a corresponding near render plane positioned to replicate the real-world CAVE space.

To best ensure the images displayed on each wall do so as expected, the origin for each camera is set to the centre of the CAVE but raised to average head hight. In other words, viewing a 3D scene from the centre of the CAVE should give a user the most accurate perspective.

<br/>
<br/>



## How to use

1. Download this repository, extract the contents, and within Unity's Package Manager add "install package from disk".
2. Include the "CAVE_3W1F" prefab in your scene or create a variation for your specific number of walls desired.

- For intractability, see the CAVEController.cs component on the "CAVE_3W1F" prefab.

- Camera projection is handled through the default Unity camera and supports both the Universal and High Definition Render Pipelines (UPR and HDRP), and also any other camera screen effects.

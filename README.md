## Overview
This repository is a basic Unity template for a three wall and one floor [CAVE (Cave automatic virtual environment)](https://en.wikipedia.org/wiki/Cave_automatic_virtual_environment) system with appropriate camera projection. This project is specifically configured for the Brooks CAVE (BR0.89), a 5m by 5m floorspace, with ~2.8125m high walls.

### Purpose
Well why cant we simply use normal cameras? To understand the challenges with display within a CAVE system, Computer Graphics cameras and the [View Frustum](https://en.wikipedia.org/wiki/Viewing_frustum) should first be understood.

If you use a typical camera, you will encounter two main questions.
1. How should digital content inside the physical bounds of the CAVE be handled?
2. How can the digital content outside of the bounds properly link up between the surfaces and without warping?

### Solution
My solution is to use [Off-axis projection in Unity](https://github.com/aptas/off-axis-projection-unity) as a base. With this technique, a camera's near plane and camera point of origin (or eye) can be unaligned. 

To ensure that only content outside the CAVE is rendered, each real surface has a camera with a corresponding near render plane positioned to replicate the real-world CAVE space.

To ensure the images displayed via each camera align as expected, the origin for each camera is the centre of the CAVE but set to head hight. This also is the physical place within the CAVE to view content where projection will be most accurate. 




## How to use

1. Include the appropriate prefab for your desired setup selection of walls and the floor into your desired scene.
2. Include any desired intractability such as for touch, Canvas UI interaction or movement.




## Features
- [x] Correctly projecting scenes on all walls within a build
- [x] Wall-agnostic Raycast
- [x] Wall-agnostic world space UI interacting 
- [x] Screen space demo UI (main menu)
- [x] Movement: Teleportation hotspots
- [x] Movement: Continuous
- [x] 360 degree video player (correctly aligned)

## Roadmap 
- [ ] Polish up MVP (Main menu, Good example, Bad examples, 360 video player, Flashy interactive scene)
- [ ] Documentation
- [ ] Kinect Integration (with head-tracked perspective-based projection)
- [ ] AI Avatar Integration

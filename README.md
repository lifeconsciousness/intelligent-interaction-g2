
# Run Socket Client and Eye Tracker

If you are opening the project in unity please use version 6000.0.33f1. Othwerwise, the project may not work.

## Setup Instructions

#### 1. Clone the Repository
First, clone the repository to your local machine:

```bash
git clone https://github.com/lifeconsciousness/intelligent-interaction-g2.git
```

#### 2. Install Dependencies

Navigate to the projects python directory and install the required dependencies:

```bash
cd intelligent-interaction-g2/python
python -m .venv venv
```

Activate the virtual environment:

On Linux:
```bash
source venv/bin/activate
```

On Windows:
```bash
./venv/Scripts/activate
```

#### 3. Download the game

Either:

- Download the game from the [release page](https://github.com/lifeconsciousness/intelligent-interaction-g2/releases/latest) for your operating system.
- Build the game from the unity project in the unity folder. See the unity build instructions below.

## Usage

You can run the application using python and specify the desired tracking method and mode via command-line arguments.

#### 1. Track Eyes in an Image
To process an image with Haarcascade or Mediapipe, use the following commands:

**Haarcascade:**

```bash
python3 app.py --haarcascade
```

**Mediapipe:**

```bash
python3 app.py --mediapipe
```

#### 2. Enable Video Tracking
If you want to use real-time video tracking (e.g., via webcam), add the --video flag. By default, this will display the tracking results in a live video window.

**Haarcascade with Videos:**

```bash
python3 app.py --haarcascade --video
```

**Mediapipe with Video:**

```bash
python3 app.py --mediapipe --video
```

#### 3. Run the Game

To run the game, navigate to the game directory and run the executable, if the face tracker is not running, the game will default to keyboard controls.

If the face tracking is running on game start the game will default to face tracking controls.

## Docker Instructions

Note that the docker image may not work on all systems, as it requires access to the camera. So try the non docker instructions if you encounter any issues.

#### 1. Build the Docker Image
Run the following command to build the Docker image:

```bash
docker build -t eye-tracking-app .
```

#### 2. Run the Docker Container

```bash
docker run --rm \
 --net=host \
 --privileged \
 eye-tracking-app
```

## Unity build instructions

#### 1. Download unity hub

Download unity hub from the following link: https://unity.com/download

#### 2. Install unity editor

Install unity editor version 6000.0.33f1 in the unity hub. Under the installs tab, click on the install editor button and select the unity editor version 6000.0.33f1. There may be a new version of the unity editor available by now in theory any 6000.0.x version should work. If not to get version 6000.0.33f1 you may need to click on the Archive tab in the Install Editor prompt and then click on the "download archive" link. A website will open where you can install version 6000.0.33f1.

#### 3. Add project to unity

In the projects tab click on the "Add" button and click on the "Add project from disk" button. Select the "IC game" folder inside of this repository.

#### 4. Start the game

Click on the project "IC game" to open it. On the bottom window you should see folders go into the "Scenes" folder and double click on the "Menu" scene not the folder to open it. Then click on the play button on the top of screen to run the game.


# The game

## Main menu

Has interactive scene start button and calibration tool button

![image](https://ibb.co/FYMdcFf)

## The game itself

The objective is to dodge projectiles that fly into the player. After certain time point is reached the boss appears.

![image](https://ibb.co/bHt23DJ)



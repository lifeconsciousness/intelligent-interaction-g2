
# Run Socket Client and Eye Tracker

Please use unity version 6000.0.33f1 for the unity project. Othwerwise, the project may not work.

## Setup Instructions

#### 1. Clone the Repository
First, clone the repository to your local machine:

```bash
git clone https://github.com/lifeconsciousness/intelligent-interaction-g2.git
cd intelligent-interaction-g2/python
```

#### 2. Build the Docker Image
Run the following command to build the Docker image:

```bash
docker build -t eye-tracking-app .
```

## Usage

You can run the application using Docker and specify the desired tracking method and mode via command-line arguments.

#### 1. Track Eyes in an Image
To process an image with Haarcascade or Mediapipe, use the following commands:

**Haarcascade:**

```bash
python3 app.py --haarcascade
```

**Mediapipe:**

```bash
docker run --rm \
 --net=host \
 --privileged \
 eye-tracking-app
```

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
FROM python:3.12.3-slim

WORKDIR /app

COPY . /app

RUN pip install -r requirements.txt

RUN apt-get update && apt-get install -y \
    libgl1 \
    libglib2.0-0

RUN apt-get clean && rm -rf /var/lib/apt/lists/*

CMD ["python", "app.py", "--mediapipe"]

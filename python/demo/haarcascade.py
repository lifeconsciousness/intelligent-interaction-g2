import cv2
import os

base_dir = os.path.dirname(__file__)

eye_cascade_path = os.path.join(base_dir, "haarcascade/haarcascade_eye.xml")
face_cascade_path = os.path.join(base_dir, "haarcascade/haarcascade_frontalface_default.xml")

eye_cascade = cv2.CascadeClassifier(eye_cascade_path)
face_cascade = cv2.CascadeClassifier(face_cascade_path)

cap = cv2.VideoCapture(0)

while 1:
    ret, img = cap.read()
    gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
    faces = face_cascade.detectMultiScale(gray, 1.3, 5)

    for (x,y,w,h) in faces:
        cv2.rectangle(img,(x,y),(x+w,y+h),(255,0,0),2)
        roi_gray = gray[y:y+h, x:x+w]
        roi_color = img[y:y+h, x:x+w]
        
        eyes = eye_cascade.detectMultiScale(roi_gray, scaleFactor=1.1, minNeighbors=50, minSize=(90, 90))
        eyes = sorted(eyes, key=lambda e: e[0]) 
        right_eye = eyes[0] if len(eyes) > 0 else None

        if right_eye is not None:
            (ex, ey, ew, eh) = right_eye

            cv2.rectangle(roi_color, (ex, ey), (ex + ew, ey + eh), (0, 255, 0), 2)

    cv2.imshow('img',img)
    k = cv2.waitKey(30) & 0xff
    if k == 27:
        break

cap.release()
cv2.destroyAllWindows()
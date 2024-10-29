from abc import ABC, abstractmethod
import threading

class EyeTrackerInterface(ABC):

    def __init__(self):
        self.x_position = 0
        self.y_position = 0
        self.distance = 0
        self.stop_event = threading.Event() 
        
    @abstractmethod
    def eye_tracking(self):
        pass

    def stop(self):
        self.stop_event.set()

    def getPosition(self):
        return self.x_position, self.y_position, self.distance
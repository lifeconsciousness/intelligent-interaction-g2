from abc import ABC, abstractmethod
from dataclasses import dataclass
from typing import Tuple

@dataclass
class EyeTrackingResult:
    x: int
    y: int
    distance: float
    eye_rect: Tuple[int, int, int, int]

class EyeTrackingConfig:
    KNOWN_DISTANCE = 76.2  
    KNOWN_FACE_WIDTH = 3.0  
    FOCAL_LENGTH = 381.0  

class EyeTrackerInterface(ABC):
    def __init__(self):
        self.x_position = 0
        self.y_position = 0
        self.distance = 0
        self.eye_rect = None
        
    @abstractmethod
    def eye_tracking(self, image) -> EyeTrackingResult:
        pass

    def get_position(self):
        return self.x_position, self.y_position, self.distance
    
    def calculate_distance(self, face_width_in_frame):
        if face_width_in_frame == 0:
            return float('inf')
        return (EyeTrackingConfig.KNOWN_FACE_WIDTH * EyeTrackingConfig.FOCAL_LENGTH) / face_width_in_frame
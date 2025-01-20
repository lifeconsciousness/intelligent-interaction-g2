from matplotlib import pyplot as plt
import numpy as np
from dataclasses import dataclass
from typing import List
import random 

@dataclass
class Metrics:
    latencies: List[float]
    frame_count: int
    miss_rate: float
    fps: float
    
    def __post_init__(self):
        self.frames = list(range(len(self.latencies)))
        
def print_metrics(metrics : Metrics):
    print(f"Total Frames: {metrics.frame_count}")
    print(f"Miss Rate: {metrics.miss_rate:.2f}%")
    print(f"FPS: {metrics.fps:.2f}")
    print(f"Average Latency: {np.mean(metrics.latencies[1::]) * 1000:.2f}")
    print(f"Max Latency: {np.max(metrics.latencies[1::]) * 1000:.2f}")
    print(f"Min Latency: {np.min(metrics.latencies[1::]) * 1000:.2f}")
    print(f"Standard Deviation: {np.std(metrics.latencies[1::]) * 1000:.2f}")

def display_metrics(metrics : Metrics):

    print_metrics(metrics)
    
    plt.figure(figsize=(10, 6))
    plt.plot(metrics.frames[1::], np.array(metrics.latencies[1::]) * 1000, label='Extraction Time (ms)')
    plt.xlabel('# Frame')
    plt.ylabel('Extraction Time (ms)')
    plt.title('Extraction Time')
    plt.legend()
    plt.grid()
    random_num = random.randint(0, 10)
    plt.savefig(f'latencies{random_num}.png')
    
    summary_metrics = ['Miss Rate (%)', 'FPS']
    summary_values = [metrics.miss_rate, metrics.fps]

    plt.figure(figsize=(6, 6))
    plt.bar(summary_metrics, summary_values, color=['red', 'blue'])
    plt.title('Summary Metrics')
    plt.ylabel('Value')
    for i, value in enumerate(summary_values):
        plt.text(i, value + 0.5, f"{value:.2f}", ha='center')
    plt.ylim(0, max(summary_values) + 10)

    plt.savefig(f'metrics{random_num}.png')
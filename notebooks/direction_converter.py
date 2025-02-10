import yaml
from enum import Enum

class Direction(Enum):
    NORTH = 'n'
    EAST = 'e'
    SOUTH = 's'
    WEST = 'w'

def direction_representer(dumper, data):
    return dumper.represent_scalar('!direction', data.value)

def direction_constructor(loader, node):
    value = loader.construct_scalar(node)
    return Direction(value.lower())

# Register the custom converter
yaml.add_representer(Direction, direction_representer)
yaml.add_constructor('!direction', direction_constructor) 
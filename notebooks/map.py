import json
import graphviz

def create_map():
    # Read the room definitions
    with open('../src/Game/data/room-defs.json', 'r') as f:
        rooms = json.load(f)

    # Create a new directed graph
    dot = graphviz.Digraph(comment='Room Map')
    dot.attr(rankdir='TB')  # Top to Bottom for North/South

    # Add all rooms as nodes
    for room in rooms:
        dot.node(room['id'], room['name'])

    # Add all exits as edges with constraints based on direction
    for room in rooms:
        for exit in room['exits']:
            # Set constraint and weight based on direction
            if exit['direction'] in ['n', 's']:
                dot.edge(room['id'], exit['destinationId'], exit['direction'], constraint='true')
            elif exit['direction'] == 'e':
                dot.edge(room['id'], exit['destinationId'], exit['direction'], constraint='false', weight='1')
            elif exit['direction'] == 'w':
                dot.edge(room['id'], exit['destinationId'], exit['direction'], constraint='false', weight='8')
            else:
                raise ValueError(f"Invalid direction: {exit['direction']}")

    return dot
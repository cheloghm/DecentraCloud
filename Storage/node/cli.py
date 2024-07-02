import click
import requests
import json
import os

BASE_URL = "http://localhost:5000/api"  # Update this with the actual URL of the central server

@click.group()
def cli():
    pass

@click.command()
@click.option('--username', prompt='Your username', help='The username to register the node.')
@click.option('--password', prompt='Your password', hide_input=True, confirmation_prompt=True, help='The password to register the node.')
@click.option('--storage', prompt='Storage space (in GB)', help='The amount of storage space to allocate.')
def register(username, password, storage):
    """Register the storage node with the central server."""
    data = {
        'username': username,
        'password': password,
        'storage': storage
    }

    response = requests.post(f"{BASE_URL}/nodes/register", json=data)

    if response.status_code == 200:
        click.echo("Node registered successfully!")
        node_data = response.json()
        with open('node_config.json', 'w') as f:
            json.dump(node_data, f)
    else:
        click.echo("Failed to register node. Please try again.")

cli.add_command(register)

if __name__ == '__main__':
    cli()

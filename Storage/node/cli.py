import click
import requests
import json
import os
from dotenv import load_dotenv

# Load environment variables from .env file
load_dotenv()

BASE_URL = os.getenv('BASE_URL')  # Use BASE_URL from the environment variable

@click.group()
def cli():
    pass

@cli.command()
@click.option('--email', prompt='Your email', help='The email to register the node.')
@click.option('--password', prompt='Your password', hide_input=True, confirmation_prompt=True, help='The password to register the node.')
@click.option('--storage', prompt='Storage space (in GB)', help='The amount of storage space to allocate.')
@click.option('--endpoint', prompt='Node endpoint URL', default=os.getenv('NODE_ENDPOINT'), help='The public endpoint URL of this node.')
@click.option('--nodename', prompt='Node name', help='The name of the node.')
def register(email, password, storage, endpoint, nodename):
    """Register the storage node with the central server."""
    if BASE_URL is None:
        click.echo("BASE_URL environment variable is not set.")
        return

    # Register node
    node_data = {
        'email': email,
        'password': password,
        'storage': int(storage),
        'endpoint': endpoint,
        'nodeName': nodename
    }
    headers = {
        'Content-Type': 'application/json'
    }
    try:
        register_response = requests.post(f"{BASE_URL}/Nodes/register", json=node_data, headers=headers, verify=False)

        if register_response.status_code == 200:
            click.echo("Node registered successfully!")
            node_config = register_response.json()
            with open('node_config.json', 'w') as f:
                json.dump(node_config, f)
        else:
            click.echo(f"Failed to register node. Status code: {register_response.status_code}. Response: {register_response.text}")
    except Exception as e:
        click.echo(f"An error occurred: {str(e)}")

if __name__ == '__main__':
    cli()

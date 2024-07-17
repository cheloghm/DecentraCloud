import click
import requests
import json
import os
from dotenv import load_dotenv
from subprocess import Popen, PIPE, TimeoutExpired

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
@click.option('--nodename', prompt='Node name', help='The name of the node.')
def register(email, password, storage, nodename):
    """Register the storage node with the central server."""
    if BASE_URL is None:
        click.echo("BASE_URL environment variable is not set.")
        return

    # Register node
    node_data = {
        'email': email,
        'password': password,
        'storage': int(storage),
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

@cli.command()
@click.option('--nodename', prompt='Node name', help='The name of the node.')
@click.option('--email', prompt='Your email', help='The email associated with the node.')
@click.option('--password', prompt='Your password', hide_input=True, help='The password to authenticate the node.')
@click.option('--port', prompt='Port number', default=3000, help='The port number where the node server is running.')
def login(nodename, email, password, port):
    """Authenticate the storage node with the central server."""
    if BASE_URL is None:
        click.echo("BASE_URL environment variable is not set.")
        return

    endpoint = f"https://localhost:{port}"
    click.echo(f"Node endpoint: {endpoint}")

    # Authenticate node
    login_data = {
        'nodeName': nodename,
        'email': email,
        'password': password,
        'endpoint': endpoint
    }
    headers = {
        'Content-Type': 'application/json'
    }
    click.echo(f"Sending login data: {json.dumps(login_data)}")  # Debugging line
    try:
        login_response = requests.post(f"{BASE_URL}/Nodes/login", json=login_data, headers=headers, verify=False)

        if login_response.status_code == 200:
            click.echo("Node authenticated successfully!")
            node_config = login_response.json()
            with open('node_config.json', 'w') as f:
                json.dump(node_config, f)
        else:
            click.echo(f"Failed to authenticate node. Status code: {login_response.status_code}. Response: {login_response.text}")
    except Exception as e:
        click.echo(f"An error occurred during authentication: {str(e)}")

if __name__ == '__main__':
    cli()

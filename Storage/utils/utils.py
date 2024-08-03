import shutil

def check_storage(min_required_gb):
    total, used, free = shutil.disk_usage("/")
    free_gb = free // (2**30)
    if free_gb < min_required_gb:
        raise Exception(f"Not enough free storage space. At least {min_required_gb}GB required, but only {free_gb}GB available.")
    return free_gb

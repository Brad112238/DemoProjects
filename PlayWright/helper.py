import time, json, subprocess

def connect():
    commond = 'rasdial.exe Hinet'
    subprocess.call(commond,shell=True)

def disconnect():
    commond = 'rasdial.exe Hinet /DISCONNECT'
    subprocess.call(commond,shell=True)

def reconnect():
    disconnect()
    time.sleep(3)
    connect()
    time.sleep(0.5)

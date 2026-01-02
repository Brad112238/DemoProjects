import requests,time,json,subprocess
from bs4 import BeautifulSoup
s = requests.session()
headers = {
    'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.0.0 Safari/537.36',
    "Content-Type" : "application/json;charset=UTF-8"
    }


def connect():
    commond = 'rasdial.exe Hinet'
    subprocess.call(commond,shell=True)

def disconnect():
    commond = 'rasdial.exe Hinet /DISCONNECT'
    subprocess.call(commond,shell=True)

def reconnect():
    disconnect()
    time.sleep(5)
    connect()
    time.sleep(0.5)


def get_TotalPage(city):
    url = f"https://buy.housefun.com.tw/region/{city}_c/?pg=1"
    r = requests.get(url, headers=headers)
    soup = BeautifulSoup(r.text, 'html.parser')
    total_count = soup.find("li", class_="list is-single is-curr").a.i.text
    total_count = total_count.replace("(","")
    total_count = total_count.replace(")","")
    total_count = total_count.replace(",","")
    count = int(total_count)/30 +1
    return int(count)
    


def get_data(city,page):
    url = f"https://buy.housefun.com.tw/region/{city}_c/?pg={page}"
    r = requests.get(url, headers=headers)
    soup = BeautifulSoup(r.text, 'html.parser')
    data = soup.find_all("a" ,class_="m-list-figure")
    return data


def get_house_detail(Oid):
    url = f"https://buy.housefun.com.tw/api/house?id={Oid}"
    r = requests.get(url,headers=headers)
    return r.json()
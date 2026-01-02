from datetime import datetime
from bs4 import BeautifulSoup
from organize_data_SQL import organize
import requests,time,json , schedule,traceback
from city import city_list
from city import hot_city
import helper
headers = {'user-agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/126.0.0.0 Safari/537.36'}
s = requests.session()

def crawler(city):
    helper.reconnect()
    page = 1
    save_data_count = 0
    reconnect_count = 0
    total_page = helper.get_TotalPage(city)
    print(f"{city}共有{total_page}頁") 
    a = organize(city)
    while page <= total_page:
        try:
            data = helper.get_data(city,page)
            for i in data :
                Oid = i['href'].split("/")[-1]
                detail = helper.get_house_detail(Oid)
                save_data_count += a.organize_data(detail,Oid)
                print("==================================================================================")
            page +=1
            reconnect_count+=1
            if reconnect_count ==5:
                helper.reconnect()
                reconnect_count = 0
        except Exception as e :
            print(e.__traceback__.tb_lineno)
            print(e)
            helper.reconnect()
    return save_data_count

def start():
    start_time =datetime.now()
    for city in city_list :
        save_data_count = crawler(city)
        print(f"{city}已存入{save_data_count}筆")
    ex_time = datetime.now() - start_time
    print(f"花費時間 ： {ex_time}")

def start2():
    start_time =datetime.now()
    for city in hot_city :
        save_data_count = crawler(city)
        print(f"{city}已存入{save_data_count}筆")
    ex_time = datetime.now() - start_time
    print(f"花費時間 ： {ex_time}")

while True:
    try:
        start()
    except Exception as e:
        print("發生錯誤 :" , e)
        traceback.print_exc()
    finally:
        time.sleep(1800)
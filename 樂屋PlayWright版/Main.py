from datetime import datetime
import helper
from rakuya_crawler import RakuyaCrawler
from organize_data_SQL import organize
from share import CITY
import time

def crawler(zipCode, area, county): 
    helper.reconnect()
    crawlerRakuya = RakuyaCrawler(True)
    page, count, save, reconnect_count= 1, 0, 0, 0 

    while True:
        total = crawlerRakuya.get_total(zipCode)
        if total > 0 :
            print(f"已找到{total}頁")
            break
        elif total == 0:
            helper.reconnect()
    a = organize(county)
    print(f"已開始爬取{county}{area}")

    while page <= total :
        try:
            items = crawlerRakuya.fetch_page(zipCode, page)
            print(f"第{page}頁，共{len(items)}筆")
            for i in items:
                type_ = (i.find("div", class_="info__detail-info").ul.find_all("li")[0].text.strip())

                oid = get_OID( i.a["href"])
                map = crawlerRakuya.get_map(oid)
                soup = crawlerRakuya.fetch_detail_soup(i.a["href"])

                result = a.organize_data(i.a["href"], county, area, type_, oid, map, soup)
                if result == 1:
                    save += 1 
                print("================")
                # time.sleep(5000)
            page +=1
            reconnect_count +=1
            if reconnect_count == 5:
                crawlerRakuya.close()
                helper.reconnect()
                time.sleep(2)
                print("crawler重建")
                crawlerRakuya = RakuyaCrawler(False)
                reconnect_count=0
            time.sleep(1)
        except:
            helper.reconnect()
            reconnect_count=0
    crawlerRakuya.close()
    return save
        
def start():
    start_time = datetime.now()
    county_list = ["屏東縣"]
    for county in county_list:
        for zipCode in CITY.zipCode[county].keys():
            area = CITY.zipCode[county][zipCode]
            save = crawler(zipCode, area, county)
            time.sleep(10)
            print(f"{county}{area}，已存入{save}筆物件")
    execution_time = datetime.now()- start_time
    print(f"花費時間:{execution_time}")

def get_OID(url):
    # print(url)
    url = url.split("ehid=")[-1]
    OID = url.split("&")[0]
    return OID

while True:
    try:
        start()
    except:
        print("error")
    finally:
        time.sleep(500)
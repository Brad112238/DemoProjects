import SQL,time
from datetime import datetime
from bs4 import BeautifulSoup
import re,requests,json
s = requests.session()
headers = {'user-agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/126.0.0.0 Safari/537.36'}

buildType = {
    "住宅大樓":"1",
    "電梯大樓":"1",
    "大樓":"1",
    "華廈":"1",
    "公寓" : "2",
    "套房" :"3",
    "別墅" : "4",
    "透天厝" : "4",
    "店面" : "5",
    "辦公商業大樓" : "1,6",
    "廠房" : "6",
    "農舍" : "7",
    "車位":"7",
    "土地":"8",
    "不限":"8",
    "不限":"8"
}

def get_area(city,area):
    if city =="新竹市":
        return "新竹市"
    if city =="嘉義市":
        return "嘉義市"
    return area

def get_room(room , text):
    try:
        if text == "房":
            role = re.compile(r'\d*房')
            count = role.search(room)[0].replace("房","")
            return int(count)
        if text == "廳":
            role = re.compile(r'\d*廳')
            count = role.search(room)[0].replace("廳","")
            return int(count)
        if text == "衛":
            role = re.compile(r'\d*衛')
            count = role.search(room)[0].replace("衛","")
            return int(count)
    except:
        return None

def get_house_area_size(area_text):
    try:
        area_text = area_text.replace("坪"," ")
        return float(area_text)
    except:
        return None

def get_Single_Price(Single_Price_text):
    try:
        Single_Price_text = Single_Price_text.replace("萬/坪" , "")
        return float(Single_Price_text)
    except:
        return None

def get_TargetFloorNumberFrom(floor_text):
    floor_text = floor_text.replace("B" , "-")
    try:
        if "~" in floor_text:
            floor = floor_text.split("~")[0]
            return int(floor)
        else :
            return int(floor_text)
    except:
        return None
    
def get_TargetFloorNumberTo(floor_text):
    floor_text = floor_text.replace("B" , "-")
    try:
        if "~" in floor_text:
            floor = floor_text.split("~")[-1]
            return int(floor)
        else :
            return int(floor_text)
    except:
        return None
def get_HouseFloorCount(floor_text):
    floor_text = floor_text.replace("B" , "-")
    try:
        return int(floor_text)
    except:
        return None

def get_Feature1(salesHighlights):
    Feature1 = ""
    for i in salesHighlights:
        Feature1 += i+","
    return Feature1

def get_Parking(Parking_text):
    if Parking_text == "無" or Parking_text == "":
        return False
    else :
        return True
def get_img(detail):
    try:
        img = detail["data"][0]["pictures"][0]["url"]
        if "https:" not in img:
            img = "https:" + img,
        return img
    except:
        return "https://s1.hfcdn.com/fp/hf_buy/images/defaultImage.jpg"
    
def get_street(address,city,area):
    street = address.replace(city , "")
    street = street.replace(area , "")
    return street

class organize():
    def __init__(self,CITY):
        self.city =CITY
        self.OID_list = []
    def organize_data(self, detail, OId,):
        try:
            street = get_street(detail["data"][0]["address"], self.city , detail["data"][0]["district"])
            area = get_area(self.city , detail["data"][0]["district"])
            date = datetime.today().strftime("%Y%m%d_%H%M")
            data={
                    "OId": OId,
                    "CId": 16,
                    "Vision": 16,
                    "CaseName": detail["data"][0]["caseName"],
                    "AgencyPageUrl": f"https://buy.housefun.com.tw/buy/house/{OId}",
                    "ImgUrl": get_img(detail),
                    "BuildingType": buildType[detail["data"][0]["caseTypeName"]],
                    "City": self.city,
                    "Area": area,
                    "Street": street,
                    "Address": detail["data"][0]["address"],
                    "TotalAreaSize": detail["data"][0]["regArea"],
                    "MainAreaSize": get_house_area_size(detail["data"][0]["tranScript"]["mainArea"]),
                    "RoomCount": get_room(detail["data"][0]["patternShow"],"房"),
                    "HallCount": get_room(detail["data"][0]["patternShow"],"廳"),
                    "BathroomCount": get_room(detail["data"][0]["patternShow"],"衛"),
                    "TargetFloorNumberFrom": get_TargetFloorNumberFrom(detail["data"][0]["floorShow"].split("/")[0]),
                    "TargetFloorNumberTo": get_TargetFloorNumberTo(detail["data"][0]["floorShow"].split("/")[0]),
                    "HouseFloorCount": get_HouseFloorCount(detail["data"][0]["floorShow"].split("/")[-1]),
                    "Parking": get_Parking(detail["data"][0]["parkingType"]),
                    "HouseYear": detail["data"][0]["buildingAge"],
                    "Longitude": detail["data"][0]["longitude"],
                    "Latitude": detail["data"][0]["latitude"],
                    "TotalPrice": detail["data"][0]["price"],
                    "SinglePrice": get_Single_Price(detail["data"][0]["unitPrice"]),
                    "Community": detail["data"][0]["community"]["buildingName"],
                    "Online": 1,
                    "CreateDateTime": date,
                    "RefreshDateTime": date,
                    "LastRefreshDetail": date,
                    "LandAreaSize": get_house_area_size(detail["data"][0]["tranScript"]["landPin"]),
                    "PublicAreaSize": get_house_area_size(detail["data"][0]["tranScript"]["publicArea"]),
                    "AttachSize": get_house_area_size(detail["data"][0]["tranScript"]["porchArea"]),
                    "HousePurpose": detail["data"][0]["purposeShow"],
                    "HouseType": detail["data"][0]["caseTypeName"],
                    "HouseDirection": detail["data"][0]["dirLoca"],
                    "HouseMaterial": detail["data"][0]["mainMaterial"],
                    "ManagementType": detail["data"][0]["manageType"],
                    "MonthlyPayGuard": detail["data"][0]["manageFee"],
                    "Feature1": get_Feature1(detail["data"][0]["salesHighlights"]),
                    "AgencyCompanyName": detail["data"][0]["agent"]["company"],
                    "AgencyName": detail["data"][0]["agent"]["brand"],
                    "AgencyStoreName": detail["data"][0]["agent"]["shop"],
                    "AgencySalerName": detail["data"][0]["agent"]["name"],
                    "AgencyPhone": None,
                    "AgencyMobile": None,
                    "BatchId": datetime.today().strftime("%Y%m%d"),
                    "ParkingType": detail["data"][0]["parkingType"],
                    "Process":False
                }
            if detail["data"][0]["agent"]["phone"][0:2] == "09":
                data["AgencyMobile"] =detail["data"][0]["agent"]["phone"]
            else:
                data["AgencyPhone"] =detail["data"][0]["agent"]["phone"]
            
            if data["TotalAreaSize"] == 0 and data["LandAreaSize"]>0:
                data["TotalAreaSize"] = data["TotalAreaSize"]
            # print(data)
  
            if data["TotalAreaSize"] > 0: 
                SQL.insert_house_data(data)
                print(f"已存入資料：{data['OId']}")
                return 1
            else:
                print(f"{data['OId']}重複資料!")
                return 0
        except Exception as e :
            print(OId)
            print(e.__traceback__.tb_lineno)
            print(e)
            return 0
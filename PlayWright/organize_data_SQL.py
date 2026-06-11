from datetime import datetime
import SQL
import helper

buildType = {
    "電梯大樓":"1",
    "電梯大廈":"1",
    "華廈":"1",
    "樓中樓":"1",
    "公寓" : "2",
    "套房" : "3",
    "透天厝" : "4",
    "別墅" : "4",
    "店面" : "5",
    "倉庫" : "5",
    "廠房" : "6",
    "辦公" : "6",
    "農舍" : "7",
    "其他" : "7",
    "車位" : "7",
    "土地" : "8",
}

def HouseFeatures(soup):
        try:
            HouseFeatures = soup.find("div", class_="block__info-detail").div.text
            HouseFeatures = HouseFeatures.replace("\n","")
            HouseFeatures = HouseFeatures.replace("\xa0","")
            if len(HouseFeatures) >= 1000:
                HouseFeatures = HouseFeatures[0:999]
            return HouseFeatures.strip()
        except:
            return ""
    
def get_OID(url):
    # print(url)
    url = url.split("ehid=")[-1]
    OID = url.split("&")[0]
    return OID

def get_HouseFeatures(soup):
    return soup.find("div",class_="content").text

def get_area(county,area):
    if county =="新竹市":
        return "新竹市"
    if county =="嘉義市":
        return "嘉義市"
    if area == "三地門":
        return "三地門鄉"
    return area

def extract_street_name(address,county,area):
    address = address.replace(county,"")
    address = address.replace(area,"")
    street_keywords = ["段", "街", "路", "道"]
    max_index = -1

    for keyword in street_keywords:
        index = address.find(keyword) + 1 
        if index >= 2 and index > max_index:  
            max_index = index

    if max_index >= 2:
        return address[:max_index].replace(" ", "")

    return address

def get_size(size):
    try:
        if size == "" or size ==None:
            return None
        size = size.replace("坪","")
        return float(size)
    except:
        return None

def get_TargetFloorNumberFrom(floor):
    try:
        floor = floor.replace("B" , "-")
        if "整棟" in floor:
            return 1
        return int(floor)
    except:
        return None

def get_TargetFloorNumberTo(floor1,floor2,floor3):
    try:
        if "整棟" in floor1:
            return int(floor3)
        if floor2 == "":
            return int(floor1)
        return int(floor1)
    except:
        return None

def get_parking(parking):
    if "有車位" in parking:
        return True
    return False

def get_TotalPrice(TotalPrice):
    try:
        TotalPrice = TotalPrice.replace("," , "")
        return float(TotalPrice)
    except:
        return None
def get_layout(room,living,bath):
    try:
        layout = f"{room}房{living}廳{bath}衛"
        return layout
    except:
        return None

def get_HouseFloorCount(floor):
    try:
        floor = int(floor)
        return floor
    except:
        return None

def get_img(detail):
    try:
        return detail["images"]["photo"][0]["url"]
    except:
        return "https://img.rakuya.com.tw/images/v3/default/default_b.jpg"

def get_age(houseAge):
    try:
        return float(houseAge)
    except:
        return None
    
class organize():
    def __init__(self,CITY):
        self.city =CITY
        self.OID_list = []
    def organize_data(self, url, county, area, type, oid, map, soup):
        date = datetime.today().strftime("%Y%m%d_%H%M")
        try:
            house_detail = helper.get_house_detail(soup)
            saler_detail = helper.get_saler_detail(soup)
            # print(house_detail)
            # print(saler_detail)
            data={
                "OId": oid,
                "CId": 15,
                "Vision": 15,
                "CaseName": house_detail["title"]["hname"],
                "AgencyPageUrl": house_detail["mobileOriginShareUrl"],
                "ImgUrl": get_img(house_detail),
                "BuildingType": buildType[type] or "7",
                "City": county,
                "Area": get_area(county,area),
                "Street": extract_street_name(house_detail["title"]["address"], county ,area),
                "Address": house_detail["title"]["address"],
                "TotalAreaSize": float(house_detail["price"]["totalsize"]),
                "MainAreaSize": get_size(house_detail["detail"]["mainSize"]),
                "RoomCount": house_detail["detail"]["patternBedrooms"],
                "HallCount": house_detail["detail"]["patternLivingrooms"],
                "BathroomCount": house_detail["detail"]["patternBathrooms"],
                "TargetFloorNumberFrom": get_TargetFloorNumberFrom(house_detail["detail"]["transFloors"]),
                "TargetFloorNumberTo": get_TargetFloorNumberTo(house_detail["detail"]["transFloors"] , house_detail["detail"]["maxFloors"] , house_detail["detail"]["surFloors"]),
                "HouseFloorCount": get_HouseFloorCount(house_detail["detail"]["surFloors"]),
                "Parking": get_parking(house_detail["detail"]["parking"]),
                "HouseYear": get_age(house_detail["detail"]["ageValue"]),
                "Longitude": map["data"]["itemLng"] or None ,
                "Latitude": map["data"]["itemLat"] or None ,
                "TotalPrice": get_TotalPrice(house_detail["price"]["price"]),
                "Community": house_detail["title"]["community"] or None,
                "Online": 1,
                "CreateDateTime": date,
                "RefreshDateTime": date,
                "LastRefreshDetail": date,
                "SinglePrice": house_detail["price"]["singlePrice"].replace("," ,""),
                "LandAreaSize": get_size(house_detail["detail"]["baseSize"]),
                "PublicAreaSize": get_size(house_detail["detail"]["shareSize"]),
                "AttachSize": get_size(house_detail["detail"]["subSize"]),
                "HousePurpose": house_detail["detail"]["itemUseType"],
                "HouseType": type,
                "Layout": get_layout(house_detail["detail"]["patternBedrooms"], house_detail["detail"]["patternLivingrooms"], house_detail["detail"]["patternBathrooms"]),
                "HouseDirection": house_detail["detail"]["direction"] or None,
                "Feature1": HouseFeatures(soup),
                "AgencyCompanyName": saler_detail["sellerInfo"]["companyName"],
                "AgencyStoreName": saler_detail["sellerInfo"]["branch"],
                "AgencySalerName": saler_detail["sellerInfo"]["nick"][0:19],
                "AgencyName" : saler_detail["sellerInfo"]["franchiseName"],
                "AgencyPhone": saler_detail["sellerInfo"]["cellPhoneText"] or None,
                "AgencyMobile": saler_detail["sellerInfo"]["phoneText"] or None,
                "AgencyLicense": saler_detail["sellerInfo"]["saleLicense"] or None,
                "BatchId": datetime.today().strftime("%Y%m%d"),
                "ParkingType": house_detail["detail"]["parkingType"],
                "Process":False,
            }
            if "屋主" in saler_detail["sellerInfo"]["identTitle"]:
                data["Vision"] = 1001
            if data["TotalAreaSize"] == 0 and data["LandAreaSize"] >0:
                data["TotalAreaSize"] = data["LandAreaSize"]
            # print(data)
            # return 1

            if data["TotalAreaSize"] > 0:
                # SQL.insert_house_data(data)
                print(f"已存入資料：{data['OId']}")

                return 1
            else:
                return 0
                
        except Exception as e :
            print(url)
            print(e.__traceback__.tb_lineno)
            print(e)
            return 0